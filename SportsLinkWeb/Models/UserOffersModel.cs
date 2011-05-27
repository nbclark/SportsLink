using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SportsLink;

namespace SportsLinkWeb.Models
{
    ///<summary>
    ///Offers ordered by the proposed match time
    ///- made by the user 
    ///- not confirmed 
    ///- the offer time is in the future
    ///</summary>
    public class UserOffersModel : ModuleModel
    {
        public UserOffersModel() { }

        public UserOffersModel(TennisUserModel tennisUser, SportsLinkDB db)
            : base(tennisUser)
        {
            // BUGBUG: what about offers which were not confirmed and where the offer time is past - we need to eliminate those from the db
            this.UserOffers = ModelUtils.GetOffers(db, tennisUser)
                                        .Where(o => o.ConfirmedUser == null)
                                        .Where(o => o.RequestUser.FacebookId == tennisUser.FacebookId  && o.MatchDateUtc > DateTime.UtcNow)
                                        .OrderBy(o => o.MatchDateUtc);
        }

        public IQueryable<OfferModel> UserOffers { get; private set; }
    }
}