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
    ///<summary>
    ///Offers ordered by the proposed match time
    ///- made by the user 
    ///- not confirmed 
    ///- the offer time is in the future
    ///</summary>
    public class UserOffersModel : ModuleModel
    {
        private static Func<SportsLinkDB, TennisUserModel, IQueryable<OfferModel>> CachedQuery = null;

        public UserOffersModel() { }

        public UserOffersModel(TennisUserModel tennisUserP, SportsLinkDB dbP)
            : base(tennisUserP)
        {
            // BUGBUG: what about offers which were not confirmed and where the offer time is past - we need to eliminate those from the db
            if (null == CachedQuery)
            {
                var offers = ModelUtils.GetOffersFunc();

                Expression<Func<SportsLinkDB, TennisUserModel, IQueryable<OfferModel>>> results =
                (SportsLinkDB db, TennisUserModel tennisUser) =>
                    offers.Invoke(db, tennisUser)
                    .Where(o => o.ConfirmedUser == null)
                    .Where(o => o.RequestUser.FacebookId == tennisUser.FacebookId  && o.MatchDateUtc > DateTime.UtcNow)
                    .OrderBy(o => o.MatchDateUtc);

                CachedQuery = CompiledQuery.Compile<SportsLinkDB, TennisUserModel, IQueryable<OfferModel>>
                (
                    results.Expand()
                );
            }

            this.UserOffers = CachedQuery(dbP, tennisUserP);
        }

        public IQueryable<OfferModel> UserOffers { get; private set; }
    }
}