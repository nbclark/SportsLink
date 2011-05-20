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
using System.Text.RegularExpressions;

namespace SportsLinkWeb.Controllers
{
    using Models;

    [HandleError]
    public class ServicesController : HomeController
    {
        private static object LockObject = new object();
        private static DateTime AccessTokenExpiry = DateTime.MinValue;
        private static string AccessToken = string.Empty;
        private const string AccessTokenUrl = "https://graph.facebook.com/oauth/access_token";
        private const string AccessTokenResponsePrefix = "access_token";
        private const string AccessTokenExpiryPrefix = "expires";

        /// <summary>
        /// This method is used to call external web services from the client side.
        /// We need to use a proxy due to cross-site scripting issues.
        /// NOTE: if you can use JSONP instead, use that instead of the proxy
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method is called when user edits the user profile containing NTRP/Preference & other tennis user data
        /// </summary>
        /// <param name="ntrp"></param>
        /// <param name="preference"></param>
        /// <returns></returns>
        public ActionResult PostTennisUserDetails(string ntrp, string preference, string courtData, string style, bool emailOffers)
        {
            var fbContext = FacebookWebContext.Current;

            TennisUser tennisUser = this.DB.TennisUser.Where(u => u.FacebookId == fbContext.UserId).FirstOrDefault();
            User user = this.DB.User.Where(u => u.FacebookId == fbContext.UserId).FirstOrDefault();

            Court court = ProcessCourtData(courtData);

            user.EmailOffers = emailOffers;
            tennisUser.Rating = Convert.ToDouble(ntrp);
            tennisUser.SinglesDoubles = preference;
            tennisUser.Court = court;
            tennisUser.PlayStyle = style;

            this.DB.SubmitChanges();

            // Given the user details has changed, update all modules that are potentially impacted besides the UserDetails module itself
            return Json(
                new
                {
                    UserDetails = RenderPartialViewToString("UserDetails", ModelUtils.GetModel<ModuleModel>(fbContext.UserId, this.DB)),
                    QuickMatch =  RenderPartialViewToString("QuickMatch", ModelUtils.GetModel<ModuleModel>(fbContext.UserId, this.DB)),
                    Players = RenderPartialViewToString("Players", ModelUtils.GetModel<PlayersModel>(FacebookWebContext.Current.UserId, this.DB)),
                    PotentialOffers = RenderPartialViewToString("PotentialOffers", ModelUtils.GetModel<PotentialOffersModel>(FacebookWebContext.Current.UserId, this.DB))
                }
            );
        }
        
        
        /// <summary>
        /// This service is called from the client to post the score for a match
        /// </summary>
        /// <param name="offerId"></param>
        /// <param name="comments"></param>
        /// <param name="scores"></param>
        /// <returns></returns>
        public ActionResult PostScore(string offerId, string comments, string scores)
        {
            Guid offerGuid;

            if (Guid.TryParse(offerId, out offerGuid))
            {
                Offer offer = this.DB.Offer.Where(o => o.OfferId == offerGuid).FirstOrDefault();

                if (null != offer)
                {
                    var ctx = FacebookWebContext.Current;

                    offer.Score = scores;

                    if (offer.FacebookId == ctx.UserId)
                    {
                        offer.RequestComments = comments;
                    }
                    else if (offer.AcceptedById == ctx.UserId)
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
                            ConfirmedMatches = RenderPartialViewToString("ConfirmedMatches", ModelUtils.GetModel<ConfirmedMatchesModel>(FacebookWebContext.Current.UserId, this.DB)),
                            Results = RenderPartialViewToString("Results", ModelUtils.GetModel<ResultsModel>(FacebookWebContext.Current.UserId, this.DB)),
                            UserOffers = RenderPartialViewToString("UserOffers", ModelUtils.GetModel<UserOffersModel>(FacebookWebContext.Current.UserId, this.DB))
                        }
                     );
                }
            }

            return Json("");
        }

        /// <summary>
        /// This method is called from the client (or from an email link) to accept an offer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ActionResult AcceptOffer(string id, long? uid)
        {
            Guid offerGuid;

            if (Guid.TryParse(id, out offerGuid))
            {
                Offer offer = this.DB.Offer.Where(o => o.OfferId == offerGuid && null == o.AcceptedById).FirstOrDefault();

                if (null != offer)
                {
                    var fbContext = FacebookWebContext.Current;

                    if (null != uid && fbContext.UserId != uid.Value)
                    {
                        ViewData.Model = "Wrong User Account...";
                        return View("Redirect");
                    }

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
                        if (!DB.Accept.Any(a => a.FacebookId == fbContext.UserId && a.OfferId == offer.OfferId))
                        {
                            Accept accept = new Accept();
                            accept.FacebookId = fbContext.UserId;
                            accept.OfferId = offer.OfferId;

                            this.DB.Accept.InsertOnSubmit(accept);
                            this.DB.SubmitChanges();

                            string subject = string.Format("TennisLink: <fb:name uid='{0}' capitalize='true'></fb:name> Interested in Match", tennisUser.FacebookId);
                            string template = Server.MapPath("/content/matchacceptoffer.htm");

                            SendMessage(new long[] { offer.FacebookId }, subject, template, offer, tennisUser);
                            message = "Match requestor notified.  You will be contacted if he/she accepts...";
                        }
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
                            ConfirmedMatches = RenderPartialViewToString("ConfirmedMatches", ModelUtils.GetModel<ConfirmedMatchesModel>(FacebookWebContext.Current.UserId, this.DB))
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

        /// <summary>
        /// When a user requests a match to a group of people, when a user accepts, the original poster
        /// is sent an email asking for confirmation.  The link in the confirmation redirects here.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ActionResult ConfirmOffer(string id, long uid)
        {
            Guid offerGuid;

            if (Guid.TryParse(id, out offerGuid))
            {
                var fbContext = FacebookWebContext.Current;
                Offer offer = this.DB.Offer.Where(o => o.OfferId == offerGuid && null == o.AcceptedById).FirstOrDefault();

                if (null != offer && fbContext.UserId == offer.FacebookId)
                {
                    offer.AcceptedById = uid;
                    this.DB.SubmitChanges();

                    SendMatchConfirmation(offer);
                }
            }

            return new RedirectResult("/");
        }

        /// <summary>
        /// This method will send a confirmation mail to both players in a match
        /// </summary>
        /// <param name="offer"></param>
        private void SendMatchConfirmation(Offer offer)
        {
            var fbContext = FacebookWebContext.Current;
            var tennisUsers = ModelUtils.GetTennisUsers(this.DB);
            TennisUserModel tennisUser1 = tennisUsers.Where(u => u.FacebookId == offer.FacebookId).FirstOrDefault();
            TennisUserModel tennisUser2 = tennisUsers.Where(u => u.FacebookId == offer.AcceptedById).FirstOrDefault();

            string location = OfferModel.GetLocationLink(LocationModel.Create(offer));

            Dictionary<string, string> tokens = new Dictionary<string, string>();
            tokens.Add("FacebookId1", tennisUser1.FacebookId.ToString());
            tokens.Add("Rating1", IndexModel.FormatRating(tennisUser1.Rating));
            tokens.Add("Name1", tennisUser1.Name.ToString());
            tokens.Add("FacebookId2", tennisUser2.FacebookId.ToString());
            tokens.Add("Rating2", IndexModel.FormatRating(tennisUser2.Rating));
            tokens.Add("Name2", tennisUser2.Name.ToString());

            tokens.Add("Date", IndexModel.FormatLongDate(offer.MatchDateUtc, tennisUser1.TimeZoneOffset));
            tokens.Add("Location", location);
            tokens.Add("Comments", offer.Message);
            tokens.Add("OfferId", offer.OfferId.ToString());

            string subject = string.Format("TennisLink: Match Scheduled and Confirmed");
            string template = Server.MapPath("/content/matchaccepted.htm");

            SendMessage(new long[] { tennisUser1.FacebookId, tennisUser2.FacebookId }, subject, template, tokens);
        }

        /// <summary>
        /// This method is called from the client to cancel a match request. If the match has been accepted
        /// we need to send mail to both parties.
        /// </summary>
        /// <param name="offerId"></param>
        /// <returns></returns>
        public ActionResult CancelOffer(string offerId)
        {
            Guid offerGuid;

            if (Guid.TryParse(offerId, out offerGuid))
            {
                Offer offer = this.DB.Offer.Where(o => o.OfferId == offerGuid).FirstOrDefault();

                if (null != offer)
                {
                    if (null != offer.AcceptedById)
                    {
                        var fbContext = FacebookWebContext.Current;
                        TennisUserModel tennisUser = ModelUtils.GetTennisUsers(this.DB).Where(u => u.FacebookId == fbContext.UserId).FirstOrDefault();

                        string subject = "TennisLink: Match Cancelled";
                        string template = Server.MapPath("/content/matchcancelled.htm");

                        SendMessage(new long[] { offer.AcceptedById.Value }, subject, template, offer, tennisUser);
                    }

                    this.DB.Offer.DeleteOnSubmit(offer);
                    this.DB.SubmitChanges();

                    return Json
                    (
                        new
                        {
                            ConfirmedMatches = RenderPartialViewToString("ConfirmedMatches", ModelUtils.GetModel<ConfirmedMatchesModel>(FacebookWebContext.Current.UserId, this.DB)),
                            UserOffers = RenderPartialViewToString("UserOffers", ModelUtils.GetModel<UserOffersModel>(FacebookWebContext.Current.UserId, this.DB)),
                        }
                     );
                }
            }

            return Json("");
        }

        /// <summary>
        /// This method contacts FB to get an access token for our application.  The token is used for
        /// sending emails, or doing other activities.
        /// </summary>
        /// <returns></returns>
        public static string GetAccessToken()
        {
            lock (LockObject)
            {
                if (DateTime.Now < AccessTokenExpiry)
                {
                    return AccessToken;
                }

                AccessToken = string.Empty;
                AccessTokenExpiry = DateTime.MinValue;

                string urlString = string.Concat(AccessTokenUrl, "?grant_type=client_credentials&client_id=", FacebookApplication.Current.AppId, "&client_secret=", FacebookApplication.Current.AppSecret);

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlString);
                WebResponse response = request.GetResponse();

                using (StreamReader responseStream = new StreamReader(response.GetResponseStream()))
                {
                    string responseText = responseStream.ReadToEnd();

                    string[] tokens = responseText.Split('&');

                    foreach (string token in tokens)
                    {
                        string[] tokenParts = token.Split('=');

                        if (tokenParts.Length > 1)
                        {
                            string name = tokenParts[0];
                            string value = tokenParts[1];

                            if (string.Equals(name, AccessTokenResponsePrefix, StringComparison.OrdinalIgnoreCase))
                            {
                                AccessToken = value;
                            }
                            else if (string.Equals(name, AccessTokenExpiryPrefix, StringComparison.OrdinalIgnoreCase))
                            {
                                int seconds = Convert.ToInt32(value);

                                // Get the expiration time and buffer it a little
                                AccessTokenExpiry = DateTime.Now.AddSeconds(seconds - 60);
                            }
                        }
                    }

                    return AccessToken;
                }
            }
        }

        public ActionResult CreateOffer(DateTime date, long[] locations, string courtData, string comments, long? opponentId)
        {
            var fbContext = FacebookWebContext.Current;
            
            TennisUserModel tennisUser = ModelUtils.GetTennisUsers(this.DB).Where(u => u.FacebookId == fbContext.UserId).FirstOrDefault();

            if (null != tennisUser)
            {
                List<long> pushIds = new List<long>();
                Court court = ProcessCourtData(courtData);

                if (!string.IsNullOrEmpty(courtData))
                {
                    CourtJson courtJson = JsonSerializer.Current.DeserializeObject<CourtJson>(courtData);

                    if (null != courtJson && !string.IsNullOrEmpty(courtJson.name))
                    {
                        court = this.DB.Court.Where(c => c.CourtId == courtJson.GuidId).FirstOrDefault();

                        if (null == court)
                        {
                            court = new Court();
                            court.CourtId = courtJson.GuidId;
                            court.Name = courtJson.name;
                            court.Latitude = courtJson.latitude;
                            court.Longitude = courtJson.longitude;

                            this.DB.Court.InsertOnSubmit(court);
                        }
                    }
                }

                //Add the entry, send out the messages
                Offer offer = new Offer();
                offer.OfferId = Guid.NewGuid();
                offer.FacebookId = tennisUser.FacebookId;
                offer.MatchDateUtc = date.AddHours(-tennisUser.TimeZoneOffset);
                offer.PostDateUtc = DateTime.UtcNow;
                offer.Message = comments;
                offer.PreferredLocationId = tennisUser.City.LocationId;
                offer.Court = court;
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
                        u.FacebookId != tennisUser.FacebookId &&
                        u.EmailOffers
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
                        UserOffers = RenderPartialViewToString("UserOffers", ModelUtils.GetModel<UserOffersModel>(FacebookWebContext.Current.UserId, this.DB))
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
            string location = OfferModel.GetLocationLink(LocationModel.Create(offer));

            Dictionary<string, string> tokens = new Dictionary<string, string>();
            tokens.Add("FacebookId", tennisUser.FacebookId.ToString());
            tokens.Add("Rating", IndexModel.FormatRating(tennisUser.Rating));
            tokens.Add("Date", IndexModel.FormatLongDate(offer.MatchDateUtc, tennisUser.TimeZoneOffset).Replace(",", " @ "));
            tokens.Add("Name", tennisUser.Name.ToString());
            tokens.Add("Location", location);
            tokens.Add("Comments", offer.Message);
            tokens.Add("OfferId", offer.OfferId.ToString());

            return SendMessage(pushIds, subject, templatePath, tokens);
        }

        private static bool SendMessage(IEnumerable<long> pushIds, string subject, string templatePath, Dictionary<string, string> tokens)
        {
            FacebookOAuthClient oAuth = new FacebookOAuthClient(FacebookApplication.Current);
            dynamic tokenResponse = oAuth.GetApplicationAccessToken();
            string accessToken = tokenResponse.access_token;

            FacebookWebClient postApp = new FacebookWebClient(accessToken);

            string body = System.IO.File.ReadAllText(templatePath);

            tokens.Add("CanvasUrl", FacebookApplication.Current.ReturnUrlPath);

            foreach (string key in tokens.Keys)
            {
                body = body.Replace("{" + key + "}", tokens[key]);
            }

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("method", "notifications.sendEmail");
            parameters.Add("recipients", string.Join(",", pushIds));
            parameters.Add("subject", subject);
            parameters.Add("fbml", body);
            dynamic messageResult = postApp.Post(parameters);

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

        private Court ProcessCourtData(string courtData)
        {
            Court court = null;

            if (!string.IsNullOrEmpty(courtData))
            {
                CourtJson courtJson = JsonSerializer.Current.DeserializeObject<CourtJson>(courtData);

                if (null != courtJson && !string.IsNullOrEmpty(courtJson.name))
                {
                    court = this.DB.Court.Where(c => c.CourtId == courtJson.GuidId).FirstOrDefault();

                    if (null == court)
                    {
                        court = new Court();
                        court.CourtId = courtJson.GuidId;
                        court.Name = courtJson.name;
                        court.Latitude = courtJson.latitude;
                        court.Longitude = courtJson.longitude;

                        this.DB.Court.InsertOnSubmit(court);
                    }
                }
            }

            return court;
        }

    }
}