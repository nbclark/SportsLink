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
    public class RegistrationJson
    {
        public string Name;
        public string Email;
        public DateTime Birthday;
        public string Gender;
        public Location Location;
        public double NTRPRating;
        public string SinglesDoubles;
    }

    public class Location
    {
        public string Name;
        public long Id;
    }
}