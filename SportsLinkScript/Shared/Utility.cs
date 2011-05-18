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

        internal static void WireLocationAutoComplete(jQueryUIObject autoFill, jQueryUIObject hiddenField)
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
                                        JsonObject placeJson = new JsonObject("id", item.Id, "name", item.Name, "latitude", item.Geometry.Location.Latitude, "longitude", item.Geometry.Location.Longitude);
                                        Script.Alert(Json.Stringify(placeJson));
                                        places.Add(new JsonObject("value", Json.Stringify(placeJson), "label", item.Name, "icon", item.Icon, "description", item.Vicinity));
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

        /// <summary>
        /// Generic response handler for post completions for loaded modules
        /// The index page consists of various modules which have an id of the form "module_ModuleName"
        /// The key ModuleName must be in the dictionary object. 
        /// If we find a matching jQuery object/DOM element with that id, we update the module corresponding to that element.
        /// </summary>
        /// <param name="obj"></param>
        public static void ProcessResponse(Dictionary obj)
        {
            Array keys = StaticUtility.GetKeys(obj);

            for (int i = 0; i <keys.Length; ++i)
            {
                // Try each key and check if the module exists
                string keyId = (string)keys[i];
                string name = "#module_" + keyId;

                // Get the element with the matching name if it exists
                jQueryObject content = jQuery.Select(name);

                if (content.Length > 0)
                {
                    // If element exists, update the element passing in the data which to update it
                    UpdateModule(content, (string)obj[keyId]);

                    // BUGBUG: do we need to break out or can there be multiple modules for which this message is meant for?
                }
            }
        }

        /// <summary>
        /// Creates / loads the JS object (module) corresonding to the HTML element that handles the behavior of the element
        /// </summary>
        /// <param name="element"></param>
        internal static void LoadModule(Element element)
        {
            // The type of the module is embedded in the data-type attribute
            string dataType = (string)element.GetAttribute("data-type");

            if (null != dataType)
            {
                // Construct the type name
                Type type = Type.Parse("SportsLinkScript.Controls." + dataType);

                if (!Script.IsNullOrUndefined(type))
                {
                    // use script# to construct the module
                    Type.CreateInstance(type, element);
                }
            }
        }

        /// <summary>
        /// Updats the HTML element with new HTML
        /// Also, loads the new JS object (module) to handle the behavior
        /// </summary>
        /// <param name="content">The element to be updated</param>
        /// <param name="value">The HTML with which to update it</param>
        private static void UpdateModule(jQueryObject content, string value)
        {
            Module module = null;

            // Find elements which have data-type attribute with any value
            // The attribute is applied to elements that can be updated
            jQueryObject dataTypes = content.Children("*[data-type]");

            // If updatable elements exist, 
            if (dataTypes.Length > 0)
            {
                // Get the first element found
                Element element = dataTypes.First().GetElement(0);

                // Find the corresponding module
                module = Module.GetModule(element);

                // Now we have the module which should be unloaded and then updated
            }

            // Fade the element out and update with the new HTML
            content.FadeOut(500, delegate()
            {
                // Unload the module
                // BUGBUG: when can this be NULL
                if (null != module)
                {
                    module.Unload();
                }

                // Update the content
                content.Html(value);

                content.FadeIn(500);

                // Check if it has any child modules to be loaded
                dataTypes = content.Children("*[data-type]");

                if (dataTypes.Length > 0)
                {
                    // Load only the first
                    LoadModule(dataTypes.First().GetElement(0));

                    // BUGBUG: do we care about the rest?
                }
            });
        }
    }
}
