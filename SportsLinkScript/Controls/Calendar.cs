using System;
using System.Html;
using System.Net;
using jQueryApi;
using SportsLinkScript.Shared;
using System.Serialization;
using System.Collections;
using SportsLinkScript.Pages;

namespace SportsLinkScript.Controls
{
    public class Calendar : PotentialOffers
    {
        public Calendar(Element element)
            : base(element)
        {
            this.ServiceName = "Calendar";
        }
    }
}
