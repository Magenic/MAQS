using System;

namespace Magenic.Maqs.Utilities.Helper
{
    /// <summary>
    /// Definition of exceptions which will be thrown when there is a problem with loading elements of the MAQS app.config
    /// </summary>
    public class MaqsLoggingConfigException : Exception
    {
        /// <summary>
        /// MAQS config exception
        /// </summary>
        /// <param name="message">Takes an exception message</param>
        public MaqsLoggingConfigException(string message) : base(message)
        {
        }
    }
}
