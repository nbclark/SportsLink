﻿using System;
using System.Web.Routing;

namespace Facebook.Web.Mvc
{
    /// <summary>
    /// Represents the authorization info needed to access the currently request resource.
    /// </summary>
    public sealed class FacebookAuthorizeInfo
    {
        private RouteValueDictionary _routeValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAuthorizeInfo"/> class.
        /// </summary>
        public FacebookAuthorizeInfo() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAuthorizeInfo"/> class.
        /// </summary>
        /// <param name="authorizeUrl">The authorize URL.</param>
        /// <param name="permissions">The permissions.</param>
        /// <param name="isCancelUrl">if set to <c>true</c> [is cancel URL].</param>
        /// <param name="routeValues">The route values.</param>
        public FacebookAuthorizeInfo(Uri authorizeUrl, string permissions, bool isCancelUrl, RouteValueDictionary routeValues)
        {
            this.AuthorizeUrl = authorizeUrl;
            this.Permissions = permissions;
            this.IsCancelReturn = isCancelUrl;
            this._routeValues = routeValues;
        }

        /// <summary>
        /// Gets or sets the authorize URL.
        /// </summary>
        /// <value>The authorize URL.</value>
        public Uri AuthorizeUrl { get; set; }
        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>The permissions.</value>
        public string Permissions { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is a cancel return.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is a cancel return; otherwise, <c>false</c>.
        /// </value>
        public bool IsCancelReturn { get; set; }
        /// <summary>
        /// Gets the route values.
        /// </summary>
        /// <value>The route values.</value>
        public RouteValueDictionary RouteValues
        {
            get
            {
                if (_routeValues == null)
                {
                    _routeValues = new RouteValueDictionary();
                }
                return _routeValues;
            }
        }

    }
}
