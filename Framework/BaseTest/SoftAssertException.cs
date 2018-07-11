//--------------------------------------------------
// <copyright file="SoftAssertException.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Exception class for soft asserts</summary>
//--------------------------------------------------
using System;
using System.Runtime.Serialization;

namespace Magenic.Maqs.BaseTest
{
    /// <summary>
    /// Soft assert exceptions
    /// </summary>
    public class SoftAssertException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoftAssertException" /> class
        /// </summary>
        public SoftAssertException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftAssertException" /> class
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public SoftAssertException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftAssertException" /> class
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public SoftAssertException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftAssertException" /> class
        /// </summary>
        /// <param name="info">The serialization information object that holds the serialized object data about the exception being thrown</param>
        /// <param name="context">The serialization streaming context</param>
        protected SoftAssertException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
