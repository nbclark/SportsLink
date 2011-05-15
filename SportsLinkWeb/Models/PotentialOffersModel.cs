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
    public class PotentialOffersModel : ModuleModel
    {
        public PotentialOffersModel() { }

        public PotentialOffersModel(TennisUserModel tennisUser, SportsLinkDB db)
            : base(tennisUser)
        {
            this.PotentialOffers = ModelUtils.GetOffers(db, tennisUser).Where
                (o =>
                        o.AcceptUser == null &&
                        (o.SpecificOpponent == null || o.SpecificOpponent.FacebookId == tennisUser.FacebookId) &&
                        o.RequestUser.FacebookId != tennisUser.FacebookId &&
                        o.MatchDateUtc >= DateTime.UtcNow &&
                        Math.Abs(tennisUser.Rating - o.RequestUser.Rating) <= 0.25 &&
                        db.CoordinateDistanceMiles(o.City.Latitude, o.City.Longitude, tennisUser.City.Latitude, tennisUser.City.Longitude) < 15
                ).OrderBy(o => Math.Abs(tennisUser.Rating - o.RequestUser.Rating)).Take(20);
        }

        public IQueryable<OfferModel> PotentialOffers { get; private set; }
    }
}