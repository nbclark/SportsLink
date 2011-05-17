using System;
using System.Collections;
using System.Html;
using System.Net;
using jQueryApi;
using SportsLinkScript.Shared;
using System.Serialization;

namespace SportsLinkScript.Controls
{
    public class PaginatedModule : Module
    {
        protected int Page = 0;
        public string ServiceName;
        public string Filter = string.Empty;

        public PaginatedModule(Element element, string serviceName)
            : base(element)
        {
            this.Page = int.Parse(this.Obj.Find(".page").GetValue());
            this.ServiceName = serviceName;

            jQueryUIObject prev = (jQueryUIObject)this.Obj.Find(".prev");
            jQueryUIObject next = (jQueryUIObject)this.Obj.Find(".next");

            prev.Button();
            next.Button();

            prev.Click(PagePrev);
            next.Click(PageNext);
        }

        private void PagePrev(jQueryEvent e)
        {
            PostBack(this.Page - 1);
        }

        private void PageNext(jQueryEvent e)
        {
            PostBack(this.Page + 1);
        }

        protected void PostBack(int page)
        {
            this.Obj.Attribute("disabled", "disabled").AddClass("ui-state-disabled");

            JsonObject parameters = new JsonObject("page", page, "filter", this.Filter);

            jQuery.Post("/services/" + this.ServiceName + "?signed_request=" + Utility.GetSignedRequest(), Json.Stringify(parameters), (AjaxRequestCallback)delegate(object data, string textStatus, XmlHttpRequest request)
            {
                Utility.ProcessResponse((Dictionary)data);
            }
            );
        }
    }
}