﻿// --------------------------------
// <copyright file="FacebookOAuthException.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Runtime.Serialization;
using System.Globalization;

    /// <summary>
    /// Represents errors that occur as a result of problems with the OAuth access token.
    /// </summary>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class FacebookOAuthException : FacebookApiException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookOAuthException"/> class.
        /// </summary>
        public FacebookOAuthException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookOAuthException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public FacebookOAuthException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookOAuthException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorType">Type of the error.</param>
        public FacebookOAuthException(string message, string errorType)
            : this(message, errorType, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookOAuthException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public FacebookOAuthException(string message, Exception innerException)
            : this(message, null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookOAuthException"/> class.
        /// </summary>
        /// <param name="message">The error message text.</param>
        /// <param name="errorType">Type of the facebook error.</param>
        /// <param name="innerException">The inner exception.</param>
        public FacebookOAuthException(string message, string errorType, Exception innerException)
            : base(String.Format(CultureInfo.InvariantCulture, "({0}) {1}", errorType ?? "Unknown", message), innerException)
        {
            this.ErrorType = errorType;
        }

#if (!SILVERLIGHT)
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookOAuthException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected FacebookOAuthException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
