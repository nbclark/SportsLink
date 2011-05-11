﻿// --------------------------------
// <copyright file="IFacebookSettings.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;

    /// <summary>
    /// Represents the settings of a Facebook application.
    /// </summary>
    public interface IFacebookSettings
    {
        /// <summary>
        /// Gets or sets the App secret.
        /// </summary>
        /// <value>The App secret.</value>
        [Obsolete("User AppSecret. Facebook renamed this property.")]
        string AppSecret { get; set; }

        /// <summary>
        /// Gets or sets the app id.
        /// </summary>
        /// <value>The app id.</value>
        string AppId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether cookies are supported.
        /// </summary>
        /// <value><c>true</c> if cookies are supported; otherwise, <c>false</c>.</value>
        bool CookieSupport { get; set; }
        /// <summary>
        /// Gets or sets the base domain.
        /// </summary>
        /// <value>The base domain.</value>
        string BaseDomain { get; set; }
        /// <summary>
        /// Gets or sets the max retries.
        /// </summary>
        /// <value>The max retries.</value>
        int MaxRetries { get; set; }
        /// <summary>
        /// Gets or sets the retry delay.
        /// </summary>
        /// <value>The retry delay.</value>
        int RetryDelay { get; set; }

    }
}
