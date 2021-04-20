﻿//--------------------------------------------------
// <copyright file="FileLogger.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Writes event logs to plain text file</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Magenic.Maqs.Utilities.Logging
{
    /// <summary>
    ///  Helper class for adding logs to a plain text file. Allows configurable file path.
    /// </summary>
    public class FileLogger : Logger
    {
        /// <summary>
        /// The default log file save location
        /// </summary>
        protected readonly string DEFAULTLOGFOLDER = Path.GetTempPath();

        /// <summary>
        /// Object for locking the log file so 
        /// pending tasks will wait for file to be freed
        /// </summary>
        protected readonly object FileLock = new object();

        /// <summary>
        ///  Initializes a new instance of the FileLogger class
        /// </summary>
        private const string DEFAULTLOGNAME = "FileLog.txt";

        /// <summary>
        ///  Initializes a new instance of the FileLogger class
        /// </summary>
        /// <param name="logFolder">Where log files should be saved</param>
        /// <param name="name">File Name</param>
        /// <param name="messageLevel">Messaging level</param>
        /// <param name="append">True to append to an existing log file or false to overwrite it - If the file does not exist this, flag will have no affect</param>
        public FileLogger(string logFolder = "", string name = DEFAULTLOGNAME, MessageType messageLevel = MessageType.INFORMATION, bool append = false)
            : base(messageLevel)
        {
            string directory;

            if (string.IsNullOrEmpty(logFolder))
            {
                directory = this.DEFAULTLOGFOLDER;
            }
            else
            {
                directory = logFolder;
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!name.EndsWith(this.Extension, StringComparison.CurrentCultureIgnoreCase))
            {
                name += this.Extension;
            }

            this.FilePath = Path.Combine(directory, MakeValidFileName(name));

            if (File.Exists(this.FilePath) && !append)
            {
                StreamWriter writer = new StreamWriter(this.FilePath, false);
                writer.Write(string.Empty);
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Gets or sets the FilePath value
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets the file extension
        /// </summary>
        protected virtual string Extension
        {
            get { return ".txt"; }
        }

        /// <summary>
        /// Write the formatted message (one line) to the console as the specified type
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="args">String format arguments</param>
        public override void LogMessage(string message, params object[] args)
        {
            this.LogMessage(MessageType.INFORMATION, message, args);
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
                lock (this.FileLock)
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(this.FilePath, true))
                        {
                            string date = DateTime.UtcNow.ToString(Logger.DEFAULTDATEFORMAT, CultureInfo.InvariantCulture);
                            writer.WriteLine(StringProcessor.SafeFormatter($"{Environment.NewLine}{date}"));
                            writer.Write(StringProcessor.SafeFormatter($"{messageType.ToString()}:\t"));

                            writer.WriteLine(StringProcessor.SafeFormatter(message, args));
                        }
                    }
                    catch (Exception e)
                    {
                        // Failed to write to the event log, write error to the console instead
                        ConsoleLogger console = new ConsoleLogger();
                        console.LogMessage(MessageType.ERROR, StringProcessor.SafeFormatter($"Failed to write to event log because: {e.Message}"));
                        console.LogMessage(messageType, message, args);
                    }
                }
            }
        }

        /// <summary>
        /// Take a name sting and make it a valid file name
        /// </summary>
        /// <param name="name">The string to cleanup</param>
        /// <returns>The string as a valid file name</returns>
        private static string MakeValidFileName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Blank file name was provide");
            }

            // Create a regex to replace invalid characters
            string charsToReplace = string.Format(@"([{0}]*\.+$)|([{0}]+)", Regex.Escape(new string(Path.GetInvalidFileNameChars())));

            // Replace invalid characters
            return Regex.Replace(name, charsToReplace, "~");
        }
    }
}