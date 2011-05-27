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
    public class CalendarModel : ModuleModel
    {
        public CalendarModel() { }

        public CalendarModel(DateTime startDate, TennisUserModel tennisUser, SportsLinkDB db)
            : base(tennisUser)
        {
            this.UserStartDate = startDate.ToUniversalTime().AddHours(tennisUser.TimeZoneOffset);
            this.UserStartDate = this.UserStartDate.AddDays(-(int)this.UserStartDate.DayOfWeek);
            this.StartDate = this.UserStartDate.AddHours(-tennisUser.TimeZoneOffset);
            this.EndDate = this.StartDate.AddDays(7);

            this.Offers = ModelUtils.GetOffers(db, tennisUser)
                .Where(o => o.RequestUser.FacebookId != tennisUser.FacebookId)
                .Where(o => o.SpecificOpponent == null || o.SpecificOpponent.FacebookId == tennisUser.FacebookId || o.RequestUser.FacebookId == tennisUser.FacebookId)
                .Where(o => o.ConfirmedUser == null || o.ConfirmedUser.FacebookId == tennisUser.FacebookId || o.RequestUser.FacebookId == tennisUser.FacebookId)
                .Where(o => o.MatchDateUtc >= this.StartDate)
                .Where(o => o.MatchDateUtc < this.EndDate)
                .Where(o => o.MatchDateUtc >= DateTime.UtcNow || null != o.ConfirmedUser)
                .OrderBy(o => o.MatchDateUtc).ToList();
        }

        public DateTime UserStartDate { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public IEnumerable<OfferModel> Offers { get; private set; }

        public IEnumerable<OfferModel> GetOffers(int dateOffset)
        {
            return this.Offers
                .Where(o => o.MatchDateUtc >= this.StartDate.AddDays(dateOffset))
                .Where(o => o.MatchDateUtc < this.StartDate.AddDays(dateOffset + 1))
                .OrderBy(o => o.MatchDateUtc);
        }
    }
}