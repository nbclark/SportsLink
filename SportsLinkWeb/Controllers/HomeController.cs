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
            ViewData["Page"] = 0;
        }
        
        protected override void Dispose(bool disposing)
        {
            this.DB.Dispose();
            base.Dispose(disposing);
        }

        private City GetCity(JsonObject regInfo, out long locationId)
        {
            locationId = regInfo.GetValue<JsonObject>("location").GetValue<long>("id");

            long locId = locationId;
            return this.DB.City.Where(c => c.LocationId == locId).FirstOrDefault();
        }

        public ActionResult Redirect()
        {
            return new RedirectResult(FacebookApplication.Current.ReturnUrlPath);
        }


        [FacebookAuthorize(Permissions = "user_birthday,user_location,publish_stream,email", LoginUrl="/home/login")]
        public ActionResult Index()
        {
            var app = new FacebookWebClient();
            var fbContext = FacebookWebContext.Current;
            ViewData["FirstRun"] = false;

            TennisUserModel existingUser = ModelUtils.GetTennisUsers(this.DB).Where(tu => tu.FacebookId == fbContext.UserId).FirstOrDefault();

            if (null == existingUser && null != fbContext.SignedRequest)
            {
                if (!fbContext.SignedRequest.Data.ContainsKey("registration"))
                {
                    return new RedirectResult("/home/register");
                }

                var regInfo = (JsonObject)fbContext.SignedRequest.Data["registration"];

                long locationId;
                City city = GetCity(regInfo, out locationId);

                double timeZoneOffset = 0;

                JsonArray users = (JsonArray)app.Query("SELECT timezone FROM user WHERE uid = " + fbContext.UserId);

                if (null != users && users.Count > 0)
                {
                    dynamic userData = users[0];
                    timeZoneOffset = Convert.ToDouble(userData.timezone);
                }

                if (null == city)
                {
                    JsonArray places = (JsonArray)app.Query("SELECT latitude,longitude FROM place WHERE page_id = " + locationId);

                    if (null != places && places.Count > 0)
                    {
                        JsonObject location = regInfo.GetValue<JsonObject>("location");
                        dynamic place = places[0];
                        city = new City();
                        city.LocationId = location.GetValue<long>("id");
                        city.Name = location.GetValue<string>("name");
                        city.Latitude = Convert.ToDouble(place.latitude);
                        city.Longitude = Convert.ToDouble(place.longitude);
                        this.DB.City.InsertOnSubmit(city);
                    }
                }

                User user = new User();
                user.FacebookId = fbContext.UserId;
                user.Name = regInfo.GetValue<string>("name");
                user.Email = regInfo.GetValue<string>("email");
                user.Birthday = regInfo.GetValue<DateTime>("birthday");
                user.Gender = regInfo.GetValue<string>("gender") == "male";
                user.City = city;
                user.TimeZoneOffset = timeZoneOffset;
                user.EmailOffers = true;

                TennisUser tennisUser = new TennisUser();
                tennisUser.FacebookId = fbContext.UserId;
                tennisUser.Rating = regInfo.GetValue<double>("NTRPRating");
                tennisUser.SinglesDoubles = regInfo.GetValue<string>("SinglesDoubles");
                tennisUser.CurrentAvailability = true;

                this.DB.User.InsertOnSubmit(user);
                this.DB.TennisUser.InsertOnSubmit(tennisUser);

                this.DB.SubmitChanges();

                return new RedirectResult(Facebook.FacebookApplication.Current.ReturnUrlPath + "?fr=1");
            }

            if (null == existingUser)
            {
                return new RedirectResult("/home/register");
            }

            if (!string.IsNullOrEmpty(Request["fr"]))
            {
                ViewData["FirstRun"] = true;
            }

            ViewData.Model = new IndexModel(existingUser, this.DB);
            ViewData["Action"] = "Index";

            return View("~/Views/Home/Index.aspx");
        }

        public ActionResult DataGrid()
        {
            var app = new FacebookWebClient();
            var fbContext = FacebookWebContext.Current;

            TennisUserModel existingUser = ModelUtils.GetTennisUsers(this.DB).Where(tu => tu.FacebookId == fbContext.UserId).FirstOrDefault();
            //ViewData.Model = new PlayersDataGridModel(existingUser, this.DB);

            return View("DataGrid");
        }

        public ActionResult Login()
        {
            ViewData["Action"] = "Login";

            return View();
        }

        public ActionResult Register()
        {
            ViewData["Action"] = "Register";

            return View();
        }

        public ActionResult About()
        {
            ViewData["Action"] = "About";
            return View();
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewContext.ViewData.Model = model;
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
