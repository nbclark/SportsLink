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
    public static class Extensions
    {
        private static double EarthRadiusInMiles = 3963.1;

        public static double GetDistanceInMiles(this City a, City b)
        {
            var lat1Radians = a.Latitude * Math.PI / 180;
            var long1Radians = a.Longitude * Math.PI / 180;
            var lat2Radians = b.Latitude * Math.PI / 180;
            var long2Radians = b.Longitude * Math.PI / 180;

            return Math.Round(Math.Acos(
            Math.Cos(lat1Radians) * Math.Cos(long1Radians) * Math.Cos(lat2Radians) * Math.Cos(long2Radians) +
            Math.Cos(lat1Radians) * Math.Sin(long1Radians) * Math.Cos(lat2Radians) * Math.Sin(long2Radians) +
            Math.Sin(lat1Radians) * Math.Sin(lat2Radians)
            ) * EarthRadiusInMiles, 1);
        }
    }
}