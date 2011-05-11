using System;
using System.Collections;
using System.Html;
using System.Net;
using System.Serialization;
using System.Runtime.CompilerServices;
using jQueryApi;
using FamFunScript.Shared;
using FamFunScript.Shared.Facebook;
using FamFunScript.Shared.Google;
using FamFunScript.Shared.Html;
using FamFunActivities;

namespace FamFunScript.Shared
{
    internal class FamFunContext : IFamFunContext
    {
        private jQueryObject Container;
        private JQueryMapObject Map;
        private bool IsMapInitialized = false;
        private int LocationInterval;
        private LocationWatchHandler LocationHandler = null;
        private ArrayList Locations = new ArrayList();
        private DateTime LastUpdate;

        public FamFunContext(jQueryObject container, JQueryMapObject map)
        {
            this.Container = container;
            this.Map = map;
        }

        public void Start()
        {
            this.Resume();
        }

        public Element GetContainer()
        {
            return this.Container[0];
        }

        public void SetMapShow(bool showMap, bool selectBoundary)
        {
            if (showMap)
            {
                if (!this.IsMapInitialized)
                {
                    this.InitMap();
                }
                this.Map.Show();
            }
            else
            {
                this.Map.Hide();
            }
        }

        public void SetMapZoom(int zoomLevel)
        {
        }

        public void AddMapMarker(FamFunActivities.Location location)
        {
        }

        public void AddLocationWatcher(LocationWatchHandler handler)
        {
            this.LocationHandler = handler;
        }

        private void UpdateLocation()
        {
            if ((DateTime.Now - this.LastUpdate) >= 5000)
            {
                this.Locations.Add(this.GetLocation());
                this.LastUpdate = DateTime.Now;
            }

            if (null != this.LocationHandler)
            {
                this.LocationHandler(this.GetLocation());
            }
        }

        public void ClearLocationWatcher()
        {
            this.LocationHandler = null;
        }

        public void Pause()
        {
            Window.ClearInterval(this.LocationInterval);
            this.LocationInterval = 0;
        }

        public void Resume()
        {
            this.Pause();
            this.LocationInterval = Window.SetInterval(UpdateLocation, 500);
        }

        public void End(bool success, bool postToFb)
        {
            this.Pause();
            JQueryMobile.PageLoading(false);

            ArrayList users = (ArrayList)Json.Parse((string)LocalStorage.GetItem("activeUsers"));
            long familyId = (long)LocalStorage.GetItem("familyId");
            long challengeId = (long)LocalStorage.GetItem("challengeId");

            ArrayList userIds = new ArrayList();

            for (int i = 0; i < users.Count; ++i)
            {
                FamilyMember user = (FamilyMember)users[i];
                userIds.Add(user.UserId);
            }

            // todo -- calculate distance between all points

            // CompleteChallenge(long familyId, long[] userIds, long challengeId, float duration, float distance, float calories, float score)
            jQuery.Ajax(new jQueryAjaxOptions(
                "type", "POST",
                "contentType", "application/json; charset=utf-8",
                "url", "/Service.asmx/CompleteChallenge",
                "data", Json.Stringify(new JsonObject("familyId", familyId, "userIds", userIds, "challengeId", challengeId, "completed", success, "duration", 0, "distance", 0, "calories", 0, "score", 0, "locations", this.Locations)),
                "dataType", "json",
                "success", (AjaxRequestCallback)delegate(object data, string textStatus, XmlHttpRequest request)
                {
                    WebServiceResponse response = (WebServiceResponse)data;
                    CompleteChallengeResponse completeChallenge = (CompleteChallengeResponse)response.Data;
                    JQueryMobile.PageLoading(true);
                    Script.Literal("debugger");
                    JQueryMobile.ChangePage("/share/" + completeChallenge.UserChallengeId, "pop", false, false);
                },
                "error", (AjaxRequestCallback)delegate(object data, string textStatus, XmlHttpRequest request)
                {
                    JQueryMobile.PageLoading(true);
                }
            ));
        }

        public void Checkin()
        {
        }

        public Point GetPosition()
        {
            Point p = new Point();
#if (DEBUG)
            p.X = 55;
            p.Y = 55;
#endif
            return p;
        }

        public FamFunActivities.Location GetLocation()
        {
            LatLng latlng = GetLatLng();
            FamFunActivities.Location location = new FamFunActivities.Location();
            location.Latitude = latlng.lat();
            location.Longitude = latlng.lng();
            location.Date = DateTime.Now;

            return location;
        }

        public FamFunActivities.Location PositionToLocation(Point position)
        {
            return null;
        }

        public Point LocationToPosition(FamFunActivities.Location location)
        {
            return null;
        }

        private void InitMap()
        {
            this.Map.CreateMap
            (
                new JsonObject
                (
                    "draggable", false,
                    "disableDefaultUI", true,
                    "disableDoubleClickZoom", true,
                    "scrollwheel", false,
                    "scaleControl", false,
                    "rotateControl", false,
                    "zoomControl", false,
                    "mapTypeId", Script.Literal("google.maps.MapTypeId.HYBRID"),
                    "zoom", 19,
                    "center", GetLatLng(),
                    "callback", (Callback)delegate()
                    {
                        this.Map.AddMapMarker
                        (
                            "addMarker",
                            new JsonObject
                            (
                                "position", GetLatLng(),
                                "animation", Script.Literal("google.maps.Animation.DROP"),
                                "title", "Test Pushpin!"
                            ),
                            (MapMarkerHandler)delegate(object map, object marker)
                            {
                            }
                        );
                    }
                )
            );
        }

        public static LatLng GetLatLng()
        {
            if (Loader.ClientLocation != null)
            {
                return new LatLng(Loader.ClientLocation.Latitude, Loader.ClientLocation.Longitude);
            }
            return new LatLng(59.3426606750f, 18.0736160278f);
        }
    }
}
