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
    internal static class ModelUtils
    {
        public static IQueryable<TennisUserModel> GetTennisUsers(SportsLinkDB db)
        {
            return from u in db.User
                   join tu in db.TennisUser on u.FacebookId equals tu.FacebookId
                   select new TennisUserModel()
                   {
                       FacebookId = u.FacebookId,
                       Birthday = u.Birthday,
                       Gender = u.Gender,
                       Name = u.Name,
                       Rating = tu.Rating,
                       PlayStyle = tu.PlayStyle,
                       SinglesDoubles = tu.SinglesDoubles,
                       USTAId = tu.USTAId,
                       CurrentAvailability = tu.CurrentAvailability,
                       City = u.City
                   };
        }
    }
}