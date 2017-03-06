//--------------------------------------------------
// <copyright file="FileLogger.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Writes event logs to plain text file</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Magenic.MaqsFramework.Utilities.Logging
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
        ///  Initializes a new instance of the FileLogger class
        /// </summary>
        private const string DEFAULTLOGNAME = "FileLog.txt";

        /// <summary>
        /// Create a private string for the path of the file
        /// </summary>
        private string filePath;

        /// <summary>
        /// Creates a private string for the directory of the folder
        /// </summary>
        private string directory;

        /// <summary>
        ///  Initializes a new instance of the FileLogger class
        /// </summary>
        /// <param name="logFolder">Where log files should be saved</param>
        /// <param name="name">File Name</param>
        /// <param name="messageLevel">Messaging level</param>
        /// <param name="append">True to append to an existing log file or false to overwrite it - If the file does not exist this, flag will have no affect</param>
        public FileLogger(string logFolder = "", string name = DEFAULTLOGNAME, MessageType messageLevel = MessageType.GENERIC, bool append = false)
            : base(messageLevel)
        {
            if (string.IsNullOrEmpty(logFolder))
            {
                this.directory = this.DEFAULTLOGFOLDER;
            }
            else
            {
                this.directory = logFolder;
            }

            if (!Directory.Exists(this.directory))
            {
                Directory.CreateDirectory(this.directory);
            }

            if (!name.EndsWith(this.Extension, StringComparison.CurrentCultureIgnoreCase))
            {
                name += this.Extension;
            }

            this.filePath = Path.Combine(this.directory, MakeValidFileName(name));

            if (File.Exists(this.filePath) && !append)
            {
                StreamWriter writer = new StreamWriter(this.filePath, false);
                writer.Write(string.Empty);
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Gets or sets the FilePath value
        /// </summary>
        public string FilePath
        {
            get { return this.filePath; }
            set { this.filePath = value; }
        }

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
            this.LogMessage(MessageType.GENERIC, message, args);
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
                    StreamWriter writer = new StreamWriter(this.filePath, true);

                    string date = DateTime.UtcNow.ToString(Logger.DEFAULTDATEFORMAT, CultureInfo.InvariantCulture);
                    writer.WriteLine(StringProcessor.SafeFormatter("{0}{1}", Environment.NewLine, date));
                    writer.Write(StringProcessor.SafeFormatter("{0}:\t", messageType.ToString()));

                    writer.WriteLine(StringProcessor.SafeFormatter(message, args));

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
        /// Take a name sting and make it a valid file name
        /// </summary>
        /// <param name="name">The string to cleanup</param>
        /// <returns>The string as a valid file name</returns>
        private static string MakeValidFileName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Blank file name was provide");
            }

            // Create a regex to replace invalid characters
            string charsToReplace = string.Format(@"([{0}]*\.+$)|([{0}]+)", Regex.Escape(new string(Path.GetInvalidFileNameChars())));

            // Replace invalid characters
            return Regex.Replace(name, charsToReplace, "~");
        }
    }
}