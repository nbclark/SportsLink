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
        public static IQueryable<OfferModel> GetOffers(SportsLinkDB db, TennisUserModel user)
        {
            var tennisUsers = ModelUtils.GetTennisUsers(db);

            return       from o in db.Offer
                         join u1 in tennisUsers on o.FacebookId equals u1.FacebookId
                         join u2 in tennisUsers on o.AcceptedById equals u2.FacebookId
                         into tempUser
                         from acceptedUser in tempUser.DefaultIfEmpty()
                         join u3 in tennisUsers on o.SpecificOpponentId equals u3.FacebookId
                         into tempSpecUser
                         from specificUser in tempSpecUser.DefaultIfEmpty()
                         join c in db.City on o.PreferredLocationId equals c.LocationId
                         into tempCity
                         from city in tempCity.DefaultIfEmpty()

                         where user.Gender == u1.Gender

                         select new OfferModel()
                         {
                             OfferId = o.OfferId,
                             PostDateUtc = o.PostDateUtc,
                             MatchDateUtc = o.MatchDateUtc,
                             City = city,
                             Completed = o.Completed,
                             Score = o.Score,
                             Message = o.Message,
                             RequestComments = o.RequestComments,
                             AcceptComments = o.AcceptComments,
                             RequestUser = u1,
                             AcceptUser = acceptedUser,
                             SpecificOpponent = specificUser
                         };
        }

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
                       City = u.City,
                       TimeZoneOffset = u.TimeZoneOffset
                   };
        }

        public static T GetModel<T>(long userId, SportsLinkDB db)
        {
            return GetModel<T>(GetTennisUsers(db).Where(u => u.FacebookId == userId).FirstOrDefault(), db);
        }

        public static T GetModel<T>(TennisUserModel tennisUser, SportsLinkDB db)
        {
            return (T)Activator.CreateInstance(typeof(T), tennisUser, db);
        }
    }
}