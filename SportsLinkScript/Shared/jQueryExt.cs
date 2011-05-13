// Utility.cs
//

using System;
using System.Collections;
using System.Html;
using jQueryApi;
using System.Net;
using System.Runtime.CompilerServices;

namespace SportsLinkScript.Shared
{
    [IgnoreNamespace]
    [ScriptName("$")]
    [Imported]
    public static class jQueryExt
    {
        [ScriptName("jsonp")]
        public static XmlHttpRequest GetJsonP(JsonObject obj) { return null; }
    }
}
