//--------------------------------------------------
// <copyright file="ILogger.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Logger interface</summary>
//--------------------------------------------------
using System;

namespace Magenic.Maqs.Utilities.Logging
{
    /// <summary>
    /// Base interface for loggers 
    /// </summary>
    public interface ILogger : IDisposable
    {
        /// <summary>
        /// Continue logging after it was suspended
        /// </summary>
        void ContinueLogging();

        /// <summary>
        /// Get the logging level
        /// </summary>
        /// <returns>The logging level</returns>
        MessageType GetLoggingLevel();

        /// <summary>
        /// Write the formatted message (one line) to the console as a generic message
        /// </summary>
        /// <param name="messageType">The type of message</param>
        /// <param name="message">The message text</param>
        /// <param name="args">String format arguments</param>
        void LogMessage(MessageType messageType, string message, params object[] args);

        /// <summary>
        /// Write the formatted message (one line) to the console as the specified type
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="args">String format arguments</param>
        void LogMessage(string message, params object[] args);

        /// <summary>
        /// Set the logging level
        /// </summary>
        /// <param name="level">The logging level</param>
        void SetLoggingLevel(MessageType level);

        /// <summary>
        /// Get current date time for logging purposes
        /// </summary>
        /// <returns>Current data time as a string</returns>
        string CurrentDateTime();

        /// <summary>
        /// Suspends logging
        /// </summary>
        void SuspendLogging();
    }
}