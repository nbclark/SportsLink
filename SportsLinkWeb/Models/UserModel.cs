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
    public class UserModel
    {
        public UserModel() { }

        public UserModel(User user, TennisUser tennisUser, List<City> neighboringCities)
        {
            this.User = user;
            this.TennisUser = tennisUser;
            this.NeighboringCities = neighboringCities;
        }

        public User User { get; private set; }
        public TennisUser TennisUser { get; private set; }
        public List<City> NeighboringCities { get; private set; }
    }
}