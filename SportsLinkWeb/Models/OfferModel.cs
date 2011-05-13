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

        public City City;
        public TennisUserModel RequestUser;
        public TennisUserModel AcceptUser;
        public TennisUserModel SpecificOpponent;
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
        public City City;
        public int TimeZoneOffset;
    }
}