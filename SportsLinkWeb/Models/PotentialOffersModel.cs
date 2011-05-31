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
    /// Represents offers that are potentially interesting to a tennis user
    /// - the offer was not posted by the user himself
    /// - where the offerer is of same gender as the user
    /// - where the offer is still active (not confirmed)
    /// - the offer was posted by someone who is within a range of the user's NTRP rating
    /// - the offer location (BUGBUG: check this) is within a radius of the user's location
    /// </summary>
    public class PotentialOffersModel : ModuleModel
    {
        private static Func<SportsLinkDB, TennisUserModel, IQueryable<OfferModel>> CachedQuery = null;

        public PotentialOffersModel() { }

        public PotentialOffersModel(TennisUserModel tennisUserP, SportsLinkDB dbP)
            : base(tennisUserP)
        {
            // BUGBUG: get rid of the hard-coded values
            if (null == CachedQuery)
            {
                var offers = ModelUtils.GetOffersFunc();

                Expression<Func<SportsLinkDB, TennisUserModel, IQueryable<OfferModel>>> results =
                (SportsLinkDB db, TennisUserModel tennisUser) =>
                    offers.Invoke(db, tennisUser)
                    .Where
                    (o =>
                        o.ConfirmedUser == null &&
                        (o.SpecificOpponent == null || o.SpecificOpponent.FacebookId == tennisUser.FacebookId) &&
                        o.RequestUser.FacebookId != tennisUser.FacebookId &&
                        o.MatchDateUtc >= DateTime.UtcNow &&
                        Math.Abs(tennisUser.Rating - o.RequestUser.Rating) <= 0.25 &&
                        db.CoordinateDistanceMiles(o.City.Latitude, o.City.Longitude, tennisUser.City.Latitude, tennisUser.City.Longitude) < 15
                ).OrderBy(o => Math.Abs(tennisUser.Rating - o.RequestUser.Rating)).Take(20);

                CachedQuery = CompiledQuery.Compile<SportsLinkDB, TennisUserModel, IQueryable<OfferModel>>
                (
                    results.Expand()
                );
            }

            this.PotentialOffers = CachedQuery(dbP, tennisUserP);
        }

        public IQueryable<OfferModel> PotentialOffers { get; private set; }
    }
}