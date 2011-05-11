﻿// --------------------------------
// <copyright file="FacebookSession.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// Represents a Facebook session.
    /// </summary>
    public sealed class FacebookSession
    {
        /// <summary>
        /// The internal dictionary store.
        /// </summary>
        private Dictionary<string, string> dictionary = new Dictionary<string, string>();

        /// <summary>
        /// Gets the internal dictionary store.
        /// </summary>
        /// <value>The dictionary.</value>
        internal IDictionary<string, string> Dictionary
        {
            get
            {
                return this.dictionary;
            }
        }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public long UserId
        {
            get
            {
                if (this.dictionary.ContainsKey("uid"))
                {
                    return long.Parse(this.dictionary["uid"], CultureInfo.InvariantCulture);
                }

                return default(long);
            }

            set
            {
                this.dictionary["uid"] = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets or sets the secret.
        /// </summary>
        /// <value>The secret.</value>
        public string Secret
        {
            get
            {
                if (this.dictionary.ContainsKey("secret"))
                {
                    return this.dictionary["secret"];
                }
 
                return null;
            }

            set
            {
                this.dictionary["secret"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>The access token.</value>
        public string AccessToken
        {
            get
            {
                if (this.dictionary.ContainsKey("access_token"))
                {
                    return this.dictionary["access_token"];
                }

                return null;
            }

            set
            {
                this.dictionary["access_token"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the session key.
        /// </summary>
        /// <value>The session key.</value>
        public string SessionKey
        {
            get
            {
                if (this.dictionary.ContainsKey("session_key"))
                {
                    return this.dictionary["session_key"];
                }

                return null;
            }

            set
            {
                this.dictionary["session_key"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the expires.
        /// </summary>
        /// <value>The expires.</value>
        public DateTime Expires
        {
            get
            {
                if (this.dictionary.ContainsKey("expires") && !String.IsNullOrEmpty(this.dictionary["expires"]))
                {
                    return DateTimeConvertor.FromUnixTime(this.dictionary["expires"]);
                }

                return default(DateTime);
            }

            set
            {
                Contract.Requires(value >= DateTimeConvertor.Epoch);

                this.dictionary["expires"] = DateTimeConvertor.ToUnixTime(value).ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets or sets the signature.
        /// </summary>
        /// <value>The signature.</value>
        public string Signature
        {
            get
            {
                if (this.dictionary.ContainsKey("sig"))
                {
                    return this.dictionary["sig"];
                }

                return null;
            }

            set
            {
                this.dictionary["sig"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the base domain.
        /// </summary>
        /// <value>The base domain.</value>
        public string BaseDomain
        {
            get
            {
                if (this.dictionary.ContainsKey("base_domain"))
                {
                    return this.dictionary["base_domain"];
                }

                return null;
            }

            set
            {
                this.dictionary["base_domain"] = value;
            }
        }

        /// <summary>
        /// The code contracts invarient object method.
        /// </summary>
        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Code contracts invarient method.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Code contracts invarient method.")]
        private void InvarientObject()
        {
            Contract.Invariant(this.dictionary != null);
        }
    }
}
