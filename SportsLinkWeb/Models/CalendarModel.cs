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
    public class CalendarModel : ModuleModel
    {
        private static Func<SportsLinkDB, TennisUserModel, DateTime, DateTime, IQueryable<OfferModel>> CachedQuery = null;

        public CalendarModel() { }

        public CalendarModel(DateTime startDateP, TennisUserModel tennisUserP, SportsLinkDB dbP)
            : base(tennisUserP)
        {
            this.UserStartDate = IndexModel.GetLocalDate(startDateP.ToUniversalTime(), tennisUserP.TimeZoneOffset);
            this.UserStartDate = this.UserStartDate.AddDays(-(int)this.UserStartDate.DayOfWeek);
            this.StartDate = IndexModel.GetUtcDate(this.UserStartDate, tennisUserP.TimeZoneOffset);
            this.EndDate = this.StartDate.AddDays(7);

            if (null == CachedQuery)
            {
                var offers = ModelUtils.GetOffersFunc();

                Expression<Func<SportsLinkDB, TennisUserModel, DateTime, DateTime, IQueryable<OfferModel>>> results =
                (SportsLinkDB db, TennisUserModel tennisUser, DateTime startDate, DateTime endDate) =>
                    offers.Invoke(db, tennisUser)
                    .Where(o => o.RequestUser.FacebookId != tennisUser.FacebookId)
                    .Where(o => o.SpecificOpponent == null || o.SpecificOpponent.FacebookId == tennisUser.FacebookId || o.RequestUser.FacebookId == tennisUser.FacebookId)
                    .Where(o => o.ConfirmedUser == null || o.ConfirmedUser.FacebookId == tennisUser.FacebookId || o.RequestUser.FacebookId == tennisUser.FacebookId)
                    .Where(o => o.MatchDateUtc >= startDate)
                    .Where(o => o.MatchDateUtc < endDate)
                    .Where(o => o.MatchDateUtc >= DateTime.UtcNow || null != o.ConfirmedUser)
                    .OrderBy(o => o.MatchDateUtc);

                CachedQuery = CompiledQuery.Compile<SportsLinkDB, TennisUserModel, DateTime, DateTime, IQueryable<OfferModel>>
                (
                    results.Expand()
                );
            }

            this.Offers = CachedQuery(dbP, tennisUserP, this.StartDate, this.EndDate);
        }

        public DateTime UserStartDate { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public IEnumerable<OfferModel> Offers { get; private set; }

        public IEnumerable<OfferModel> GetOffers(int dateOffset, IEnumerable<OfferModel> offers)
        {
            return offers
                .Where(o => o.MatchDateUtc >= this.StartDate.AddDays(dateOffset))
                .Where(o => o.MatchDateUtc < this.StartDate.AddDays(dateOffset + 1))
                .OrderBy(o => o.MatchDateUtc);
        }
    }
}