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
    public class QuickMatch : Module
    {
        public QuickMatch(Element element)
            : base(element)
        {
            this.Obj.Find(".findMatch").Click(CreateMatch);
            ((jQueryUIObject)this.Obj.Find(".datepicker")).DatePicker();
            ((jQueryUIObject)this.Obj.Find(".findMatch")).Button();
            ((jQueryUIObject)this.Obj.Find("select")).SelectMenu();
        }

        private void CreateMatch(jQueryEvent e)
        {
            jQueryObject button = jQuery.FromElement(e.CurrentTarget);
            jQueryObject module = button.Parents(".module").First();

            string date = module.Find(".datepicker").GetValue();
            string time = module.Find(".time").GetValue();
            string ampm = module.Find(".ampm").GetValue();
            string comments = module.Find(".comments").GetValue();

            string datetime = date + " " + time + ampm;

            ArrayList ids = new ArrayList();
            module.Find(".cities input").Each((ElementIterationCallback)delegate(int index, Element element)
            {
                ids.Add(((CheckBoxElement)element).Value);
            });

            DoCreateMatch(this.Obj, datetime, ids, comments, 0, null);
        }

        public static void DoCreateMatch(jQueryObject obj, string datetime, object ids, string comments, object opponentId, Callback callback)
        {
            JsonObject parameters = new JsonObject("date", datetime, "locations", ids, "comments", comments, "opponentId", opponentId);

            obj.Attribute("disabled", "disabled").AddClass("ui-state-disabled");
            jQuery.Post("/services/CreateOffer", Json.Stringify(parameters), (AjaxRequestCallback)delegate(object data, string textStatus, XmlHttpRequest request)
            {
                obj.Attribute("disabled", "").RemoveClass("ui-state-disabled");
                Utility.ProcessResponse((Dictionary)data);

                if (null != callback)
                {
                    callback();
                }
            }
            );
        }
    }
}
