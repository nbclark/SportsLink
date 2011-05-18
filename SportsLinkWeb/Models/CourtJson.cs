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
using System.Web.Script.Serialization;

namespace SportsLinkWeb.Models
{
    public class CourtJson
    {
        public CourtJson()
        {
        }

        public static CourtJson FromCourt(Court court)
        {
            CourtJson json = new CourtJson();

            if (null != court)
            {
                json.id = court.CourtId.ToString().Replace("-", "");
                json.name = court.Name;
                json.latitude = court.Latitude.Value;
                json.longitude = court.Longitude.Value;
            }

            return json;
        }

        public string id;
        public string name;
        public double latitude;
        public double longitude;

        [ScriptIgnore]
        public Guid GuidId
        {
            get
            {
                return new Guid(id.Substring(0, 32));
            }
        }
    }
}