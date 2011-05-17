// Utility.cs
//

using System;
using System.Collections;
using System.Html;
using jQueryApi;
using System.Serialization;
using SportsLinkScript.Controls;
using System.Net;
using SportsLinkScript.Shared.Google;

namespace SportsLinkScript.Shared
{
    public static class Utility
    {
        private static Dictionary Cache = new Dictionary();
        private static jQueryXmlHttpRequest<object> LastRequest = null;

        internal static string GetSignedRequest()
        {
            return (string)Document.GetElementById("signed_request").GetAttribute("value");
        }

        internal static void WireAutoComplete(jQueryUIObject autoFill, jQueryUIObject hiddenField)
        {
            string accessToken = autoFill.GetAttribute("data-accesstoken");
            string location = autoFill.GetAttribute("data-location");

            autoFill.AutoComplete
            (
                new JsonObject
                (
                    "minLength", 2,
                    "open", (Callback)delegate()
                    {
                        jQuery.This.RemoveClass("ui-corner-all").AddClass("ui-corner-top");
                    },
                    "close", (Callback)delegate()
                    {
                        jQuery.This.RemoveClass("ui-corner-top").AddClass("ui-corner-all");
                    },
                    "select", (jQuerySelectItemHandler)delegate(jQueryEvent ev, object obj)
                    {
                        if (null != hiddenField)
                        {
                            jQueryAutoCompleteData data = (jQueryAutoCompleteData)obj;
                            autoFill.Value(data.Item.Label);

                            hiddenField.Value(data.Item.Value);
                            ev.StopPropagation();
                        }

                        return false;
                    },
                    "source", (jQueryAutoCompleteHandler)delegate(jQueryAutoCompleteRequest request, jQueryAutoCompleteResponse response)
                    {
                        string term = request.Term;

                        if (Cache[term] != null)
                        {
                            response((ArrayList)Cache[term]);
                            return;
                        }

                        LastRequest = jQuery.Post
                        (
                            "/services/serviceproxy",
                            Json.Stringify(new JsonObject("url", "https://maps.googleapis.com/maps/api/place/search/json?location=" + location.EncodeUriComponent() + "&radius=5000&name=" + term.EncodeUriComponent() + "&sensor=false&key=AIzaSyBnD3R38Jh9IhcT7VOJ4Mh8vE7AkSuP_zE")),
                            (AjaxRequestCallback<object>)delegate(object data, string textStatus, jQueryXmlHttpRequest<object> xhr)
                            {
                                // build up the data
                                if (xhr == LastRequest)
                                {
                                    PlacesResponse placesData = (PlacesResponse)data;
                                    ArrayList places = new ArrayList();

                                    for (int i = 0; i < placesData.Results.Length; ++i)
                                    {
                                        PlacesItem item = (PlacesItem)placesData.Results[i];
                                        places.Add(new JsonObject("value", item.Id, "label", item.Name, "icon", item.Icon, "description", item.Vicinity));
                                    }

                                    Cache[term] = places;
                                    response(places);
                                }
                            }
                        );
                    }
                )
            ).Data("autocomplete")._renderItem = (jQueryRenderItemHandler)delegate(Element element, JsonObject item)
            {
                jQueryAutoCompleteItem acItem = (jQueryAutoCompleteItem)item;

                return
                    jQuery.Select("<li class='acItem'></li>")
                    .Data("item.autocomplete", item)
                    .Append("<a><div class='acName'>" + acItem.Label + "</div><div class='acLoc'>" + acItem.Description + "</div></a>")
                    .AppendTo(element);
            };
        }

        public static void ShowPlayerDetails(string dialogContainerId, string name, long id)
        {
            jQueryUIObject container = (jQueryUIObject)jQuery.Select("#" + dialogContainerId);

            if (container.Length > 0)
            {
                container.Children().First().Html("Loading...");
                container.Attribute("title", name);
                container.Attribute("data-id", id.ToString());

                JsonObject parameters = new JsonObject("id", id);

                jQuery.Post("/services/PlayerDetails?signed_request=" + Utility.GetSignedRequest(), Json.Stringify(parameters), (AjaxRequestCallback<object>)delegate(object data, string textStatus, jQueryXmlHttpRequest<object> request)
                {
                    ProcessResponse((Dictionary)data);
                }
                );

                container.Dialog
                (
                    new JsonObject
                    (
                        "title", name,
                        "width", "340",
                        "height", "160",
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
            jQueryObject dataTypes = content.Children("*[data-type]");
            Module module = null;

            if (dataTypes.Length > 0)
            {
                Element element = dataTypes.First().GetElement(0);
                module = Module.GetModule(element);
            }

            content.FadeOut(500, delegate()
            {
                if (null != module)
                {
                    module.Unload();
                }

                content.Html(value);
                content.FadeIn(500);

                dataTypes = content.Children("*[data-type]");

                if (dataTypes.Length > 0)
                {
                    LoadModule(dataTypes.First().GetElement(0));
                }
            });
        }
    }
}
