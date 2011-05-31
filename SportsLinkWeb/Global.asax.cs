﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Web.Mvc;
using Microsoft.Web.MobileCapableViewEngine;

namespace SportsLinkWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        public static void RegisterViewEngines(ViewEngineCollection engines)
        {
            var engine = new MobileCapableWebFormViewEngine();
            engine.DeviceFolders.Clear();
            engine.DeviceFolders.Add("Mozilla", "iPhone");
            engine.DeviceFolders.Add("Safari", "iPhone");
            engine.DeviceFolders.Add("Chrome", "iPhone");

            engines.Clear();
            engines.Add(engine);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());

            RegisterViewEngines(ViewEngines.Engines);
        }
    }
}