using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;

namespace BaseTestUnitTests
{
    /// <summary>
    /// A logger that throws an exception whenever used
    /// </summary>
    class BrokenLogger : ConsoleLogger
    {
        /// <summary>
        /// Log message call that will throw and exception when used
        /// </summary>
        /// <param name="message">The massage</param>
        /// <param name="args">Any message arguments</param>
        public override void LogMessage(string message, params object[] args)
        {
            this.LogMessage(MessageType.GENERIC, message, args);
        }

        /// <summary>
        /// Log message call that will throw and exception when used
        /// </summary>
        /// <param name="messageType">The type of message</param>
        /// <param name="message">The massage</param>
        /// <param name="args">Any message arguments</param>
        public override void LogMessage(MessageType messageType, string message, params object[] args)
        {
            throw new MaqsLoggingConfigException("No valid configuration gets you to this type");
        }


    }
}
