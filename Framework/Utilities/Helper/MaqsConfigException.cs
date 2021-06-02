using System;
using System.Runtime.Serialization;

namespace Magenic.Maqs.Utilities.Helper
{
    /// <summary>
    /// Definition of exceptions which will be thrown when there is a problem with loading elements of the MAQS app.config
    /// </summary>
    [Serializable]
    public class MaqsConfigException : Exception
    {
        /// <summary>
        /// MAQS config exception
        /// </summary>
        /// <param name="message">Takes an exception message</param>
        public MaqsConfigException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaqsConfigException" /> class
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public MaqsConfigException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaqsConfigException" /> class
        /// </summary>
        /// <param name="info">The serialization information object that holds the serialized object data about the exception being thrown</param>
        /// <param name="context">The serialization streaming context</param>
        protected MaqsConfigException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
