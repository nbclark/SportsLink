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
    public class IndexModel
    {
        public IndexModel() { }

        public IndexModel(User user, TennisUser tennisUser, SportsLinkDB db)
        {
            var tennisUsers = ModelUtils.GetTennisUsers(db);

            var offers =    from o in db.Offer
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

            // Outstanding offers by the user
            this.UserOffers = offers.Where(o => o.AcceptUser == null).Where(o => o.RequestUser.FacebookId == user.FacebookId && !o.Completed && o.MatchDateUtc > DateTime.UtcNow).OrderBy(o => o.MatchDateUtc).ToList();

            // Completed matches
            this.UserResults = offers.Where(o => o.AcceptUser != null).Where(o => (o.AcceptUser.FacebookId == user.FacebookId || o.RequestUser.FacebookId == user.FacebookId)).OrderByDescending(o => o.MatchDateUtc).ToList();

            this.PotentialOffers = offers.Where
                (o =>
                        o.AcceptUser == null &&
                        (o.SpecificOpponent == null || o.SpecificOpponent.FacebookId == user.FacebookId) &&
                        o.RequestUser.FacebookId != user.FacebookId &&
                        o.MatchDateUtc >= DateTime.UtcNow &&
                        Math.Abs(tennisUser.Rating - o.RequestUser.Rating) <= 0.25 &&
                        db.CoordinateDistanceMiles(o.City.Latitude, o.City.Longitude, user.City.Latitude, user.City.Longitude) < 15
                ).OrderBy(o => Math.Abs(tennisUser.Rating - o.RequestUser.Rating)).Take(20).ToList();
        }

        public List<OfferModel> UserOffers { get; private set; }
        public List<OfferModel> UserResults { get; private set; }
        public List<OfferModel> PotentialOffers { get; private set; }

        public static string FormatRating(double rating)
        {
            if (rating.ToString().EndsWith("25"))
            {
                return string.Format("{0:0.0}-{1:0.0}", rating - .25, rating + .25);
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
    }
}