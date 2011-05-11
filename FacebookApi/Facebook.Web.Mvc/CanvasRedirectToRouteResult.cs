﻿using System.Web.Mvc;
using System.Web.Routing;

namespace Facebook.Web.Mvc
{
    /// <summary>
    /// Represents a result that performs a canvas redirection by using the specified route values dictionary.
    /// </summary>
    public class CanvasRedirectToRouteResult : RedirectToRouteResult
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasRedirectToRouteResult"/> class.
        /// </summary>
        /// <param name="routeValues">The route values.</param>
        public CanvasRedirectToRouteResult(RouteValueDictionary routeValues) : base(routeValues) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasRedirectToRouteResult"/> class.
        /// </summary>
        /// <param name="routeName">The name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        public CanvasRedirectToRouteResult(string routeName, RouteValueDictionary routeValues) : base(routeName, routeValues) { }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context within which the result is executed.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="context"/> parameter is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public override void ExecuteResult(ControllerContext context)
        {
            string destinationPathAndQuery = UrlHelper.GenerateUrl(RouteName, null /* actionName */, null /* controllerName */, RouteValues, RouteTable.Routes, context.RequestContext, false /* includeImplicitMvcValues */);

            var canvasUrlBuilder = new CanvasUrlBuilder(context.HttpContext.Request);

            var canvasUrl = canvasUrlBuilder.BuildCanvasPageUrl(destinationPathAndQuery);

            var content = CanvasUrlBuilder.GetCanvasRedirectHtml(canvasUrl.ToString());

            context.Controller.TempData.Keep();

            context.HttpContext.Response.ContentType = "text/html";
            context.HttpContext.Response.Write(content);
        }

    }
}
