// Utility.cs
//

using System;
using System.Collections;
using System.Html;
using System.Runtime.CompilerServices;
using jQueryApi;

namespace SportsLinkScript.Shared
{
    [IgnoreNamespace]
    [Imported]
    public class jQueryUIObject : jQueryObject
    {
        public void Dialog(params object[] nameValuePairs) { }
        public void Button(params object[] nameValuePairs) { }

        [ScriptName("datepicker")]
        public void DatePicker(params object[] nameValuePairs) { }

        [ScriptName("selectmenu")]
        public void SelectMenu(params object[] nameValuePairs) { }
    }
}
