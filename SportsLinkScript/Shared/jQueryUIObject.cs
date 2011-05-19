// Utility.cs
//

using System;
using System.Collections;
using System.Html;
using System.Runtime.CompilerServices;
using jQueryApi;

namespace SportsLinkScript.Shared
{
    public delegate object jQuerySelectItemHandler(jQueryEvent ev, object data);

    public delegate object jQueryRenderItemHandler(Element element, JsonObject item);
    [IgnoreNamespace]
    [Imported]
    public class jQueryUIObject : jQueryObject
    {
        public jQueryUIObject Offset(params object[] nameValuePairs) { return null; }
        public jQueryUIObject Dialog(params object[] nameValuePairs) { return null; }
        public jQueryUIObject Button(params object[] nameValuePairs) { return null; }

        [ScriptName("autocomplete")]
        public jQueryUIObject AutoComplete(params object[] nameValuePairs) { return null; }

        [ScriptName("datepicker")]
        public jQueryUIObject DatePicker(params object[] nameValuePairs) { return null; }

        [ScriptName("selectmenu")]
        public jQueryUIObject SelectMenu(params object[] nameValuePairs) { return null; }

        [ScriptName("multiselect")]
        public jQueryUIObject MultiSelect(params object[] nameValuePairs) { return null; }

        [ScriptName("multiselectfilter")]
        public jQueryUIObject MultiSelectFilter() { return null; }

        public jQueryUIObject Data(string key) { return null; }

        [ScriptName("multiselect")]
        public Array MultiSelect(string method) { return null; }

        public object _renderItem;
    }
}
