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
using Facebook.Web;
using Facebook.Web.Mvc;
using SportsLink;
using System.Configuration;

namespace SportsLinkWeb.Controllers
{
    using Models;

    [HandleError]
    public class HomeController : Controller
    {
        public static JavaScriptSerializer Serializer = new JavaScriptSerializer();

        //protected SportsLink.SportsLinkDB DB = new SportsLink.SportsLinkDB(@"Data Source=NCLARK-MOBILE\SQLEXPRESS;Initial Catalog=fbtennis;Integrated Security=True");
        protected SportsLink.SportsLinkDB DB = null;

        public HomeController()
        {
            this.DB = new SportsLink.SportsLinkDB(ConfigurationManager.AppSettings["AzureConnectionString"]);
        }
        
        protected override void Dispose(bool disposing)
        {
            this.DB.Dispose();
            base.Dispose(disposing);
        }

        private City GetCity(RegistrationJson regInfo)
        {
            return this.DB.City.Where(c => c.LocationId == regInfo.Location.Id).FirstOrDefault();
        }

        [CanvasAuthorize(Permissions = "user_birthday,user_location,publish_stream,email")]
        public ActionResult Index()
        {
            var app = new FacebookWebClient();
            var fbContext = FacebookWebContext.Current;

            User user = this.DB.User.Where(tu => tu.FacebookId == fbContext.UserId).FirstOrDefault();
            TennisUser tennisUser = this.DB.TennisUser.Where(tu => tu.FacebookId == fbContext.UserId).FirstOrDefault();

            if (null == tennisUser && null != fbContext.SignedRequest)
            {
                if (!fbContext.SignedRequest.Data.ContainsKey("registration"))
                {
                    return new RedirectResult("/home/register");
                }

                var regInfo = (RegistrationJson)Serializer.Deserialize((string)fbContext.SignedRequest.Data["registration"], typeof(RegistrationJson));

                City city = GetCity(regInfo);

                int timeZoneOffset = 0;

                if (null == city)
                {
                    JsonArray users = (JsonArray)app.Query("SELECT timezone FROM user WHERE uid = " + fbContext.UserId);
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
                user.FacebookId = fbContext.UserId;
                user.Name = regInfo.Name;
                user.Email = regInfo.Email;
                user.Birthday = regInfo.Birthday;
                user.Gender = regInfo.Gender == "male";
                user.City = city;
                user.TimeZoneOffset = timeZoneOffset;

                tennisUser = new TennisUser();
                tennisUser.FacebookId = fbContext.UserId;
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
