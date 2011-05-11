// HomePage.cs
//

using System;
using System.Collections;
using System.Html;
using jQueryApi;
using SportsLinkScript.Shared;

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
