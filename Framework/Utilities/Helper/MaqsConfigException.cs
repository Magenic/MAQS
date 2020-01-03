using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magenic.Maqs.Utilities.Helper
{
    /// <summary>
    /// Definition of exceptions which will be thrown when there is a problem with loading elements of the MAQS app.config
    /// </summary>
    public class MaqsConfigException : Exception
    {
        /// <summary>
        /// Standard generic MAQS config exception
        /// </summary>
        public MaqsConfigException()
        {
        }

        /// <summary>
        /// MAQS config exception
        /// </summary>
        /// <param name="message">Takes an exception message</param>
        public MaqsConfigException(string message) : base(message)
        {
        }

        /// <summary>
        /// MAQS config exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="inner">Inner exception</param>
        public MaqsConfigException(string message, Exception inner) : base(message, inner)
        {
        }

    }
}
