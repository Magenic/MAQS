//--------------------------------------------------
// <copyright file="HtmlFileLogger.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Writes event logs to HTML file</summary>
//--------------------------------------------------
using System;
using System.Globalization;
using System.IO;
using Magenic.MaqsFramework.Utilities.Data;
using System.Web;

namespace Magenic.MaqsFramework.Utilities.Logging
{
    /// <summary>
    /// Helper class for adding logs to an HTML file. Allows configurable file path.
    /// </summary>
    public class HtmlFileLogger : FileLogger, IDisposable
    {
        /// <summary>
        /// The default log name
        /// </summary>
        private const string DEFAULTLOGNAME = "FileLog.html";

        /// <summary>
        /// Default header for the HTML file, this gives us our colored text
        /// </summary>
        private const string DEFAULTHTMLHEADER =
            "<!DOCTYPE html><html><header><title>Test Log</title></header><body>";

        /// <summary>
        /// Initializes a new instance of the HtmlFileLogger class
        /// </summary>
        /// <param name="logFolder">Where log files should be saved</param>
        /// <param name="name">File Name</param>
        /// <param name="messageLevel">Messaging level</param>
        /// <param name="append">True to append to an existing log file or false to overwrite it - If the file does not exist this, flag will have no affect</param>
        public HtmlFileLogger(string logFolder = "", string name = DEFAULTLOGNAME, MessageType messageLevel = MessageType.GENERIC, bool append = false)
            : base(logFolder, name, messageLevel, append)
        {
            StreamWriter writer = new StreamWriter(this.FilePath, true);
            writer.Write(DEFAULTHTMLHEADER);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Gets the file extension
        /// </summary>
        protected override string Extension
        {
            get { return ".html"; }
        }

        /// <summary>
        /// Dispose the class
        /// </summary>
        public void Dispose()
        {
            if (File.Exists(this.FilePath))
            {
                var writer = new StreamWriter(this.FilePath, true);

                writer.WriteLine("</body></html>");
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Write the formatted message (one line) to the console as a generic message
        /// </summary>
        /// <param name="messageType">The type of message</param>
        /// <param name="message">The message text</param>
        /// <param name="args">String format arguments</param>
        public override void LogMessage(MessageType messageType, string message, params object[] args)
        {
            // If the message level is greater that the current log level then do not log it.
            if (this.ShouldMessageBeLogged(messageType))
            {
                // Log the message
                try
                {
                    string date = DateTime.UtcNow.ToString(Logger.DEFAULTDATEFORMAT, CultureInfo.InvariantCulture);

                    StreamWriter writer = new StreamWriter(this.FilePath, true);

                    // Set the style
                    writer.Write(this.GetTextWithColorFlag(messageType));

                    // Add the content
                    writer.WriteLine(HttpUtility.HtmlEncode(StringProcessor.SafeFormatter("{0}{1}", Environment.NewLine, date)));
                    writer.Write(HttpUtility.HtmlEncode(StringProcessor.SafeFormatter("{0}:\t", messageType.ToString())));
                    writer.WriteLine(HttpUtility.HtmlEncode(StringProcessor.SafeFormatter(message, args)));

                    // Close off the style
                    writer.Write("</p>");

                    writer.Flush();
                    writer.Close();
                }
                catch (Exception e)
                {
                    // Failed to write to the event log, write error to the console instead
                    ConsoleLogger console = new ConsoleLogger();
                    console.LogMessage(MessageType.ERROR, StringProcessor.SafeFormatter("Failed to write to event log because: {0}", e.Message));
                    console.LogMessage(messageType, message, args);
                }
            }
        }

        /// <summary>
        /// Get the HTML style key for the given message type
        /// </summary>
        /// <param name="type">The message type</param>
        /// <returns>string - The HTML style key for the given message type</returns>
        private string GetTextWithColorFlag(MessageType type)
        {
            switch (type)
            {
                case MessageType.VERBOSE:
                    return "<p style =\"color:purple\">";
                case MessageType.ERROR:
                    return "<p style=\"color:red\">";
                case MessageType.GENERIC:
                    return "<p style =\"color:black\">";
                case MessageType.INFORMATION:
                    return "<p style =\"color:blue\">";
                case MessageType.SUCCESS:
                    return "<p style=\"color:green\">";
                case MessageType.WARNING:
                    return "<p style=\"color:orange\">";
                default:
                    Console.WriteLine(this.UnknownMessageTypeMessage(type));
                    return "<p style=\"color:hotpink\">";
            }
        }
    }
}