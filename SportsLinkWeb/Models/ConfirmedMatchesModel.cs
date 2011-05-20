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
    public class ConfirmedMatchesModel : ModuleModel
    {
        public ConfirmedMatchesModel() { }

        public ConfirmedMatchesModel(TennisUserModel tennisUser, SportsLinkDB db)
            : base(tennisUser)
        {
            // Select confirmed matchs which do not have a score yet 
            // BUGBUG: eliminate matches that are too old and no score have been reported into 
            // BUGBUG: need to make sure match is confirmed
            // AcceptUser != null may not be sufficient
            this.ConfirmedMatches = ModelUtils.GetOffers(db, tennisUser)
                                              .Where(o => o.AcceptUser != null)
                                              .Where(o => (o.AcceptUser.FacebookId == tennisUser.FacebookId || o.RequestUser.FacebookId == tennisUser.FacebookId))
                                              .Where(o => (o.Score == null || o.Score == ""))
                                              .OrderByDescending(o => o.MatchDateUtc);
        }

        public IQueryable<OfferModel> ConfirmedMatches { get; private set; }
    }
}