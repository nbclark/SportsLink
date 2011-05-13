// HomePage.cs
//

using System;
using System.Html;
using System.Net;
using jQueryApi;
using SportsLinkScript.Shared;
using System.Serialization;

namespace SportsLinkScript.Controls
{
    public class PotentialOffers : Module
    {
        public PotentialOffers(Element element)
            : base(element)
        {
            jQueryUIObject acceptMatch = (jQueryUIObject)this.Obj.Find(".acceptMatch");
            jQueryUIObject rejectMatch = (jQueryUIObject)this.Obj.Find(".rejectMatch");

            acceptMatch.Button(new JsonObject("text", false, "icons", new JsonObject("primary", "ui-icon-check")));
            rejectMatch.Button(new JsonObject("text", false, "icons", new JsonObject("primary", "ui-icon-closethick")));

            acceptMatch.Click(AcceptMatch);
            rejectMatch.Click(RejectMatch);
        }

        private void AcceptMatch(jQueryEvent e)
        {
            jQueryObject button = jQuery.FromElement(e.CurrentTarget);
            string offerId = button.Siblings("input").GetValue();

            jQueryObject parentRow = button.Parents(".offer");
            parentRow.Attribute("disabled", "disabled").AddClass("ui-state-disabled");

            jQuery.Post("/services/AcceptOffer?signed_request=" + Utility.GetSignedRequest(), Json.Stringify(new JsonObject("id", offerId)), (AjaxRequestCallback)delegate(object data, string textStatus, XmlHttpRequest request)
            {
                parentRow.Attribute("disabled", "").RemoveClass("ui-state-disabled");
                button.Parent().Children("a").FadeOut(EffectDuration.Slow);
            });
        }

        private void RejectMatch(jQueryEvent e)
        {
            Script.Alert("reject");
        }
    }
}
