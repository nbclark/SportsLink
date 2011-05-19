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
    public class IndexModel : ModuleModel
    {
        public IndexModel() { }

        public IndexModel(TennisUserModel tennisUser, SportsLinkDB db)
            : base(tennisUser)
        {
            this.DB = db;
        }

        public SportsLinkDB DB { get; private set; }

        public T GetModel<T>()
        {
            return ModelUtils.GetModel<T>(this.TennisUser, this.DB);
        }

        public static string GetAgeGroup(int age)
        {
            return string.Format("{0}-{1}", age, age + 5);
        }

        public static int GetAge(DateTime birthday)
        {
            int plusOne = (DateTime.Now.DayOfYear < birthday.DayOfYear) ? -1 : 0;

            return (DateTime.Now.Year - birthday.Year + plusOne);
        }

        public static string FormatAge(DateTime birthday)
        {
            return GetAge(birthday).ToString();
        }

        public static string FormatRating(double rating)
        {
            if (rating.ToString().EndsWith("25") || rating.ToString().EndsWith("75"))
            {
                return string.Format("{0:0.0}-{1:0.0}", rating - .25, rating + .25);
            }
            else if (rating == 5.0)
            {
                return "5.0+";
            }
            else
            {
                return string.Format("{0:0.0}", rating);
            }
        }

        public static string FormatDate(DateTime date, int timeZoneOffset)
        {
            DateTime matchDate = date.AddHours(timeZoneOffset);
            DateTime localNow = DateTime.UtcNow.AddHours(timeZoneOffset);

            double dayDiff = (matchDate.Date - localNow.Date).TotalDays;

            if (dayDiff < 1)
            {
                return string.Format("today,{0:h:mm tt}", matchDate);
            }
            else
            {
                return string.Format("{0:ddd,h:mm tt}", matchDate);
            }
        }

        public static string FormatLongDate(DateTime date, int timeZoneOffset)
        {
            DateTime matchDate = date.AddHours(timeZoneOffset);
            DateTime localNow = DateTime.UtcNow.AddHours(timeZoneOffset);

            double dayDiff = (matchDate.Date - localNow.Date).TotalDays;

            if (dayDiff < 1)
            {
                return string.Format("today @ {0:h:mm tt}", matchDate);
            }
            else
            {
                return string.Format("{0:dddd, MMMM d @ h:mm tt}", matchDate);
            }
        }
    }
}