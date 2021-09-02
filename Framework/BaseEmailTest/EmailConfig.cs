//--------------------------------------------------
// <copyright file="EmailConfig.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting email specific configuration values</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Magenic.Maqs.BaseEmailTest
{
    /// <summary>
    /// Email configuration class
    /// </summary>
    public static class EmailConfig
    {
        /// <summary>
        /// Loads when class is loaded
        /// </summary>
        static EmailConfig()
        {
            CheckConfig();
        }

        /// <summary>
        /// Ensure required fields are in the config
        /// </summary>
        private static void CheckConfig()
        {
            var validator = new ConfigValidation()
            {
                RequiredFields = new List<string>()
                {
                    "EmailHost",
                    "EmailUserName",
                    "EmailPassword"
                }
            };
            Config.Validate(ConfigSection.EmailMaqs, validator);
        }
        /// <summary>
        /// Get the host string
        /// </summary>
        /// <returns>The email host</returns>
        public static string GetHost()
        {
            return Config.GetValueForSection(ConfigSection.EmailMaqs, "EmailHost");
        }

        /// <summary>
        /// Get the user name string
        /// </summary>
        /// <returns>The user name</returns>
        public static string GetUserName()
        {
            return Config.GetValueForSection(ConfigSection.EmailMaqs, "EmailUserName");
        }

        /// <summary>
        /// Get the password string
        /// </summary>
        /// <returns>The password</returns>
        public static string GetPassword()
        {
            return Config.GetValueForSection(ConfigSection.EmailMaqs, "EmailPassword");
        }

        /// <summary>
        /// Get the port number
        /// </summary>
        /// <returns>The port number</returns>
        public static int GetPort()
        {
            return int.Parse(Config.GetValueForSection(ConfigSection.EmailMaqs, "EmailPort", "143"));
        }

        /// <summary>
        /// Get the use access email via SSL boolean 
        /// </summary>
        /// <returns>True if we should use SSL</returns>
        public static bool GetEmailViaSSL()
        {
            return GetYesOrNo("ConnectViaSSL", "Yes");
        }

        /// <summary>
        /// Get the skip SSL validation boolean 
        /// </summary>
        /// <returns>True if we are skipping SSL validation</returns>
        public static bool GetEmailSkipSslValidation()
        {
            return GetYesOrNo("SkipSslValidation", "No");
        }

        /// <summary>
        /// Gets the download directory path for email attachments
        /// </summary>
        /// <returns>String of file path</returns>
        public static string GetAttachmentDownloadDirectory()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Attachments");
            return Config.GetValueForSection(ConfigSection.EmailMaqs, "AttachmentDownloadPath", path);
        }

        /// <summary>
        /// Get the timeout in milliseconds
        /// </summary>
        /// <returns>The timeout r</returns>
        public static int GetTimeout()
        {
            return int.Parse(Config.GetValueForSection(ConfigSection.EmailMaqs, "EmailTimeout", "10000"));
        }

        /// <summary>
        /// Get a yes or no and change into a boolean
        /// </summary>
        /// <param name="key">The key to get the value for</param>
        /// <param name="defaultValue">The default value for the key</param>
        /// <returns>True if the values is yes</returns>
        private static bool GetYesOrNo(string key, string defaultValue)
        {
            switch (Config.GetValueForSection(ConfigSection.EmailMaqs, key, defaultValue).ToUpper())
            {
                case "YES":
                    return true;
                case "NO":
                    return false;
                default:
                    throw new ArgumentException(StringProcessor.SafeFormatter($" value '{key}' is not a valid option", Config.GetValueForSection(ConfigSection.EmailMaqs, key, defaultValue)));
            }
        }
    }
}
