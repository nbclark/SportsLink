// HomePage.cs
//

using System;
using System.Collections;
using System.Html;
using System.Net;
using jQueryApi;
using SportsLinkScript.Shared;
using System.Serialization;
using SportsLinkScript.Shared.Google;

namespace SportsLinkScript.Pages
{
    internal static class Index
    {
        static Index()
        {
            jQuery.AjaxSetup(new jQueryAjaxOptions("contentType", "application/json; charset=utf-8", "dataType", "json"));

            jQuery.OnDocumentReady(delegate()
            {
                // Add script that runs once the document is ready for being
                // consumed by script.
                jQuery.Select(".module").Each((ElementInterruptibleIterationCallback)delegate(int index, Element element)
                {
                    Utility.LoadModule(element);

                    return true;
                });

                jQuery.Select("#header .calendar").Click(Calendar);
            });
        }

        static void Calendar(jQueryEvent ev)
        {
            jQueryUIObject dialog = (jQueryUIObject)jQuery.Select("#calendarCard");
            dialog.Children().First().Html("Loading...");

            JsonObject parameters = new JsonObject("page", 0);

            jQuery.Post("/services/Calendar?signed_request=" + Utility.GetSignedRequest(), Json.Stringify(parameters), (AjaxRequestCallback)delegate(object data, string textStatus, XmlHttpRequest request)
            {
                Utility.ProcessResponse((Dictionary)data);
            }
            );

            dialog.Dialog(
                new JsonObject(
                    "width", jQuery.Window.GetWidth() / 1.5,
                    "height", jQuery.Window.GetHeight() - 20,
                    "modal", true,
                    "title", "Calendar",
                    "open", (Callback)delegate()
                    {
                        dialog.Find(".comments").Focus();
                    }
                )
            );
        }
    }
}
