// Utility.cs
//

using System;
using System.Collections;
using System.Html;
using System.Runtime.CompilerServices;
using jQueryApi;

namespace SportsLinkScript.Shared.Facebook
{
    public delegate void FBSubscribeHandler(object response);
    [IgnoreNamespace]
    [Imported]
    public class Event
    {
        public void Subscribe(string ev, FBSubscribeHandler response)
        {
        }
    }
}
