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
        private int Page = 0;

        public Players(Element element)
            : base(element)
        {
            this.Page = int.Parse(jQuery.Select("#playersPage").GetValue());
            jQueryUIObject requestMatch = (jQueryUIObject)this.Obj.Find(".requestMatch");
            requestMatch.Button(new JsonObject("text", true, "icons", new JsonObject("secondary", "ui-icon-carat-1-e")));
            requestMatch.Click(RequestMatch);

            jQueryUIObject prev = (jQueryUIObject)jQuery.Select("#playersPrev");
            jQueryUIObject next = (jQueryUIObject)jQuery.Select("#playersNext");

            prev.Button();
            next.Button();

            prev.Click(PagePrev);
            next.Click(PageNext);
        }

        private void RequestMatch(jQueryEvent e)
        {
            jQueryObject button = jQuery.FromElement(e.CurrentTarget);
            jQueryUIObject dialog = (jQueryUIObject)jQuery.Select("#challengeDialog");
            jQueryUIObject datePicker = (jQueryUIObject)dialog.Find(".datepicker");

            string id = button.GetElement(0).ID;

            datePicker.DatePicker("disable");

            dialog.Dialog(
                new JsonObject(
                    "width", "260",
                    "height", "254",
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

        private void PagePrev(jQueryEvent e)
        {
            this.Obj.Attribute("disabled", "disabled").AddClass("ui-state-disabled");
            jQueryObject button = jQuery.FromElement(e.CurrentTarget);

            JsonObject parameters = new JsonObject("page", this.Page - 1);

            jQuery.Post("/services/Players", Json.Stringify(parameters), (AjaxRequestCallback)delegate(object data, string textStatus, XmlHttpRequest request)
            {
                Utility.ProcessResponse((Dictionary)data);
            }
            );
        }

        private void PageNext(jQueryEvent e)
        {
            this.Obj.Attribute("disabled", "disabled").AddClass("ui-state-disabled");
            jQueryObject button = jQuery.FromElement(e.CurrentTarget);

            JsonObject parameters = new JsonObject("page", this.Page + 1);

            jQuery.Post("/services/Players", Json.Stringify(parameters), (AjaxRequestCallback)delegate(object data, string textStatus, XmlHttpRequest request)
            {
                Utility.ProcessResponse((Dictionary)data);
            }
            );
        }

        private void CreateMatch(string id)
        {
            jQueryUIObject dialog = (jQueryUIObject)jQuery.Select("#challengeDialog");

            string date = dialog.Find(".datepicker").GetValue();
            string time = dialog.Find(".time").GetValue();
            string ampm = dialog.Find(".ampm").GetValue();
            string comments = dialog.Find(".comments").GetValue();

            string datetime = date + " " + time + ampm;

            ArrayList ids = new ArrayList();
            dialog.Find(".cities input").Each((ElementIterationCallback)delegate(int index, Element element)
            {
                ids.Add(((CheckBoxElement)element).Value);
            });

            JsonObject parameters = new JsonObject("date", datetime, "locations", ids, "comments", comments, "opponentId", 0);
            QuickMatch.DoCreateMatch(dialog, datetime, ids, comments, id, (Callback)delegate() { dialog.Dialog("close"); });
        }
    }
}
