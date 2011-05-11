// HomePage.cs
//

using System;
using System.Collections;
using System.Html;
using System.Net;
using jQueryApi;
using SportsLinkScript.Shared;
using System.Serialization;

namespace SportsLinkScript.Controls
{
    public class UserOffers : Module
    {
        public UserOffers(Element element)
            : base(element)
        {
            jQueryUIObject cancelMatch = (jQueryUIObject)this.Obj.Find(".cancelMatch");
            cancelMatch.Button(new JsonObject("text", false, "icons", new JsonObject("primary", "ui-icon-closethick")));

            //cancelMatch.Click(AcceptMatch);
        }
    }
}
