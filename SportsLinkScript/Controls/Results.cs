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
    public class Results : Module
    {
        public Results(Element element)
            : base(element)
        {
            jQuery.Select(".inputScore").Click(ReportScore);
        }

        private void ReportScore(jQueryEvent e)
        {
            jQueryObject button = jQuery.FromElement(e.CurrentTarget);
            jQueryUIObject dialog = (jQueryUIObject)jQuery.Select("#scoredialog");

            string score = button.Siblings(".score").GetValue();
            string offerId = button.Siblings(".offerId").GetValue();
            string requestName = button.Siblings(".requestName").GetValue();
            string acceptName = button.Siblings(".acceptName").GetValue();

            dialog.Find("input").Value("");
            string[] scores = score.Split(", ");

            for (int i = 0; i < scores.Length; ++i)
            {
                string[] parts = scores[i].Split('-');

                jQuery.Select("#request" + i).Value(parts[0]);
                jQuery.Select("#accept" + i).Value(parts[1]);
            }

            dialog.Find(".requestName").Html(requestName);
            dialog.Find(".acceptName").Html(acceptName);

            dialog.Dialog(
                new JsonObject(
                    "width", "210",
                    "height", "305",
                    "modal", "true",
                    "buttons", new JsonObject(
                        "Report Score", (jQueryEventHandler)delegate(jQueryEvent ex)
                        {
                            PostResults(dialog, offerId);
                        }
                    ),
                    "position", "top"
                )
            );
        }

        private void PostResults(jQueryUIObject dialog, string offerId)
        {
            string comments = jQuery.Select("#scoreComments").GetValue();

            string score = "";

            for (int i = 0; i < 5; ++i)
            {
                string requestValue = jQuery.Select("#request" + i).GetValue();
                string acceptValue = jQuery.Select("#accept" + i).GetValue();

                if (string.IsNullOrEmpty(requestValue) || string.IsNullOrEmpty(acceptValue))
                {
                    break;
                }

                if (score.Length > 0)
                {
                    score = score + ", ";
                }

                score = score + requestValue + "-" + acceptValue;
            }

            JsonObject parameters = new JsonObject("offerId", offerId, "comments", comments, "scores", score);

            dialog.Attribute("disabled", "disabled").AddClass("ui-state-disabled");
            jQuery.Post("/services/PostScore?signed_request=" + Utility.GetSignedRequest(), Json.Stringify(parameters), (AjaxRequestCallback<object>)delegate(object data, string textStatus, jQueryXmlHttpRequest<object> request)
            {
                dialog.Dialog("destroy");
                Utility.ProcessResponse((Dictionary)data);
            }
            );
        }

        public override void Unload()
        {
            jQueryUIObject dialog = (jQueryUIObject)jQuery.Select("#scoredialog");
            dialog.Remove();

            base.Unload();
        }
    }
}
