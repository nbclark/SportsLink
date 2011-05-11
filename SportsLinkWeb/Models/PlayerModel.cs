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
    public class PlayerModel
    {
        public PlayerModel() { }

        public PlayerModel(User user, TennisUser tennisUser, SportsLinkDB db, int page)
        {
            this.Page = page;

            var tennisUsers = ModelUtils.GetTennisUsers(db);

            var players = tennisUsers.Where
            (p =>
                Math.Abs(p.Rating - tennisUser.Rating) <= 0.25 &&
                db.CoordinateDistanceMiles(p.City.Latitude, p.City.Longitude, user.City.Latitude, user.City.Longitude) < 15 &&
                p.FacebookId != user.FacebookId &&
                p.Gender == user.Gender
            );
            
            this.Count = players.Count();
            this.Players = players.Skip(this.ItemsPerPage * page).Take(this.ItemsPerPage).ToList();
        }

        public List<TennisUserModel> Players { get; private set; }

        public int ItemsPerPage { get { return 20; } }
        public int Count { get; private set ; }
        public int Page { get; private set; }

        public bool HasNext
        {
            get
            {
                return (this.Page+1) * this.ItemsPerPage < this.Count;
            }
        }

        public bool HasPrev
        {
            get
            {
                return this.Page > 0;
            }
        }
    }
}