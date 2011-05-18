// Utility.cs
//

using System;
using System.Collections;
using System.Html;
using System.Runtime.CompilerServices;
using jQueryApi;

namespace SportsLinkScript.Shared.Google
{
    [IgnoreNamespace]
    [Imported]
    [ScriptName("Object")]
    public class PlacesItem
    {
        public string Vicinity;
        public Array Types;
        public string Icon;
        public string Id;
        public string Name;
        public GeometryItem Geometry;
    }

    [IgnoreNamespace]
    [Imported]
    [ScriptName("Object")]
    public class GeometryItem
    {
        public LocationItem Location;
    }

    [IgnoreNamespace]
    [Imported]
    [ScriptName("Object")]
    public class LocationItem
    {
        [ScriptName("lat")]
        public float Latitude;
        [ScriptName("lng")]
        public float Longitude;
    }

    [IgnoreNamespace]
    [Imported]
    [ScriptName("Object")]
    public class PlacesJsonItem
    {
        public string Id;
        public string Name;
        public float Latitude;
        public float Longitude;
    }
}
