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
            // Select confirmed matchs which do not have a score yet where the current user is either a requestor or an acceptor
            // BUGBUG: we might want to eliminate confirmed matches that way older than current time (user might have forgotten to enter a score)
            this.ConfirmedMatches = ModelUtils.GetOffers(db, tennisUser)
                                              .Where(o => o.ConfirmedUser != null)
                                              .Where(o => (o.ConfirmedUser.FacebookId == tennisUser.FacebookId || o.RequestUser.FacebookId == tennisUser.FacebookId))
                                              .Where(o => (o.Score == null || o.Score == ""))
                                              .OrderByDescending(o => o.MatchDateUtc);
        }

        public IQueryable<OfferModel> ConfirmedMatches { get; private set; }
    }
}