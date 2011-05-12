﻿// --------------------------------
// <copyright file="FacebookAuthorizeAttributeBase.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web.Mvc
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Represents the base class for restricting access to controllers or actions based on Facebook permissions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public abstract class FacebookAuthorizeAttributeBase : ActionFilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Gets or sets the extended permissions.
        /// </summary>
        [System.Obsolete("Perms is marked for removal in future version. Use Permissions instead.")]
        public string Perms
        {
            get { return Permissions; }
            set { Permissions = value; }
        }

        /// <summary>
        /// Gets or sets the extended permissions.
        /// </summary>
        public string Permissions { get; set; }

        /// <summary>
        /// Called when authorization is required.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            OnAuthorization(filterContext, FacebookApplication.Current);
        }

        public abstract void OnAuthorization(AuthorizationContext filterContext, IFacebookApplication facebookApplication);

        internal static string[] ToArrayString(string str)
        {
            return string.IsNullOrEmpty(str) ? null : str.Split(',');
        }

        /*
        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // redirect to facebook login
            var oauth = new FacebookOAuthClientAuthorizer
            {
                ClientId = AppId,
                ClientSecret = AppSecret,
                // set the redirect_uri
            };

            var parameters = new Dictionary<string, object>();
            parameters["display"] = LoginDisplayMode;

            if (!string.IsNullOrEmpty(Perms))
            {
                parameters["scope"] = Perms;
            }

            if (!string.IsNullOrEmpty(State))
            {
                parameters["state"] = State;
            }

            var loginUrl = oauth.GetLoginUri(parameters);
            filterContext.HttpContext.Response.Redirect(loginUrl.ToString());
        } */
    }
}