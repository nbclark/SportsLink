// Utility.cs
//

using System;
using System.Collections;
using System.Html;
using System.Runtime.CompilerServices;
using jQueryApi;

namespace SportsLinkScript.Shared
{
    public class SessionContext
    {
        public static SessionContext Instance = new SessionContext();

        public long FamilyId;
        public ArrayList ActiveUsers = new ArrayList();
        public long ChallengId;
    }
}