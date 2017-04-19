//--------------------------------------------------
// <copyright file="LoggingConfig.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Logging related configuration</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Helper;
using System;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;

namespace Magenic.MaqsFramework.Utilities.Logging
{
    /// <summary>
    /// Logging related configuration
    /// </summary>
    public static class LoggingConfig
    {
        /// <summary>
        /// Get our logging state - Yes, no or on failure
        /// </summary>
        /// <returns>The log enabled state</returns>
        public static LoggingEnabled GetLoggingEnabledSetting()
        {
            switch (Config.GetValue("Log", "NO").ToUpper())
            {
                case "YES":
                    return LoggingEnabled.YES;
                case "ONFAIL":
                    return LoggingEnabled.ONFAIL;
                case "NO":
                    return LoggingEnabled.NO;
                default:
                    throw new ArgumentException(StringProcessor.SafeFormatter("Log value '{0}' is not a valid option", Config.GetValue("Log", "NO")));
            }
        }

        /// <summary>
        /// Get our logging level
        /// </summary>
        /// <returns>MessageType - The current log level</returns>
        public static MessageType GetLoggingLevelSetting()
        {
            switch (Config.GetValue("LogLevel", "INFORMATION").ToUpper())
            {
                case "VERBOSE":
                    return MessageType.VERBOSE;         // Includes this and all of those below
                case "INFORMATION":
                    return MessageType.INFORMATION;     // Includes this and all of those below
                case "GENERIC":
                    return MessageType.GENERIC;         // Includes this and all of those below
                case "SUCCESS":
                    return MessageType.SUCCESS;         // Includes this and all of those below
                case "WARNING":
                    return MessageType.WARNING;         // Includes this and all of those below
                case "ERROR":
                    return MessageType.ERROR;           // Includes errors only
                case "SUSPENDED":
                    return MessageType.SUSPENDED;       // All logging is suspended
                default:
                    throw new ArgumentException(StringProcessor.SafeFormatter("Logging level value '{0}' is not a valid option", Config.GetValue("LogLevel", "ERROR")));
            }
        }

        /// <summary>
        /// Get the logger
        /// </summary>
        /// <param name="fileName">File name to use for the log</param>
        /// <returns>The logger</returns>
        public static Logger GetLogger(string fileName)
        {
            // Disable logging means we just send any logged messages to the console
            if (GetLoggingEnabledSetting() == LoggingEnabled.NO)
            {
                return new ConsoleLogger();
            }

            string logDirectory = GetLogDirectory();

            switch (Config.GetValue("LogType", "CONSOLE").ToUpper())
            {
                case "CONSOLE":
                    return new ConsoleLogger(GetLoggingLevelSetting());
                case "TXT":
                    return new FileLogger(logDirectory, fileName, GetLoggingLevelSetting());
                case "HTML":
                case "HTM":
                    return new HtmlFileLogger(logDirectory, fileName, GetLoggingLevelSetting());
                default:
                    throw new ArgumentException(StringProcessor.SafeFormatter("Log type '{0}' is not a valid option", Config.GetValue("LogType", "CONSOLE")));
            }
        }

        /// <summary>
        /// Gets the File Directory to store log files
        /// </summary>
        /// <returns>String of file path</returns>
        public static string GetLogDirectory()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Logs");
            return Config.GetValue("FileLoggerPath", path);
        }

        /// <summary>
        /// Gets the Screenshot Format to save images
        /// </summary>
        /// <returns>Desired ImageFormat Type</returns>
        /// <param name="imageFormat">Image Format Screen format screen</param>
        public static ScreenshotImageFormat GetScreenShotFormat(string imageFormat = "ImageFormat")
        {
            switch (Config.GetValue(imageFormat, "PNG").ToUpper())
            {
                case "BMP":
                    return ScreenshotImageFormat.Bmp;
                case "GIF":
                    return ScreenshotImageFormat.Gif;
                case "JPEG":
                    return ScreenshotImageFormat.Jpeg;
                case "PNG":
                    return ScreenshotImageFormat.Png;
                case "TIFF":
                    return ScreenshotImageFormat.Tiff;
                default:
                    throw new ArgumentException(StringProcessor.SafeFormatter("ImageFormat '{0}' is not a valid option", Config.GetValue("ScreenShotFormat", "PNG")));
            }
        }
    }
}
