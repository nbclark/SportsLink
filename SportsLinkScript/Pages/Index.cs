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
            });
        }
    }
}
