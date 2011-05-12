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
    /*
    <script>
        $(function () {
            $(".acceptMatch").button({
                text: false,
                icons: {
                    primary: "ui-icon-check"
                }
            });
            $(".rejectMatch").button({
                text: false,
                icons: {
                    primary: "ui-icon-closethick"
                }
            });
        });
    </script>
    */

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

            jQuery.Post("/services/AcceptOffer?signed_request=" + Utility.GetSignedRequest(), Json.Stringify(new JsonObject("offerId", offerId)), (AjaxRequestCallback)delegate(object data, string textStatus, XmlHttpRequest request)
            {
                button.Parent().Children("a").FadeOut(EffectDuration.Slow);
            });
        }

        private void RejectMatch(jQueryEvent e)
        {
            Script.Alert("reject");
        }
    }
}
