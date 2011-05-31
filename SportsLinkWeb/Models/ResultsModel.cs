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
    /// <summary>
    /// Represents all offers for a tennis user where there was a non-empty result.
    /// </summary>
    public class ResultsModel : ModuleModel
    {
        private static Func<SportsLinkDB, TennisUserModel, IQueryable<OfferModel>> CachedQuery = null;

        public ResultsModel() { }

        public ResultsModel(TennisUserModel tennisUserP, SportsLinkDB dbP)
            : base(tennisUserP)
        {
            if (null == CachedQuery)
            {
                var offers = ModelUtils.GetOffersFunc();

                Expression<Func<SportsLinkDB, TennisUserModel, IQueryable<OfferModel>>> results =
                (SportsLinkDB db, TennisUserModel tennisUser) =>
                    offers.Invoke(db, tennisUser)
                    .Where(o => o.ConfirmedUser != null)
                    .Where(o => (o.ConfirmedUser.FacebookId == tennisUser.FacebookId || o.RequestUser.FacebookId == tennisUser.FacebookId))
                    .Where(o => (o.Score != null && o.Score != ""))
                    .OrderByDescending(o => o.MatchDateUtc);

                CachedQuery = CompiledQuery.Compile<SportsLinkDB, TennisUserModel, IQueryable<OfferModel>>
                (
                    results.Expand()
                );
            }

            this.UserResults = CachedQuery(dbP, tennisUserP);
        }

        public IQueryable<OfferModel> UserResults { get; private set; }
    }
}