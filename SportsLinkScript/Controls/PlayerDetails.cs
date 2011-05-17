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
    public class PlayerDetails : Module
    {
        public PlayerDetails(Element element)
            : base(element)
        {
            jQueryUIObject sendMessage = (jQueryUIObject)this.Obj.Find("#playerMessage .sendMessage");
            sendMessage.Button(new JsonObject("text", true, "icons", new JsonObject("secondary", "ui-icon-carat-1-e")));
            sendMessage.Click(SendMessage);
        }

        private void SendMessage(jQueryEvent e)
        {
            jQueryObject button = jQuery.FromElement(e.CurrentTarget);
            jQueryUIObject dialog = (jQueryUIObject)jQuery.Select("#playerDetailsCard");

            string text = jQuery.Select("#playerDetailsCard .comments").GetValue();
            string id = dialog.GetAttribute("data-id");

            Script.Literal("debugger");

            dialog.Attribute("disabled", "disabled").AddClass("ui-state-disabled");
            JsonObject parameters = new JsonObject("userId", id, "comments", text);
            jQuery.Post("/services/SendMessage?signed_request=" + Utility.GetSignedRequest(), Json.Stringify(parameters), (AjaxRequestCallback<object>)delegate(object data, string textStatus, jQueryXmlHttpRequest<object> request)
            {
                dialog.Attribute("disabled", "").RemoveClass("ui-state-disabled");
                dialog.Dialog("close");
                Utility.ProcessResponse((Dictionary)data);
            }
            );
        }
    }
}
