// Utility.cs
//

using System;
using System.Collections;
using System.Html;
using jQueryApi;
using System.Serialization;
using SportsLinkScript.Controls;
using System.Net;

namespace SportsLinkScript.Shared
{
    public static class Utility
    {
        public static void ShowPlayerDetails(string dialogContainerId, string name, long id)
        {
            jQueryUIObject container = (jQueryUIObject)jQuery.Select("#" + dialogContainerId);

            if (container.Length > 0)
            {
                container.Html("Loading...");
                container.Attribute("title", name);

                JsonObject parameters = new JsonObject("id", id);

                jQuery.Post("/services/PlayerDetails", Json.Stringify(parameters), (AjaxRequestCallback)delegate(object data, string textStatus, XmlHttpRequest request)
                {
                    string html = (string)((Dictionary)data)["PlayerDetails"];
                    container.Html(html);
                }
                );

                container.Dialog
                (
                    new JsonObject
                    (
                        "title", name,
                        "width", "340",
                        "height", "150",
                        "modal", "true"
                    )
                );
            }
        }

        public static void ProcessResponse(Dictionary obj)
        {
            Array keys = StaticUtility.GetKeys(obj);

            for (int i = 0; i <keys.Length; ++i)
            {
                string keyId = (string)keys[i];
                string name = "#module_" + keyId;
                jQueryObject content = jQuery.Select(name);

                if (content.Length > 0)
                {
                    UpdateModule(content, (string)obj[keyId]);
                }
            }
        }

        internal static void LoadModule(Element element)
        {
            string dataType = (string)element.GetAttribute("data-type");

            if (null != dataType)
            {
                Type type = Type.Parse("SportsLinkScript.Controls." + dataType);

                if (!Script.IsNullOrUndefined(type))
                {
                    Type.CreateInstance(type, element);
                }
            }
        }

        private static void UpdateModule(jQueryObject content, string value)
        {
            Element element = content.Children().First().GetElement(0);
            Module module = Module.GetModule(element);

            Script.Literal("debugger");

            content.FadeOut(500, (Callback)delegate()
            {
                if (null != module)
                {
                    module.Unload();
                }

                content.Html(value);
                content.FadeIn(500);

                LoadModule(content.Children().First().GetElement(0));
            });
        }
    }
}
