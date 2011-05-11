using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Text;
using System.Web.Script;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Facebook;
using Facebook.Web.Mvc;
using SportsLink;

namespace SportsLinkWeb.Controllers
{
    using Models;

    [HandleError]
    public class ServicesController : HomeController
    {
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

                    if (offer.FacebookId == app.UserId)
                    {
                        offer.RequestComments = comments;
                    }
                    else if (offer.AcceptedById == app.UserId)
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

        public ActionResult AcceptOffer(string offerId)
        {
            Guid offerGuid;

            if (Guid.TryParse(offerId, out offerGuid))
            {
                Offer offer = this.DB.Offer.Where(o => o.OfferId == offerGuid && null == o.AcceptedById).FirstOrDefault();

                if (null != offer)
                {
                    var app = new FacebookApp();

                    offer.AcceptedById = app.UserId;
                    this.DB.SubmitChanges();

                    ActionResult result = Index();

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

            return Json("");
        }

        public ActionResult CreateOffer(DateTime date, long[] locations, string comments, long? opponentId)
        {
            FacebookApp app = new FacebookApp();
            User user = this.DB.User.Where(u => u.FacebookId == app.UserId).FirstOrDefault();
            TennisUser tennisUser = this.DB.TennisUser.Where(u => u.FacebookId == app.UserId).FirstOrDefault();

            if (null != user && null != tennisUser)
            {
                //Add the entry, send out the messages
                Offer offer = new Offer();
                offer.OfferId = Guid.NewGuid();
                offer.FacebookId = user.FacebookId;
                offer.MatchDateUtc = date.AddHours(-user.TimeZoneOffset);
                offer.PostDateUtc = DateTime.UtcNow;
                offer.Message = comments;
                offer.PreferredLocationId = user.CityId;
                offer.Completed = false;

                if (opponentId.HasValue && opponentId.Value != 0)
                {
                    offer.SpecificOpponentId = opponentId;
                }

                this.DB.Offer.InsertOnSubmit(offer);
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

            return Json("");
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
            User user = this.DB.User.Where(u => u.FacebookId == app.UserId).FirstOrDefault();
            TennisUser tennisUser = this.DB.TennisUser.Where(u => u.FacebookId == app.UserId).FirstOrDefault();

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