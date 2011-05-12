using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Text;
using System.Web.Script;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Facebook;
using System.Net;
using Facebook.Web;
using Facebook.Web.Mvc;
using SportsLink;

namespace SportsLinkWeb.Controllers
{
    using Models;

    [HandleError]
    public class ServicesController : HomeController
    {
        private const string FeedUrl = "https://graph.facebook.com/feed";
        private static string AccessToken = string.Empty;
        private const string AccessTokenUrl = "https://graph.facebook.com/oauth/access_token";
        private const string FqlApiUrl = "https://api.facebook.com/method/fql.query?access_token={0}&query={1}";
        private const string PageLikesUrl = "https://api.facebook.com/method/pages.getinfo?fields=page_id%2C+name%2C+page_url%2C+fan_count&format=xml&access_token=";
        private const string AccessTokenResponsePrefix = "access_token=";
        private const string FacebookAppId = "121654811241938";
        private const string FacebookAppSecret = "02b1fc02ef9c2a48510331eac380ecab";

        public ActionResult PostScore(string offerId, string comments, string scores)
        {
            Guid offerGuid;

            if (Guid.TryParse(offerId, out offerGuid))
            {
                Offer offer = this.DB.Offer.Where(o => o.OfferId == offerGuid).FirstOrDefault();

                if (null != offer)
                {
                    var app = new FacebookApp();

                    offer.Score = scores;

                    if (offer.FacebookId == app.Session.UserId)
                    {
                        offer.RequestComments = comments;
                    }
                    else if (offer.AcceptedById == app.Session.UserId)
                    {
                        offer.AcceptComments = comments;
                    }
                    else
                    {
                        return Json("");
                    }

                    this.DB.SubmitChanges();

                    ActionResult result = Index();

                    return Json
                    (
                        new
                        {
                            Results = RenderPartialViewToString("Results"),
                            UserOffers = RenderPartialViewToString("UserOffers")
                        }
                     );
                }
            }

            return Json("");
        }

        public ActionResult AcceptOffer(string id)
        {
            Guid offerGuid;

            if (Guid.TryParse(id, out offerGuid))
            {
                Offer offer = this.DB.Offer.Where(o => o.OfferId == offerGuid && null == o.AcceptedById).FirstOrDefault();

                if (null != offer)
                {
                    var fbContext = FacebookWebContext.Current;

                    offer.AcceptedById = fbContext.UserId;
                    this.DB.SubmitChanges();

                    ActionResult result = Index();

                    if (!Request.IsAjaxRequest())
                    {
                        return new RedirectResult("/");
                    }

                    return Json
                    (
                        new
                        {
                            PotentialMatches = RenderPartialViewToString("PotentialMatches"),
                            Results = RenderPartialViewToString("Results")
                        }
                     );
                }
            }

            if (!Request.IsAjaxRequest())
            {
                return new RedirectResult("/");
            }

            return Json("");
        }

        public ActionResult CancelOffer(string offerId)
        {
            Guid offerGuid;

            if (Guid.TryParse(offerId, out offerGuid))
            {
                Offer offer = this.DB.Offer.Where(o => o.OfferId == offerGuid && null == o.AcceptedById).FirstOrDefault();

                if (null != offer)
                {
                    this.DB.Offer.DeleteOnSubmit(offer);
                    this.DB.SubmitChanges();

                    ActionResult result = Index();

                    return Json
                    (
                        new
                        {
                            UserOffers = RenderPartialViewToString("UserOffers"),
                            Results = RenderPartialViewToString("Results")
                        }
                     );
                }
            }

            return Json("");
        }

        public string GetAccessToken()
        {
            string urlString = string.Concat(AccessTokenUrl, "?type=client_cred&client_id=", "197465840298266", "&client_secret=", "fb414fe06ea76c51457a7cdef79466ea");

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlString);
            WebResponse response = request.GetResponse();

            using (StreamReader responseStream = new StreamReader(response.GetResponseStream()))
            {
                string responseText = responseStream.ReadToEnd();

                if (responseText.StartsWith(AccessTokenResponsePrefix))
                {
                    return responseText.Substring(AccessTokenResponsePrefix.Length);
                }
                else
                {
                    return null;
                }
            }
        }

        public ActionResult CreateOffer(DateTime date, long[] locations, string comments, long? opponentId)
        {
            string accessToken = GetAccessToken();

            FacebookApp postApp = new FacebookApp(accessToken);
            FacebookApp app = new FacebookApp();
            var fbContext = FacebookWebContext.Current;

            User user = this.DB.User.Where(u => u.FacebookId == fbContext.UserId).FirstOrDefault();
            TennisUser tennisUser = this.DB.TennisUser.Where(u => u.FacebookId == fbContext.UserId).FirstOrDefault();

            if (null != user && null != tennisUser)
            {
                List<long> pushIds = new List<long>();

                //Add the entry, send out the messages
                Offer offer = new Offer();
                offer.OfferId = Guid.NewGuid();
                offer.FacebookId = user.FacebookId;
                offer.MatchDateUtc = date.AddHours(-user.TimeZoneOffset);
                offer.PostDateUtc = DateTime.UtcNow;
                offer.Message = comments;
                offer.PreferredLocationId = user.CityId;
                offer.Completed = false;

                User opponent = null;

                if (opponentId.HasValue && opponentId.Value != 0)
                {
                    opponent = this.DB.User.Where(u => u.FacebookId == opponentId.Value).FirstOrDefault();
                }

                if (null != opponent)
                {
                    offer.SpecificOpponentId = opponent.FacebookId;
                    pushIds.Add(opponent.FacebookId);
                }
                else
                {
                    var tennisUsers = ModelUtils.GetTennisUsers(this.DB);
                    var matchUsers = tennisUsers.Where
                    (u =>
                        this.DB.CoordinateDistanceMiles(u.City.Latitude, u.City.Longitude, user.City.Latitude, user.City.Longitude) < 15 &&
                        Math.Abs(tennisUser.Rating - u.Rating) <= 0.25 &&
                        u.Gender == user.Gender &&
                        u.FacebookId != user.FacebookId
                    );

                    foreach (var tu in matchUsers)
                    {
                        pushIds.Add(tu.FacebookId);
                    }
                }

                string requests = "";

                foreach (long id in pushIds)
                {
                    requests = requests + string.Format(@"{{ 'method' : 'POST', 'relative_url' : '{0}/apprequests' }}.", id);

                    //parameters.Add("message", string.Format("{0} ({1}) has requested a match, {2}.", user.Name, IndexModel.FormatRating(tennisUser.Rating), IndexModel.FormatDate(offer.MatchDateUtc, user.TimeZoneOffset).Replace(",", " @ ")));
                    
                }
                Dictionary<string, object> parameters = new Dictionary<string,object>();
                //parameters.Add("batch", "[" + requests.TrimEnd(',') + "]");

                //dynamic messageResult = postApp.Api("/", parameters, HttpMethod.Post);

                //parameters.Add("method", "events.create");
                //parameters.Add("event_info", string.Format(" {{ 'page_id' : '197465840298266', 'name' : 'Test Event', 'start_time' : '{0}', 'privacy_type' : 'CLOSED' }}", DateTime.UtcNow.ToString()));
                //parameters.Add("name", "Test Tennis Match");
                //parameters.Add("start_time", DateTime.UtcNow.ToString());
                //parameters.Add("privacy_type", "CLOSED");
                //parameters.Add("description", "We want you to come play tennis!");

                this.DB.Offer.InsertOnSubmit(offer);
                this.DB.SubmitChanges();

                string bodyPath = Server.MapPath("/content/matchrequest.htm");

                string body = System.IO.File.ReadAllText(bodyPath);
                body = body.Replace("{FacebookId}", user.FacebookId.ToString());
                body = body.Replace("{Rating}", IndexModel.FormatRating(tennisUser.Rating));
                body = body.Replace("{Date}", IndexModel.FormatDate(date, user.TimeZoneOffset).Replace(",", " @ "));
                body = body.Replace("{Name}", user.Name.ToString());
                body = body.Replace("{Location}", user.City.Name);
                body = body.Replace("{Comments}", comments);
                body = body.Replace("{OfferId}", offer.OfferId.ToString());

                parameters.Add("method", "notifications.sendEmail");
                parameters.Add("recipients", string.Join(",", pushIds));
                parameters.Add("subject", string.Format("TennisLink: Match Requested from <fb:name uid='{0}' capitalize='true'></fb:name>", user.FacebookId));
                parameters.Add("fbml", body);
                dynamic messageResult = postApp.Api("/", parameters, HttpMethod.Post);

                // Send out messages to all matching users

                ActionResult result = Index();

                return Json
                (
                    new
                    {
                        UserOffers = RenderPartialViewToString("UserOffers"),
                        Results = RenderPartialViewToString("Results")
                    }
                 );
            }

            return Json("");
        }

        private void SendMatchMessage()
        {
            //
        }

        public ActionResult PlayerDetails(long id)
        {
            TennisUserModel model = ModelUtils.GetTennisUsers(this.DB).Where(u => u.FacebookId == id).FirstOrDefault();

            if (null != model)
            {
                ViewData["PlayerModel"] = model;

                if (!Request.IsAjaxRequest())
                {
                    return View();
                }

                return Json
                (
                    new
                    {
                        PlayerDetails = RenderPartialViewToString("PlayerDetails")
                    },
                    JsonRequestBehavior.AllowGet
                 );
            }

            return Json("");
        }

        public ActionResult Players(int page)
        {
            FacebookApp app = new FacebookApp();
            User user = this.DB.User.Where(u => u.FacebookId == app.Session.UserId).FirstOrDefault();
            TennisUser tennisUser = this.DB.TennisUser.Where(u => u.FacebookId == app.Session.UserId).FirstOrDefault();

            ViewData["PlayerModel"] = new PlayerModel(user, tennisUser, this.DB, page);

            return Json
            (
                new
                {
                    Players = RenderPartialViewToString("Players")
                }
             );
        }

    }
}