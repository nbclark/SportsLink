﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SportsLink;
using System.Data.Linq;
using System.Linq.Expressions;
using LinqKit;

namespace SportsLinkWeb.Models
{
    internal static class ModelUtils
    {
        private static Expression<Func<SportsLinkDB, IQueryable<TennisUserModel>>> TennisUsersQuery = null;
        private static Expression<Func<SportsLinkDB, TennisUserModel, IQueryable<OfferModel>>> OffersQuery = null;

        /// <summary>
        /// Gets all offers for the matching gender of the user passed in
        /// - also for each offer selected, sets a flag whether the user passed in has accepted the offer
        /// - only selects offers for which the offerer is available
        /// </summary>
        /// <param name="db"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static IQueryable<OfferModel> GetOffers(SportsLinkDB db, TennisUserModel user)
        {
            var tennisUsers = ModelUtils.GetTennisUsers(db);

            IQueryable<OfferModel> offers = from o in db.Offer
                         join u1 in tennisUsers on o.FacebookId equals u1.FacebookId
                         join u2 in tennisUsers on o.AcceptedById equals u2.FacebookId
                            into tempUser
                         join accept in db.Accept on o.OfferId equals accept.OfferId
                            into acceptedUsers
                         from confirmedUser in tempUser.DefaultIfEmpty()
                         join u3 in tennisUsers on o.SpecificOpponentId equals u3.FacebookId
                            into tempSpecUser
                         from specificUser in tempSpecUser.DefaultIfEmpty()
                         join c in db.City on o.PreferredLocationId equals c.LocationId
                            into tempCity
                         from city in tempCity.DefaultIfEmpty()
                         join ct in db.Court on o.PreferredCourtId equals ct.CourtId
                            into tempCourt
                         from court in tempCourt.DefaultIfEmpty()

                         where
                            user.Gender == u1.Gender && u1.CurrentAvailability
                            && !acceptedUsers.Any(au => !au.Accepted && au.FacebookId == user.FacebookId)

                         select new OfferModel()
                         {
                             OfferId = o.OfferId,
                             PostDateUtc = o.PostDateUtc,
                             MatchDateUtc = o.MatchDateUtc,
                             Court = court,
                             City = city,
                             Completed = o.Completed,
                             Score = o.Score,
                             Message = o.Message,
                             RequestComments = o.RequestComments,
                             AcceptComments = o.AcceptComments,
                             RequestUser = u1,
                             ConfirmedUser = confirmedUser,
                             AcceptedUsers = acceptedUsers,
                             SpecificOpponent = specificUser,
                             UserPending = db.Accept.Any(a => a.FacebookId == user.FacebookId && a.OfferId == o.OfferId)
                         };

            return offers;
        }

        public static Expression<Func<SportsLinkDB, TennisUserModel, IQueryable<OfferModel>>> GetOffersFunc()
        {
            if (null == OffersQuery)
            {
                var tennisUsers = ModelUtils.GetTennisUsersFunc();

                OffersQuery =
                    (SportsLinkDB db, TennisUserModel user) =>
                        from o in db.Offer
                         join u1 in tennisUsers.Invoke(db) on o.FacebookId equals u1.FacebookId
                        join u2 in tennisUsers.Invoke(db) on o.AcceptedById equals u2.FacebookId
                            into tempUser
                         join accept in db.Accept on o.OfferId equals accept.OfferId
                            into acceptedUsers
                         from confirmedUser in tempUser.DefaultIfEmpty()
                        join u3 in tennisUsers.Invoke(db) on o.SpecificOpponentId equals u3.FacebookId
                            into tempSpecUser
                         from specificUser in tempSpecUser.DefaultIfEmpty()
                         join c in db.City on o.PreferredLocationId equals c.LocationId
                            into tempCity
                         from city in tempCity.DefaultIfEmpty()
                         join ct in db.Court on o.PreferredCourtId equals ct.CourtId
                            into tempCourt
                         from court in tempCourt.DefaultIfEmpty()

                         where
                            user.Gender == u1.Gender && u1.CurrentAvailability
                            && !acceptedUsers.Any(au => !au.Accepted && au.FacebookId == user.FacebookId)

                         select new OfferModel()
                         {
                             OfferId = o.OfferId,
                             PostDateUtc = o.PostDateUtc,
                             MatchDateUtc = o.MatchDateUtc,
                             Court = court,
                             City = city,
                             Completed = o.Completed,
                             Score = o.Score,
                             Message = o.Message,
                             RequestComments = o.RequestComments,
                             AcceptComments = o.AcceptComments,
                             RequestUser = u1,
                             ConfirmedUser = confirmedUser,
                             AcceptedUsers = acceptedUsers,
                             SpecificOpponent = specificUser,
                             UserPending = db.Accept.Any(a => a.FacebookId == user.FacebookId && a.OfferId == o.OfferId)
                         };
            }

            return OffersQuery;
        }

        /// <summary>
        /// Get all users who are tennis users
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<TennisUserModel> GetTennisUsers(SportsLinkDB database)
        {
            GetTennisUsersFunc();

            return TennisUsersQuery.Invoke(database);
        }

        public static Expression<Func<SportsLinkDB, IQueryable<TennisUserModel>>> GetTennisUsersFunc()
        {
            if (null == TennisUsersQuery)
            {
                TennisUsersQuery =
                    (SportsLinkDB db) =>
                        from u in db.User.AsExpandable()
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
                            Court = tu.Court,
                            TimeZoneOffset = u.TimeZoneOffset,
                            EmailOffers = u.EmailOffers,
                            Age = (DateTime.UtcNow.Year - u.Birthday.Year - ((DateTime.UtcNow.DayOfYear < u.Birthday.Year) ? 1 : 0))
                        };
            }

            return TennisUsersQuery;
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