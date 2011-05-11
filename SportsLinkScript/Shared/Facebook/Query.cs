// Utility.cs
//

using System;
using System.Collections;
using System.Html;
using System.Runtime.CompilerServices;
using jQueryApi;

namespace SportsLinkScript.Shared.Facebook
{
    public delegate void FBQueryHandler(Array rows);

    [IgnoreNamespace]
    [Imported]
    public class Query
    {
        public void Wait(FBQueryHandler waiter)
        {
        }
    }
}
