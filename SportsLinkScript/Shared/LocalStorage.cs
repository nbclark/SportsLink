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
    [ScriptName("localStorage")]
    public static class LocalStorage
    {
        public static void SetItem(string name, object value) { }
        public static object GetItem(string name) { return null; }
        public static void RemoveItem(string name) { }
        public static void Clear() { }
        public static int Length;
        public static string Key(int index) { return null; }
    }
}
