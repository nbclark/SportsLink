// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

namespace Microsoft.Web.MobileCapableViewEngine
{
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Represents a view engine for rendering a Web Forms page in MVC with support for discover views for mobile devices.
    /// </summary>
    public class MobileCapableWebFormViewEngine : WebFormViewEngine
    {
        private StringDictionary deviceFolders;

        /// <summary>
        /// Initializes a new instance of the MobileCapableWebFormViewEngine class.
        /// </summary>
        public MobileCapableWebFormViewEngine()
        {
            this.deviceFolders = new StringDictionary
            {
                { "MSIE Mobile", "WindowsMobile" },
                { "Mozilla", "iPhone" }
            };
        }

        /// <summary>
        /// Get the "browser/folder" mapping dictionary.
        /// </summary>
        public StringDictionary DeviceFolders
        {
            get
            {
                return this.deviceFolders;
            }
        }

        /// <summary>
        /// Finds the view.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="masterName">Name of the master.</param>
        /// <param name="useCache">if set to true [use cache].</param>
        /// <returns>The page view.</returns>
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            ViewEngineResult result = null;
            HttpRequestBase request = controllerContext.HttpContext.Request;

            if (request.Browser.IsMobileDevice)
            {
                string mobileViewName = string.Empty;

                mobileViewName = GetMobileViewName(string.Concat("Mobile/", this.RetrieveDeviceFolderName(request.Browser.Browser)), viewName);

                result = this.ResolveView(controllerContext, mobileViewName, masterName, useCache);

                if (result == null || result.View == null)
                {
                    mobileViewName = GetMobileViewName("Mobile", viewName);

                    result = this.ResolveView(controllerContext, mobileViewName, masterName, useCache);
                }
            }
            
            if (result == null || result.View == null)
            {
                result = this.ResolveView(controllerContext, viewName, masterName, useCache);
            }

            return result;
        }

        private static string GetMobileViewName(string folderName, string viewName)
        {
            string[] viewParts = viewName.Split('/');

            return string.Format("{0}/{1}/{2}", string.Join("/", viewParts.Take(viewParts.Length-1)), folderName, viewParts.Skip(viewParts.Length-1).First());
        }

        protected virtual ViewEngineResult ResolveView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

        /// <summary>
        /// Get the device folder associated with the name of the browser.
        /// </summary>
        /// <param name="browser">Name of the browser.</param>
        /// <returns>The associated folder name.</returns>
        private string RetrieveDeviceFolderName(string browser)
        {
            if (this.deviceFolders.ContainsKey(browser))
            {
                return this.deviceFolders[browser.Trim()];
            }
            else
            {
                return "unknown";
            }
        }
    }
}