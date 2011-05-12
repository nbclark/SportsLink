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

            cancelMatch.Click(CancelOffer);
        }

        private void CancelOffer(jQueryEvent e)
        {
            this.Obj.Attribute("disabled", "disabled").AddClass("ui-state-disabled");
            jQueryObject button = jQuery.FromElement(e.CurrentTarget);

            JsonObject parameters = new JsonObject("offerId", button.GetAttribute("data-offerId"));

            jQuery.Post("/services/CancelOffer?signed_request=" + Utility.GetSignedRequest(), Json.Stringify(parameters), (AjaxRequestCallback)delegate(object data, string textStatus, XmlHttpRequest request)
            {
                Utility.ProcessResponse((Dictionary)data);
            }
            );
        }
    }
}
