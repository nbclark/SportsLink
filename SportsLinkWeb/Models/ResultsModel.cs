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
    /// <summary>
    /// Represents all offers for a tennis user where there was a non-empty result.
    /// </summary>
    public class ResultsModel : ModuleModel
    {
        public ResultsModel() { }

        public ResultsModel(TennisUserModel tennisUser, SportsLinkDB db)
            : base(tennisUser)
        {
            this.UserResults = ModelUtils.GetOffers(db, tennisUser)
                                            .Where(o => o.ConfirmedUser != null)
                                            .Where(o => (o.ConfirmedUser.FacebookId == tennisUser.FacebookId || o.RequestUser.FacebookId == tennisUser.FacebookId))
                                            .Where(o => (o.Score != null && o.Score != ""))
                                            .OrderByDescending(o => o.MatchDateUtc);
        }

        public IQueryable<OfferModel> UserResults { get; private set; }
    }
}