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
    public class OfferModel
    {
        public Guid OfferId;
        public DateTime PostDateUtc;
        public DateTime MatchDateUtc;
        public bool Completed;
        public string Score;
        public string Message;
        public string RequestComments;
        public string AcceptComments;

        public Court Court;
        public City City;
        public TennisUserModel RequestUser;
        public TennisUserModel AcceptUser;
        public TennisUserModel SpecificOpponent;

        public LocationModel GetLocation()
        {
            return (null != this.Court) ?
                new LocationModel() { Name = this.Court.Name, Latitude = this.Court.Latitude, Longitude = this.Court.Longitude } :
                new LocationModel() { Name = this.City.Name, Latitude = this.City.Latitude, Longitude = this.City.Longitude };
        }

        public string GetLocationLink()
        {
            return GetLocationLink(GetLocation());
        }

        public static string GetLocationLink(LocationModel location)
        {
            return string.Format("<a href='{0}' class='maplink' target='_blank'>{1}</a>", location.GetMapLink(), location.Name);
        }
    }

    public class TennisUserModel
    {
        public long FacebookId;
        public DateTime Birthday;
        public bool Gender;
        public string Name;
        public double Rating;
        public string PlayStyle;
        public string SinglesDoubles;
        public long? USTAId;
        public bool CurrentAvailability;
        public Court Court;
        public City City;
        public int TimeZoneOffset;
        public int Age;
    }

    public class LocationModel
    {
        public string Name;
        public double? Latitude;
        public double? Longitude;

        public static LocationModel Create(Offer o)
        {
            return (null != o.Court) ?
                new LocationModel() { Name = o.Court.Name, Latitude = o.Court.Latitude, Longitude = o.Court.Longitude } :
                new LocationModel() { Name = o.City.Name, Latitude = o.City.Latitude, Longitude = o.City.Longitude };
        }
    }
}