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
    public class CourtJson
    {
        public string id;
        public string name;
        public float latitude;
        public float longitude;

        public Guid GuidId
        {
            get
            {
                return new Guid(id.Substring(0, 32));
            }
        }
    }
}