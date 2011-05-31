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
using System.Linq.Expressions;
using System.Data.Linq;
using LinqKit;

namespace SportsLinkWeb.Models
{
    public class ConfirmedMatchesModel : ModuleModel
    {
        private static Func<SportsLinkDB, TennisUserModel, IQueryable<OfferModel>> CachedQuery = null;

        public ConfirmedMatchesModel() { }

        public ConfirmedMatchesModel(TennisUserModel tennisUserP, SportsLinkDB dbP)
            : base(tennisUserP)
        {
            // Select confirmed matchs which do not have a score yet where the current user is either a requestor or an acceptor
            // BUGBUG: we might want to eliminate confirmed matches that way older than current time (user might have forgotten to enter a score)
            if (null == CachedQuery)
            {
                var offers = ModelUtils.GetOffersFunc();

                Expression<Func<SportsLinkDB, TennisUserModel, IQueryable<OfferModel>>> results =
                (SportsLinkDB db, TennisUserModel tennisUser) =>
                    offers.Invoke(db, tennisUser)
                    .Where(o => o.ConfirmedUser != null)
                    .Where(o => (o.ConfirmedUser.FacebookId == tennisUser.FacebookId || o.RequestUser.FacebookId == tennisUser.FacebookId))
                    .Where(o => (o.Score == null || o.Score == ""))
                    .OrderByDescending(o => o.MatchDateUtc);

                CachedQuery = CompiledQuery.Compile<SportsLinkDB, TennisUserModel, IQueryable<OfferModel>>
                (
                    results.Expand()
                );
            }

            this.ConfirmedMatches = CachedQuery(dbP, tennisUserP);
        }

        public IQueryable<OfferModel> ConfirmedMatches { get; private set; }
    }
}