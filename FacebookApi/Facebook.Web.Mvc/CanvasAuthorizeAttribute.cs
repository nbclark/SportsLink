// --------------------------------
// <copyright file="CanvasAuthorizeAttribute.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;
    using Facebook;
    using Facebook.Web;

    public class CanvasAuthorizeAttribute : FacebookAuthorizeAttributeBase
    {
        private static List<long> AuthorizedUsers = new List<long>();

        public string LoginDisplayMode { get; set; }

        public string CancelUrlPath { get; set; }

        public string ReturnUrlPath { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext, IFacebookApplication settings)
        {
            var authorizer = new FacebookWebContext(settings, filterContext.HttpContext);

            if (!string.IsNullOrEmpty(Permissions) && Permissions.IndexOf(" ") != -1)
            {
                throw new ArgumentException("Permissions cannot contain whitespace.");
            }
            
            long? userId = (null != FacebookWebContext.Current.Session) ? (long?)FacebookWebContext.Current.Session.UserId : null;

            if (null == userId || !AuthorizedUsers.Contains(userId.Value))
            {
                if (!authorizer.IsAuthorized(ToArrayString(Permissions)))
                {
                    this.HandleUnauthorizedRequest(filterContext, FacebookApplication.Current);
                }
                else
                {
                    if (!AuthorizedUsers.Contains(FacebookWebContext.Current.Session.UserId))
                    {
                        AuthorizedUsers.Add(FacebookWebContext.Current.Session.UserId);
                    }
                }
            }
        }

        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext, IFacebookApplication settings)
        {
            Contract.Requires(filterContext != null);
            Contract.Requires(settings != null);

            var loginUri = this.GetLoginUrl(settings, filterContext.HttpContext, null);
            filterContext.Result = new CanvasRedirectResult(loginUri.ToString());
        }

        internal virtual protected Uri GetLoginUrl(IFacebookApplication settings, HttpContextBase httpContext, IDictionary<string, object> parameters)
        {
            Contract.Requires(settings != null);
            Contract.Requires(httpContext != null);

            this.ReturnUrlPath = FacebookConfigurationSection.Current.ReturnUrlPath;
            this.CancelUrlPath = FacebookConfigurationSection.Current.CancelUrlPath;

            var authorizer = new CanvasAuthorizer(settings, httpContext)
            {
                ReturnUrlPath = this.ReturnUrlPath,
                CancelUrlPath = this.CancelUrlPath,
                LoginDisplayMode = this.LoginDisplayMode
            };

            if (!String.IsNullOrEmpty(this.Permissions))
            {
                authorizer.Permissions = this.Permissions.Replace(" ", String.Empty).Split(',');
            }

            return authorizer.GetLoginUrl(parameters);
        }
    }
}