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
    public class Players : Module
    {
        public Players(Element element)
            : base(element)
        {
            jQueryUIObject moreButton = (jQueryUIObject)this.Obj.Find(".more");

            jQueryUIObject requestMatch = (jQueryUIObject)this.Obj.Find(".requestMatch");
            requestMatch.Button(new JsonObject("text", true, "icons", new JsonObject("secondary", "ui-icon-carat-1-e")));
            requestMatch.Click(RequestMatch);

            // playerGridCard
            moreButton.Click(MoreClick);
        }

        private void MoreClick(jQueryEvent e)
        {
            jQueryUIObject dialog = (jQueryUIObject)jQuery.Select("#playerGridCard");
            dialog.Children().First().Html("Loading...");

            JsonObject parameters = new JsonObject("page", 0);

            jQuery.Post("/services/PlayerGrid?signed_request=" + Utility.GetSignedRequest(), Json.Stringify(parameters), (AjaxRequestCallback<object>)delegate(object data, string textStatus, jQueryXmlHttpRequest<object> request)
            {
                Utility.ProcessResponse((Dictionary)data);
            }
            );

            dialog.Dialog(
                new JsonObject(
                    "width", jQuery.Window.GetWidth() - 40,
                    "height", jQuery.Window.GetHeight() - 20,
                    "modal", true,
                    "title", "Similar Players",
                    "open", (Callback)delegate()
                    {
                        dialog.Find(".comments").Focus();
                    }
                )
            );
        }

        public static void RequestMatch(jQueryEvent e)
        {
            jQueryObject button = jQuery.FromElement(e.CurrentTarget);
            jQueryUIObject dialog = (jQueryUIObject)jQuery.Select("#challengeDialog");
            jQueryUIObject datePicker = (jQueryUIObject)dialog.Find(".datepicker");

            Utility.WireLocationAutoComplete((jQueryUIObject)dialog.Find(".placesAutoFill"), (jQueryUIObject)dialog.Find(".placesAutoValue"));

            string id = button.GetElement(0).ID;

            datePicker.DatePicker("disable");

            dialog.Dialog(
                new JsonObject(
                    "width", "260",
                    "height", "324",
                    "modal", true,
                    "title",  button.GetAttribute("Title"),
                    "buttons", new JsonObject(
                        "Challenge!", (jQueryEventHandler)delegate(jQueryEvent ex)
                        {
                            CreateMatch(id);
                        }
                    ),
                    "open", (Callback)delegate()
                    {
                        dialog.Find(".comments").Focus();
                        datePicker.DatePicker("enable");
                    }
                )
            );
        }

        private static void CreateMatch(string id)
        {
            jQueryUIObject dialog = (jQueryUIObject)jQuery.Select("#challengeDialog");

            string date = dialog.Find(".datepicker").GetValue();
            string time = dialog.Find(".time").GetValue();
            string ampm = dialog.Find(".ampm").GetValue();
            string comments = dialog.Find(".comments").GetValue();
            string courtData = dialog.Find(".placesAutoValue").GetValue();

            string datetime = date + " " + time + ampm;

            ArrayList ids = new ArrayList();
            dialog.Find(".cities input").Each((ElementIterationCallback)delegate(int index, Element element)
            {
                ids.Add(((CheckBoxElement)element).Value);
            });

            JsonObject parameters = new JsonObject("date", datetime, "locations", ids, "comments", comments, "opponentId", 0);
            QuickMatch.DoCreateMatch(dialog, datetime, ids, courtData, comments, id, (Callback)delegate() { dialog.Dialog("close"); });
        }
    }
}
