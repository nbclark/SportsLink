using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Text;
using System.Web.Script;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Facebook;
using Facebook.Web.Mvc;
using SportsLink;

namespace SportsLinkWeb.Controllers
{
    using Models;

    [HandleError]
    public class HomeController : Controller
    {
        public static string AppId = "197465840298266";
        public static string ApiKey = "4f175801a1a388540d72445a059bd01d";
        public static string AppSecret = "fb414fe06ea76c51457a7cdef79466ea";
        public static JavaScriptSerializer Serializer = new JavaScriptSerializer();

        //protected SportsLink.SportsLinkDB DB = new SportsLink.SportsLinkDB(@"Data Source=NCLARK-MOBILE\SQLEXPRESS;Initial Catalog=fbtennis;Integrated Security=True");
        protected SportsLink.SportsLinkDB DB = new SportsLink.SportsLinkDB(@"Data Source=e3z720am4s.database.windows.net;Initial Catalog=sportslink;Persist Security Info=True;User ID=fbtennis@live.com@e3z720am4s;Password=May22RTM");
        
        protected override void Dispose(bool disposing)
        {
            this.DB.Dispose();
            base.Dispose(disposing);
        }

        private City GetCity(RegistrationJson regInfo)
        {
            return this.DB.City.Where(c => c.LocationId == regInfo.Location.Id).FirstOrDefault();
        }

        [CanvasAuthorize(Perms="user_birthday,user_location")]
        public ActionResult Index()
        {
            var app = new FacebookApp();

            User user = this.DB.User.Where(tu => tu.FacebookId == app.UserId).FirstOrDefault();
            TennisUser tennisUser = this.DB.TennisUser.Where(tu => tu.FacebookId == app.UserId).FirstOrDefault();

            if (null == tennisUser && null != app.SignedRequest)
            {
                if (!app.SignedRequest.Dictionary.ContainsKey("registration"))
                {
                    return new RedirectResult("/home/register");
                }

                var regInfo = (RegistrationJson)Serializer.Deserialize(app.SignedRequest.Dictionary["registration"], typeof(RegistrationJson));

                City city = GetCity(regInfo);

                int timeZoneOffset = 0;

                if (null == city)
                {
                    JsonArray users = (JsonArray)app.Query("SELECT timezone FROM user WHERE uid = " + app.UserId);
                    JsonArray places = (JsonArray)app.Query("SELECT latitude,longitude FROM place WHERE page_id = " + regInfo.Location.Id);

                    if (null != users && users.Count > 0)
                    {
                        dynamic userData = users[0];
                        timeZoneOffset = Convert.ToInt32(userData.timezone);
                    }

                    if (null != places && places.Count > 0)
                    {
                        dynamic place = places[0];
                        city = new City();
                        city.LocationId = regInfo.Location.Id;
                        city.Name = regInfo.Location.Name;
                        city.Latitude = Convert.ToDouble(place.latitude);
                        city.Longitude = Convert.ToDouble(place.longitude);
                        this.DB.City.InsertOnSubmit(city);
                    }
                }

                user = new User();
                user.FacebookId = app.UserId;
                user.Name = regInfo.Name;
                user.Email = regInfo.Email;
                user.Birthday = regInfo.Birthday;
                user.Gender = regInfo.Gender == "male";
                user.City = city;
                user.TimeZoneOffset = timeZoneOffset;

                tennisUser = new TennisUser();
                tennisUser.FacebookId = app.UserId;
                tennisUser.Rating = regInfo.NTRPRating;
                tennisUser.SinglesDoubles = regInfo.SinglesDoubles;
                tennisUser.CurrentAvailability = true;

                this.DB.User.InsertOnSubmit(user);
                this.DB.TennisUser.InsertOnSubmit(tennisUser);

                this.DB.SubmitChanges();

                return new RedirectResult("http://apps.facebook.com/tennislink");
            }

            if (null == tennisUser)
            {
                return new RedirectResult("/home/register");
            }

            List<City> neighboringCities = this.DB.City.Where(c => this.DB.CoordinateDistanceMiles(user.City.Latitude, user.City.Longitude, c.Latitude, c.Longitude) < 15).OrderBy(c => this.DB.CoordinateDistanceMiles(user.City.Latitude, user.City.Longitude, c.Latitude, c.Longitude)).ToList();

            ViewData["UserModel"] = new UserModel(user, tennisUser, neighboringCities);
            ViewData["IndexModel"] = new IndexModel(user, tennisUser, this.DB);
            ViewData["PlayerModel"] = new PlayerModel(user, tennisUser, this.DB, 0);
            ViewData["Action"] = "Index";

            return View("~/Views/Home/Index.aspx");
        }

        public ActionResult Login()
        {
            ViewData["Action"] = "Login";
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult Register()
        {
            ViewData["Action"] = "Register";
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            ViewData["Action"] = "About";
            return View();
        }

        protected string RenderPartialViewToString(string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
