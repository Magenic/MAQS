//--------------------------------------------------
// <copyright file="Logger.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Abstract logging interface</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using System;

namespace Magenic.MaqsFramework.Utilities.Logging
{
    /// <summary>
    /// Abstract logging interface base class
    /// </summary>
    public abstract class Logger
    {
        /// <summary>
        /// Default date format
        /// </summary>
        protected const string DEFAULTDATEFORMAT = "yyyy-MM-dd HH:mm:ss.fff";

        /// <summary>
        /// Log Level value area
        /// </summary>
        private MessageType logLevel;

        /// <summary>
        /// Log Level value save area
        /// </summary>
        private MessageType logLevelSaved = MessageType.SUSPENDED;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger" /> class.
        /// </summary>
        /// <param name="level">The logging level</param>
        public Logger(MessageType level = MessageType.INFORMATION)
        {
            this.logLevel = level;
        }

        /// <summary>
        /// Set the logging level
        /// </summary>
        /// <param name="level">The logging level</param>
        public void SetLoggingLevel(MessageType level)
        {
            this.logLevel = level;
        }

        /// <summary>
        /// Suspends logging
        /// </summary>
        public void SuspendLogging()
        {
            if (this.logLevel != MessageType.SUSPENDED)
            {
                this.logLevelSaved = this.logLevel;
                this.logLevel = MessageType.SUSPENDED;
                this.LogMessage(MessageType.VERBOSE, "Suspending Logging..");
            }
        }

        /// <summary>
        /// Continue logging after it was suspended
        /// </summary>
        public void ContinueLogging()
        {
            // Check if the logging was suspended
            if (this.logLevelSaved != MessageType.SUSPENDED)
            {
                // Return to the log level at the suspension of logging
                this.logLevel = this.logLevelSaved;
            }

            this.logLevelSaved = MessageType.SUSPENDED;
            this.LogMessage(MessageType.VERBOSE, "Logging Continued..");
        }

        /// <summary>
        /// Write the formatted message (one line) to the console as a generic message
        /// </summary>
        /// <param name="messageType">The type of message</param>
        /// <param name="message">The message text</param>
        /// <param name="args">String format arguments</param>
        public abstract void LogMessage(MessageType messageType, string message, params object[] args);

        /// <summary>
        /// Write the formatted message (one line) to the console as the specified type
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="args">String format arguments</param>
        public abstract void LogMessage(string message, params object[] args);

        /// <summary>
        /// Determine if the message should be logged
        /// The message should be logged if it's level is greater than or equal to the current logging level
        /// </summary>
        /// <param name="messageType">The type of message being logged</param>
        /// <returns>True if the message should be logged</returns>
        protected bool ShouldMessageBeLogged(MessageType messageType)
        {
            // The message should be logged if it's level is less than or equal to the current logging level
            return messageType <= this.logLevel;
        }

        /// <summary>
        /// Get the message for an unknown message type
        /// </summary>
        /// <param name="type">The message type</param>
        /// <returns>The unknown message type message</returns>
        protected string UnknownMessageTypeMessage(MessageType type)
        {
            return StringProcessor.SafeFormatter("Unknown MessageType: {0}{1}{2}{3}", Enum.GetName(typeof(MessageType), type), Environment.NewLine, "Message will be displayed with the MessageType of: ", Enum.GetName(typeof(MessageType), MessageType.GENERIC));
        }
    }
}
