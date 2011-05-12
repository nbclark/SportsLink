// --------------------------------
// <copyright file="FacebookSignedRequest.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Represents a Facebook signed request.
    /// </summary>
#if !NET35
    [TypeForwardedFrom("Facebook, Version=4.2.1.0, Culture=neutral, PublicKeyToken=58cb4f2111d1e6de")]
#endif
    public sealed class FacebookSignedRequest
    {

        private const string HttpContextKey = "facebook_signed_request";
        private const string SignedRequestKey = "signed_request";

        /// <summary>
        /// The actual value of the signed request.
        /// </summary>
        private IDictionary<string, object> _data;

        /// <summary>
        /// The access token.
        /// </summary>
        private string _accessToken;

        /// <summary>
        /// The user id.
        /// </summary>
        private long _userId;

        /// <summary>
        /// The profile id.
        /// </summary>
        private string _profileId;

        /// <summary>
        /// The expires.
        /// </summary>
        private DateTime _expires;

        /// <summary>
        /// The issued at.
        /// </summary>
        private DateTime _issuedAt;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookSignedRequest"/> class.
        /// </summary>
        /// <param name="data">
        /// The signed request data.
        /// </param>
        public FacebookSignedRequest(IDictionary<string, object> data)
        {
            Contract.Requires(data != null);

            Data = data;
        }

        /// <summary>
        /// Gets actual value of signed request.
        /// </summary>
        public IDictionary<string, object> Data
        {
            get
            {
                Contract.Ensures(Contract.Result<object>() != null);
                return _data;
            }

            private set
            {
                Contract.Requires(value != null);

                var data = (IDictionary<string, object>)(value is JsonObject ? value : FacebookUtils.ToDictionary(value));

                _issuedAt = data.ContainsKey("issued_at") ? DateTimeConvertor.FromUnixTime(Convert.ToDouble(data["issued_at"])) : DateTime.MinValue;

                if (data.ContainsKey("payload"))
                {
                    // new signed_request: http://developers.facebook.com/docs/authentication/canvas/encryption_proposal
                    var payload = (IDictionary<string, object>)data["payload"];

                    if (payload != null)
                    {
                        _accessToken = payload.ContainsKey("access_token") ? (string)payload["access_token"] : null;

                        if (data.ContainsKey("expires_in"))
                        {
                            _expires = data["expires_in"].ToString() == "0" ? DateTime.MaxValue : DateTimeConvertor.FromUnixTime(Convert.ToDouble(data["expires_in"]));
                        }
                        else
                        {
                            _expires = DateTime.MinValue;
                        }

                        string sUserId = payload.ContainsKey("user_id") ? (string)payload["user_id"] : null;
                        long userId;
                        long.TryParse(sUserId, out userId);
                        _userId = userId;

                        _profileId = data.ContainsKey("profile_id") ? (string)data["profile_id"] : null;
                    }
                }
                else
                {
                    // old signed_request: http://developers.facebook.com/docs/authentication/canvas
                    string sUserId = data.ContainsKey("user_id") ? (string)data["user_id"] : null;
                    long userId;
                    long.TryParse(sUserId, out userId);
                    _userId = userId;

                    _accessToken = data.ContainsKey("oauth_token") ? (string)data["oauth_token"] : null;

                    if (data.ContainsKey("expires"))
                    {
                        _expires = data["expires"].ToString() == "0" ? DateTime.MaxValue : DateTimeConvertor.FromUnixTime(Convert.ToDouble(data["expires"]));
                    }
                    else
                    {
                        _expires = DateTime.MinValue;
                    }


                    _profileId = data.ContainsKey("profile_id") ? (string)data["profile_id"] : null;
                }

                _data = data;
            }
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        public string AccessToken
        {
            get { return _accessToken; }
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        public long UserId
        {
            get { return _userId; }
        }

        /// <summary>
        /// Gets the profile id.
        /// </summary>
        public string ProfileId
        {
            get { return _profileId; }
        }

        /// <summary>
        /// Gets the expires.
        /// </summary>
        public DateTime Expires
        {
            get { return _expires; }
        }

        /// <summary>
        /// Gets the issued at.
        /// </summary>
        public DateTime IssuedAt
        {
            get { return _issuedAt; }
        }

        /// <summary>
        /// Try parsing the signed request.
        /// </summary>
        /// <param name="secret">
        /// The secret.
        /// </param>
        /// <param name="signedRequestValue">
        /// The signed request value.
        /// </param>
        /// <param name="signedRequest">
        /// The signed request.
        /// </param>
        /// <returns>
        /// Returns true if parsing was successful otherwise false.
        /// </returns>
        public static bool TryParse(string secret, string signedRequestValue, out FacebookSignedRequest signedRequest)
        {
            Contract.Requires(!String.IsNullOrEmpty(signedRequestValue));
            Contract.Requires(!String.IsNullOrEmpty(secret));
            Contract.Requires(signedRequestValue.Contains("."), Properties.Resources.InvalidSignedRequest);

            var result = TryParse(secret, signedRequestValue, 0, DateTimeConvertor.ToUnixTime(DateTime.UtcNow), false);
            signedRequest = result == null ? null : new FacebookSignedRequest(result);
            return result != null;
        }

        /// <summary>
        /// Try parsing the signed request.
        /// </summary>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        /// <param name="signedRequestValue">
        /// The signed request value.
        /// </param>
        /// <param name="signedRequest">
        /// The signed request.
        /// </param>
        /// <returns>
        /// Returns true if parsing was successful otherwise false.
        /// </returns>
        public static bool TryParse(IFacebookApplication facebookApplication, string signedRequestValue, out FacebookSignedRequest signedRequest)
        {
            Contract.Requires(facebookApplication != null);
            Contract.Requires(!string.IsNullOrEmpty(facebookApplication.AppSecret));
            Contract.Requires(!String.IsNullOrEmpty(signedRequestValue));
            Contract.Requires(signedRequestValue.Contains("."), Properties.Resources.InvalidSignedRequest);

            return TryParse(facebookApplication.AppSecret, signedRequestValue, out signedRequest);
        }

        /// <summary>
        /// Parse the signed request.
        /// </summary>
        /// <param name="secret">
        /// The secret.
        /// </param>
        /// <param name="signedRequestValue">
        /// The signed request value.
        /// </param>
        /// <returns>
        /// Returns the signed request.
        /// </returns>
        public static FacebookSignedRequest Parse(string secret, string signedRequestValue)
        {
            Contract.Requires(!string.IsNullOrEmpty(secret));
            Contract.Requires(!String.IsNullOrEmpty(signedRequestValue));
            Contract.Requires(signedRequestValue.Contains("."), Properties.Resources.InvalidSignedRequest);

            var result = TryParse(secret, signedRequestValue, 0, DateTimeConvertor.ToUnixTime(DateTime.UtcNow), true);
            return result == null ? null : new FacebookSignedRequest(result);
        }

        /// <summary>
        /// Parse the signed request.
        /// </summary>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        /// <param name="signedRequestValue">
        /// The signed request value.
        /// </param>
        /// <returns>
        /// Returns the signed request.
        /// </returns>
        public static FacebookSignedRequest Parse(IFacebookApplication facebookApplication, string signedRequestValue)
        {
            Contract.Requires(facebookApplication != null);
            Contract.Requires(!string.IsNullOrEmpty(facebookApplication.AppSecret));
            Contract.Requires(!String.IsNullOrEmpty(signedRequestValue));
            Contract.Requires(signedRequestValue.Contains("."), Properties.Resources.InvalidSignedRequest);

            return Parse(facebookApplication.AppSecret, signedRequestValue);
        }

        /// <summary>
        /// Parse the signed request.
        /// </summary>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// Returns the signed request.
        /// </returns>
        public static FacebookSignedRequest Parse(IFacebookApplication facebookApplication, HttpRequestBase request)
        {
            return request.Params.AllKeys.Contains(SignedRequestKey)
                       ? Parse(facebookApplication, request.Params[SignedRequestKey])
                       : null;
        }

        /// <summary>
        /// Try parsing the signed request.
        /// </summary>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="signedRequest">
        /// The signed request.
        /// </param>
        /// <returns>
        /// Returns true if parsing is successful otherwise false.
        /// </returns>
        public static bool TryParse(IFacebookApplication facebookApplication, HttpRequestBase request, out FacebookSignedRequest signedRequest)
        {
            signedRequest = null;

            return request.Params.AllKeys.Contains(SignedRequestKey) &&
                   TryParse(facebookApplication, request.Params[SignedRequestKey], out signedRequest);
        }

        /// <summary>
        /// Gets the facebook signed request from the http request.
        /// </summary>
        /// <param name="appSecret">
        /// The app Secret.
        /// </param>
        /// <param name="httpContext">
        /// The http context.
        /// </param>
        /// <returns>
        /// Returns the signed request if found otherwise null.
        /// </returns>
        internal static FacebookSignedRequest GetSignedRequest(string appSecret, HttpContextBase httpContext)
        {
            Contract.Requires(httpContext != null);
            Contract.Requires(httpContext.Request != null);
            Contract.Requires(httpContext.Request.Params != null);

            var items = httpContext.Items;
            var httpRequest = httpContext.Request;
            FacebookSignedRequest signedRequest;
            if (items[HttpContextKey] == null)
            {
                signedRequest = httpRequest.Params.AllKeys.Contains(SignedRequestKey) ? FacebookSignedRequest.Parse(appSecret, httpRequest.Params[SignedRequestKey]) : null;
                items[HttpContextKey] = signedRequest;
            }
            else
            {
                signedRequest = items[HttpContextKey] as FacebookSignedRequest;
            }
            return signedRequest;
        }

        /// <summary>
        /// Parse the signed request string.
        /// </summary>
        /// <param name="secret">
        /// The secret.
        /// </param>
        /// <param name="signedRequestValue">
        /// The signed request value.
        /// </param>
        /// <param name="maxAge">
        /// The max age.
        /// </param>
        /// <param name="currentTime">
        /// The current time.
        /// </param>
        /// <param name="throws">
        /// The throws.
        /// </param>
        /// <returns>
        /// The FacebookSignedRequest.
        /// </returns>
        internal static IDictionary<string, object> TryParse(string secret, string signedRequestValue, int maxAge, double currentTime, bool throws)
        {
            Contract.Requires(!String.IsNullOrEmpty(signedRequestValue));
            Contract.Requires(!String.IsNullOrEmpty(secret));
            Contract.Requires(maxAge >= 0);
            Contract.Requires(currentTime >= 0);
            Contract.Requires(signedRequestValue.Contains("."), Properties.Resources.InvalidSignedRequest);

            try
            {
                // NOTE: currentTime added to parameters to make it unit testable.
                string[] split = signedRequestValue.Split('.');
                if (split.Length != 2)
                {
                    // need to have exactly 2 parts
                    throw new InvalidOperationException(Properties.Resources.InvalidSignedRequest);
                }

                string encodedSignature = split[0];
                string encodedEnvelope = split[1];

                if (string.IsNullOrEmpty(encodedSignature))
                {
                    throw new InvalidOperationException(Properties.Resources.InvalidSignedRequest);
                }

                if (string.IsNullOrEmpty(encodedEnvelope))
                {
                    throw new InvalidOperationException(Properties.Resources.InvalidSignedRequest);
                }

                var envelope = (IDictionary<string, object>)JsonSerializer.Current.DeserializeObject(Encoding.UTF8.GetString(FacebookUtils.Base64UrlDecode(encodedEnvelope)));

                string algorithm = (string)envelope["algorithm"];

                if (!algorithm.Equals("AES-256-CBC HMAC-SHA256") && !algorithm.Equals("HMAC-SHA256"))
                {
                    // TODO: test
                    throw new InvalidOperationException("Invalid signed request. (Unsupported algorithm)");
                }

                byte[] key = Encoding.UTF8.GetBytes(secret);
                byte[] digest = FacebookUtils.ComputeHmacSha256Hash(Encoding.UTF8.GetBytes(encodedEnvelope), key);

                if (!digest.SequenceEqual(FacebookUtils.Base64UrlDecode(encodedSignature)))
                {
                    throw new InvalidOperationException(Facebook.Web.Properties.Resources.InvalidSignedRequestSignature);
                }

                IDictionary<string, object> result;

                if (algorithm.Equals("HMAC-SHA256"))
                {
                    // for requests that are signed, but not encrypted, we're done
                    result = envelope;
                }
                else
                {
                    result = new JsonObject();

                    result["algorithm"] = algorithm;

                    long issuedAt = (long)envelope["issued_at"];

                    if (issuedAt < currentTime)
                    {
                        throw new InvalidOperationException(Web.Properties.Resources.OldSignedRequest);
                    }

                    result["issued_at"] = issuedAt;

                    // otherwise, decrypt the payload
                    byte[] iv = FacebookUtils.Base64UrlDecode((string)envelope["iv"]);
                    byte[] rawCipherText = FacebookUtils.Base64UrlDecode((string)envelope["payload"]);
                    var plainText = FacebookUtils.DecryptAes256CBCNoPadding(rawCipherText, key, iv);

                    var payload = (IDictionary<string, object>)JsonSerializer.Current.DeserializeObject(plainText);
                    result["payload"] = payload;
                }

                return result;
            }
            catch
            {
                if (throws)
                {
                    throw;
                }

                return null;
            }
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        [ContractInvariantMethod]
        private void InvariantObject()
        {
            Contract.Invariant(_data != null);
        }
    }
}