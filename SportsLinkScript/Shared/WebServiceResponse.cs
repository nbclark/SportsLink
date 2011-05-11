// Utility.cs
//

using System;
using System.Collections;
using System.Html;
using System.Runtime.CompilerServices;
using jQueryApi;

namespace SportsLinkScript.Shared
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    internal class WebServiceResponse
    {
        [ScriptName("d")]
        public object Data;
    }

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    internal class AddUserResponse
    {
        [ScriptName("Item1")]
        public long UserId;
        [ScriptName("Item2")]
        public bool NewUser;
    }
}
