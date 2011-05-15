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
    public class UserOffersModel : ModuleModel
    {
        public UserOffersModel() { }

        public UserOffersModel(TennisUserModel tennisUser, SportsLinkDB db)
            : base(tennisUser)
        {
            // Outstanding offers by the user
            this.UserOffers = ModelUtils.GetOffers(db, tennisUser).Where(o => o.AcceptUser == null).Where(o => o.RequestUser.FacebookId == tennisUser.FacebookId && !o.Completed && o.MatchDateUtc > DateTime.UtcNow).OrderBy(o => o.MatchDateUtc);
        }

        public IQueryable<OfferModel> UserOffers { get; private set; }
    }
}