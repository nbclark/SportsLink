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
    public class ResultsModel : ModuleModel
    {
        public ResultsModel() { }

        public ResultsModel(TennisUserModel tennisUser, SportsLinkDB db)
            : base(tennisUser)
        {
            this.UserResults = ModelUtils.GetOffers(db, tennisUser).Where(o => o.AcceptUser != null).Where(o => (o.AcceptUser.FacebookId == tennisUser.FacebookId || o.RequestUser.FacebookId == tennisUser.FacebookId)).OrderByDescending(o => o.MatchDateUtc);
        }

        public IQueryable<OfferModel> UserResults { get; private set; }
    }
}