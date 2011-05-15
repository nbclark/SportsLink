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
    public class PlayersModel : ModuleModel
    {
        public PlayersModel() { }

        public PlayersModel(TennisUserModel tennisUser, SportsLinkDB db)
            : base(tennisUser)
        {
            var tennisUsers = ModelUtils.GetTennisUsers(db);

            this.Players = tennisUsers.Where
            (p =>
                Math.Abs(p.Rating - tennisUser.Rating) <= 0.25 &&
                db.CoordinateDistanceMiles(p.City.Latitude, p.City.Longitude, tennisUser.City.Latitude, tennisUser.City.Longitude) < 15 &&
                p.FacebookId != tennisUser.FacebookId &&
                p.Gender == tennisUser.Gender
            );
        }

        public IQueryable<TennisUserModel> Players { get; private set; }
    }
}