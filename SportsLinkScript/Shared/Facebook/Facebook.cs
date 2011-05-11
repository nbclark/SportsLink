// Utility.cs
//

using System;
using System.Collections;
using System.Html;
using System.Runtime.CompilerServices;
using System.Serialization;
using jQueryApi;

namespace SportsLinkScript.Shared.Facebook
{
    public delegate void FBUiHandler(object response);

    [IgnoreNamespace]
    [Imported]
    public static class FB
    {
        [PreserveCase]
        public static Data Data;
        [PreserveCase]
        public static Event Event;

        public static void Init(JsonObject nameValuePairs)
        {
        }

        public static void Ui(JsonObject nameValuePairs, FBUiHandler handler)
        {
        }

        public static void Api(string path, string method, JsonObject nameValuePairs, FBUiHandler handler)
        {
        }

        public static void GetLoginStatus(FBSubscribeHandler handler) { }
    }
}
