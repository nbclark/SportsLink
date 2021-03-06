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
    public class PotentialOffers : PaginatedModule
    {
        public PotentialOffers(Element element)
            : base(element, "PotentialOffers")
        {
            jQueryUIObject acceptMatch = (jQueryUIObject)this.Obj.Find(".acceptMatch");
            jQueryUIObject rejectMatch = (jQueryUIObject)this.Obj.Find(".rejectMatch");
            jQueryUIObject cancelConfirmedMatch = (jQueryUIObject)this.Obj.Find(".cancelConfirmedMatch");
            jQueryUIObject inputScore = (jQueryUIObject)this.Obj.Find(".inputScore");

            acceptMatch.Button(new JsonObject("text", true, "icons", new JsonObject("secondary", "ui-icon-check")));
            rejectMatch.Button(new JsonObject("text", false, "icons", new JsonObject("primary", "ui-icon-closethick")));

            acceptMatch.Click(AcceptMatch);
            rejectMatch.Click(RejectMatch);
            inputScore.Click(ConfirmedMatches.ReportScore);
            cancelConfirmedMatch.Click(ConfirmedMatches.CancelMatch);

            this.Obj.Find(".more").Click(Index.Calendar);
        }

        private void AcceptMatch(jQueryEvent e)
        {
            jQueryObject button = jQuery.FromElement(e.CurrentTarget);
            string offerId = button.Siblings("input").GetValue();

            jQueryObject parentRow = button.Parents(".offer");
            parentRow.Attribute("disabled", "disabled").AddClass("ui-state-disabled");

            jQuery.Post("/services/AcceptOffer?signed_request=" + Utility.GetSignedRequest(), Json.Stringify(new JsonObject("id", offerId)), (AjaxRequestCallback<object>)delegate(object data, string textStatus, jQueryXmlHttpRequest<object> request)
            {
                parentRow.Attribute("disabled", "").RemoveClass("ui-state-disabled");
                button.Parent().Children("a").FadeOut(EffectDuration.Slow);
                button.Parent().Children(".pending").FadeIn(EffectDuration.Fast);

                Utility.ProcessResponse((Dictionary)data);
            });
        }

        private void RejectMatch(jQueryEvent e)
        {
            jQueryObject button = jQuery.FromElement(e.CurrentTarget);
            string offerId = button.Siblings("input").GetValue();

            jQueryObject parentRow = button.Parents(".offer");
            parentRow.Attribute("disabled", "disabled").AddClass("ui-state-disabled");

            jQuery.Post("/services/RejectOffer?signed_request=" + Utility.GetSignedRequest(), Json.Stringify(new JsonObject("id", offerId)), (AjaxRequestCallback<object>)delegate(object data, string textStatus, jQueryXmlHttpRequest<object> request)
            {
                parentRow.Hide();

                Utility.ProcessResponse((Dictionary)data);
            });
        }
    }
}
