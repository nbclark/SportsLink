// Utility.cs
//

using System;
using System.Collections;
using System.Html;
using System.Runtime.CompilerServices;
using jQueryApi;

namespace SportsLinkScript.Shared
{
    public delegate void jQueryAutoCompleteHandler(jQueryAutoCompleteRequest request, jQueryAutoCompleteResponse response);
    public delegate void jQueryAutoCompleteResponse(ArrayList data);

    [IgnoreNamespace]
    [Imported]
    [ScriptName("object")]
    public class jQueryAutoCompleteRequest
    {
        public string Term;
    }

    [IgnoreNamespace]
    [Imported]
    [ScriptName("object")]
    public class jQueryAutoCompleteData : JsonObject
    {
        public jQueryAutoCompleteItem Item;
    }

    [IgnoreNamespace]
    [Imported]
    [ScriptName("object")]
    public class jQueryAutoCompleteItem : JsonObject
    {
        public string Label;
        public string Description;
        public string Icon;
        public string Value;
    }
}
