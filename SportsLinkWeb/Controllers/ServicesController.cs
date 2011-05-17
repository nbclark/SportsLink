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

        public ActionResult ServiceProxy(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();

            using (StreamReader responseStream = new StreamReader(response.GetResponseStream()))
            {
                string responseText = responseStream.ReadToEnd();

                JavaScriptResult result = new JavaScriptResult();
                result.Script = responseText;

                return result;
            }
        }

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

                    return Json
                    (
                        new
                        {
                            Results = RenderPartialViewToString("Results", ModelUtils.GetModel<ResultsModel>(FacebookWebContext.Current.UserId, this.DB)),
                            UserOffers = RenderPartialViewToString("UserOffers", ModelUtils.GetModel<UserOffersModel>(FacebookWebContext.Current.UserId, this.DB))
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
                    TennisUserModel tennisUser = ModelUtils.GetTennisUsers(this.DB).Where(u => u.FacebookId == fbContext.UserId).FirstOrDefault();
                    string message = "Unknown Error";

                    if (offer.SpecificOpponentId != null)
                    {
                        if (fbContext.UserId == offer.SpecificOpponentId)
                        {
                            offer.AcceptedById = fbContext.UserId;
                            this.DB.SubmitChanges();

                            SendMatchConfirmation(offer);

                            message = "Offer Accepted...";
                        }

                        // else someone is being sneaky...
                    }
                    else
                    {
                        // Accepts must be confirmed by original poster

                        string subject = string.Format("TennisLink: <fb:name uid='{0}' capitalize='true'></fb:name> Interested in Match", tennisUser.FacebookId);
                        string template = Server.MapPath("/content/matchacceptoffer.htm");

                        SendMessage(new long[] { offer.FacebookId }, subject, template, offer, tennisUser);
                        message = "Match requestor notified.  You will be contacted if he/she accepts...";
                    }

                    if (!Request.IsAjaxRequest())
                    {
                        ViewData.Model = message;
                        return View("Redirect");
                    }

                    return Json
                    (
                        new
                        {
                            PotentialOffers = RenderPartialViewToString("PotentialOffers", ModelUtils.GetModel<PotentialOffersModel>(FacebookWebContext.Current.UserId, this.DB)),
                            Results = RenderPartialViewToString("Results", ModelUtils.GetModel<ResultsModel>(FacebookWebContext.Current.UserId, this.DB))
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

        public ActionResult ConfirmOffer(string id, long uid)
        {
            Guid offerGuid;

            if (Guid.TryParse(id, out offerGuid))
            {
                Offer offer = this.DB.Offer.Where(o => o.OfferId == offerGuid && null == o.AcceptedById).FirstOrDefault();

                if (null != offer)
                {
                    offer.AcceptedById = uid;
                    this.DB.SubmitChanges();

                    SendMatchConfirmation(offer);
                }
            }

            return new RedirectResult("/");
        }

        private void SendMatchConfirmation(Offer offer)
        {
            var fbContext = FacebookWebContext.Current;
            var tennisUsers = ModelUtils.GetTennisUsers(this.DB);
            TennisUserModel tennisUser1 = tennisUsers.Where(u => u.FacebookId == offer.FacebookId).FirstOrDefault();
            TennisUserModel tennisUser2 = tennisUsers.Where(u => u.FacebookId == offer.AcceptedById).FirstOrDefault();

            Dictionary<string, string> tokens = new Dictionary<string, string>();
            tokens.Add("FacebookId1", tennisUser1.FacebookId.ToString());
            tokens.Add("Rating1", IndexModel.FormatRating(tennisUser1.Rating));
            tokens.Add("Name1", tennisUser1.Name.ToString());
            tokens.Add("FacebookId2", tennisUser2.FacebookId.ToString());
            tokens.Add("Rating2", IndexModel.FormatRating(tennisUser2.Rating));
            tokens.Add("Name2", tennisUser2.Name.ToString());

            tokens.Add("Date", IndexModel.FormatDate(offer.MatchDateUtc, tennisUser1.TimeZoneOffset).Replace(",", " @ "));
            tokens.Add("Location", tennisUser1.City.Name);
            tokens.Add("Comments", offer.Message);
            tokens.Add("OfferId", offer.OfferId.ToString());

            string subject = string.Format("TennisLink: Match Scheduled and Confirmed");
            string template = Server.MapPath("/content/matchaccepted.htm");

            SendMessage(new long[] { tennisUser1.FacebookId, tennisUser2.FacebookId }, subject, template, tokens);
        }

        public ActionResult CancelOffer(string offerId)
        {
            Guid offerGuid;

            if (Guid.TryParse(offerId, out offerGuid))
            {
                Offer offer = this.DB.Offer.Where(o => o.OfferId == offerGuid).FirstOrDefault();

                if (null != offer)
                {
                    this.DB.Offer.DeleteOnSubmit(offer);
                    this.DB.SubmitChanges();

                    if (null != offer.AcceptedById)
                    {
                        /*
                        var fbContext = FacebookWebContext.Current;
                        TennisUserModel tennisUser = ModelUtils.GetTennisUsers(this.DB).Where(u => u.FacebookId == fbContext.UserId).FirstOrDefault();

                        string subject = "Match Cancelled";
                        string template = Server.MapPath("/content/matchcancelled.htm");

                        SendMessage(new long[] { offer.AcceptedById.Value }, subject, template, offer, tennisUser);
                        */
                    }

                    return Json
                    (
                        new
                        {
                            UserOffers = RenderPartialViewToString("UserOffers", ModelUtils.GetModel<UserOffersModel>(FacebookWebContext.Current.UserId, this.DB)),
                            Results = RenderPartialViewToString("Results", ModelUtils.GetModel<ResultsModel>(FacebookWebContext.Current.UserId, this.DB))
                        }
                     );
                }
            }

            return Json("");
        }

        public static string GetAccessToken()
        {
            string urlString = string.Concat(AccessTokenUrl, "?type=client_cred&client_id=", FacebookApplication.Current.AppId, "&client_secret=", FacebookApplication.Current.AppSecret);

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
            var fbContext = FacebookWebContext.Current;
            
            TennisUserModel tennisUser = ModelUtils.GetTennisUsers(this.DB).Where(u => u.FacebookId == fbContext.UserId).FirstOrDefault();

            if (null != tennisUser)
            {
                List<long> pushIds = new List<long>();

                //Add the entry, send out the messages
                Offer offer = new Offer();
                offer.OfferId = Guid.NewGuid();
                offer.FacebookId = tennisUser.FacebookId;
                offer.MatchDateUtc = date.AddHours(-tennisUser.TimeZoneOffset);
                offer.PostDateUtc = DateTime.UtcNow;
                offer.Message = comments;
                offer.PreferredLocationId = tennisUser.City.LocationId;
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
                        this.DB.CoordinateDistanceMiles(u.City.Latitude, u.City.Longitude, tennisUser.City.Latitude, tennisUser.City.Longitude) < 15 &&
                        Math.Abs(tennisUser.Rating - u.Rating) <= 0.25 &&
                        u.Gender == tennisUser.Gender &&
                        u.FacebookId != tennisUser.FacebookId
                    );

                    foreach (var tu in matchUsers)
                    {
                        pushIds.Add(tu.FacebookId);
                    }
                }

                this.DB.Offer.InsertOnSubmit(offer);
                this.DB.SubmitChanges();

                string template = Server.MapPath("/content/matchrequest.htm");
                string subject = string.Format("TennisLink: Match Requested from <fb:name uid='{0}' capitalize='true'></fb:name>", tennisUser.FacebookId);
                SendMessage(pushIds, subject, template, offer, tennisUser);

                // Send out messages to all matching users

                return Json
                (
                    new
                    {
                        UserOffers = RenderPartialViewToString("UserOffers", ModelUtils.GetModel<UserOffersModel>(FacebookWebContext.Current.UserId, this.DB)),
                        Results = RenderPartialViewToString("Results", ModelUtils.GetModel<ResultsModel>(FacebookWebContext.Current.UserId, this.DB))
                    }
                 );
            }

            return Json("");
        }

        public ActionResult SendMessage(long userId, string comments)
        {
            var fbContext = FacebookWebContext.Current;
            TennisUserModel tennisUser = ModelUtils.GetTennisUsers(this.DB).Where(u => u.FacebookId == fbContext.UserId).FirstOrDefault();
            TennisUserModel sendToUser = ModelUtils.GetTennisUsers(this.DB).Where(u => u.FacebookId == userId).FirstOrDefault();

            if (null != tennisUser && null != sendToUser)
            {
                string template = Server.MapPath("/content/usermessage.htm");
                Dictionary<string, string> tokens = new Dictionary<string, string>();
                tokens.Add("From.Id", tennisUser.FacebookId.ToString());
                tokens.Add("From.Rating", IndexModel.FormatRating(tennisUser.Rating));
                tokens.Add("Message", comments);

                SendMessage(new long[] { userId }, string.Format("TennisLink: Message from <fb:name uid='{0}' capitalize='true'></fb:name>", tennisUser.FacebookId), template, tokens);
            }

            return Json("");
        }

        private static bool SendMessage(IEnumerable<long> pushIds, string subject, string templatePath, Offer offer, TennisUserModel tennisUser)
        {
            Dictionary<string, string> tokens = new Dictionary<string, string>();
            tokens.Add("FacebookId", tennisUser.FacebookId.ToString());
            tokens.Add("Rating", IndexModel.FormatRating(tennisUser.Rating));
            tokens.Add("Date", IndexModel.FormatDate(offer.MatchDateUtc, tennisUser.TimeZoneOffset).Replace(",", " @ "));
            tokens.Add("Name", tennisUser.Name.ToString());
            tokens.Add("Location", tennisUser.City.Name);
            tokens.Add("Comments", offer.Message);
            tokens.Add("OfferId", offer.OfferId.ToString());

            return SendMessage(pushIds, subject, templatePath, tokens);
        }

        private static bool SendMessage(IEnumerable<long> pushIds, string subject, string templatePath, Dictionary<string, string> tokens)
        {
            string accessToken = GetAccessToken();
            FacebookApp postApp = new FacebookApp(accessToken);

            string body = System.IO.File.ReadAllText(templatePath);

            foreach (string key in tokens.Keys)
            {
                body = body.Replace("{" + key + "}", tokens[key]);
            }

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("method", "notifications.sendEmail");
            parameters.Add("recipients", string.Join(",", pushIds));
            parameters.Add("subject", subject);
            parameters.Add("fbml", body);
            dynamic messageResult = postApp.Api("/", parameters, HttpMethod.Post);

            return true;
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
                        PlayerDetails = RenderPartialViewToString("PlayerDetails", ModelUtils.GetModel<ModuleModel>(FacebookWebContext.Current.UserId, this.DB))
                    },
                    JsonRequestBehavior.AllowGet
                 );
            }

            return Json("");
        }

        public ActionResult Players(int page)
        {
            ViewData["Page"] = page;

            return Json
            (
                new
                {
                    Players = RenderPartialViewToString("Players", ModelUtils.GetModel<PlayersModel>(FacebookWebContext.Current.UserId, this.DB))
                }
             );
        }

        public ActionResult UserOffers(int page)
        {
            ViewData["Page"] = page;

            return Json
            (
                new
                {
                    UserOffers = RenderPartialViewToString("UserOffers", ModelUtils.GetModel<UserOffersModel>(FacebookWebContext.Current.UserId, this.DB))
                }
             );
        }

        public ActionResult PotentialOffers(int page)
        {
            ViewData["Page"] = page;

            return Json
            (
                new
                {
                    PotentialOffers = RenderPartialViewToString("PotentialOffers", ModelUtils.GetModel<PotentialOffersModel>(FacebookWebContext.Current.UserId, this.DB))
                }
             );
        }

        public ActionResult Calendar(int page)
        {
            ViewData["Page"] = page;

            var app = new FacebookWebClient();
            var fbContext = FacebookWebContext.Current;

            TennisUserModel existingUser = ModelUtils.GetTennisUsers(this.DB).Where(tu => tu.FacebookId == fbContext.UserId).FirstOrDefault();

            var calendarModel = new CalendarModel(DateTime.Now.AddDays(7 * page), existingUser, this.DB);

            return Json
            (
                new
                {
                    Calendar = RenderPartialViewToString("Calendar", calendarModel)
                }
             );
        }

        public ActionResult PlayerGrid(int page, string filter)
        {
            ViewData["Page"] = page;


            var app = new FacebookWebClient();
            var fbContext = FacebookWebContext.Current;

            TennisUserModel existingUser = ModelUtils.GetTennisUsers(this.DB).Where(tu => tu.FacebookId == fbContext.UserId).FirstOrDefault();
            var model = new PlayersDataGridModel(filter, existingUser, this.DB);
            model.IsPostBack = !string.IsNullOrEmpty(filter);

            return Json
            (
                new
                {
                    PlayerGrid = RenderPartialViewToString("PlayerGrid", model)
                }
             );
        }

    }
}