using System;
using System.Collections;
using System.Html;
using System.Net;
using jQueryApi;
using SportsLinkScript.Shared;
using System.Serialization;

namespace SportsLinkScript.Controls
{
    /// <summary>
    /// Represents a module that displays content in pages
    /// - contains the functionality to get the new content upon user navigating to a new page
    /// - the paginated module depends upon a hidden input element being present with class "page" and a value equal to the page requested
    /// </summary>
    public class PaginatedModule : Module
    {
        /// <summary>
        /// The current page that the user is being shown
        /// </summary>
        protected int Page = 0;

        /// <summary>
        /// Service to which to post the request
        /// </summary>
        public string ServiceName;

        /// <summary>
        /// Filter for the service request
        /// </summary>
        public string Filter = string.Empty;

        /// <summary>
        /// Initializes the base class of the paginated module with the DOM element and the service details
        /// </summary>
        /// <param name="element"></param>
        /// <param name="serviceName"></param>
        public PaginatedModule(Element element, string serviceName)
            : base(element)
        {
            // Note: The paginated module contains a hidden input with class "page" and a value equal to the page requested
            // Get the page
            this.Page = int.Parse(this.Obj.Find(".page").GetValue());

            // Store the service name
            this.ServiceName = serviceName;

            // Get the jquery objects for the prev/next anchor elements with class prev/next
            jQueryUIObject prev = (jQueryUIObject)this.Obj.Find(".prev");
            jQueryUIObject next = (jQueryUIObject)this.Obj.Find(".next");

            // Make them buttons
            prev.Button();
            next.Button();

            // Hook into the handlers
            prev.Click(PagePrev);
            next.Click(PageNext);
        }

        /// <summary>
        /// Handles the button click event - simply posts a request to the service get the requested page
        /// </summary>
        /// <param name="e"></param>
        private void PagePrev(jQueryEvent e)
        {
            PostBack(this.Page - 1);
        }

        /// <summary>
        /// Handles the button click event - simply posts a request to the service get the requested page
        /// </summary>
        /// <param name="e"></param>
        private void PageNext(jQueryEvent e)
        {
            PostBack(this.Page + 1);
        }

        /// <summary>
        /// Posts the service request with the requested page 
        /// Uses the filter with which it was initialized
        /// </summary>
        /// <param name="page"></param>
        protected void PostBack(int page)
        {
            // Use jquery to set the object as disabled in the DOM before initiating the post
            this.Obj.Attribute("disabled", "disabled").AddClass("ui-state-disabled");

            // Construct the parameters
            JsonObject parameters = new JsonObject("page", page, "filter", this.Filter);

            // Do the post - response will be handled by the generic response handler
            jQuery.Post("/services/" + this.ServiceName + "?signed_request=" + Utility.GetSignedRequest(), Json.Stringify(parameters), (AjaxRequestCallback<object>)delegate(object data, string textStatus, jQueryXmlHttpRequest<object> request)
            {
                Utility.ProcessResponse((Dictionary)data);
            }
            );
        }
    }
}