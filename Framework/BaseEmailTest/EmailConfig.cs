﻿//--------------------------------------------------
// <copyright file="EmailConfig.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting email specific configuration values</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Helper;
using System;
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
        ///  Static name for the email configuration section
        /// </summary>
        private const string DATABASESECTION = "EmailMaqs";

        /// <summary>
        /// Get the host string
        /// </summary>
        /// <returns>The email host</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithoutDriver.cs" region="GetHost" lang="C#" />
        /// </example>
        public static string GetHost()
        {
            return Config.GetValueForSection(DATABASESECTION, "EmailHost");
        }

        /// <summary>
        /// Get the user name string
        /// </summary>
        /// <returns>The user name</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithoutDriver.cs" region="GetUserName" lang="C#" />
        /// </example>
        public static string GetUserName()
        {
            return Config.GetValueForSection(DATABASESECTION, "EmailUserName");
        }

        /// <summary>
        /// Get the password string
        /// </summary>
        /// <returns>The password</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithoutDriver.cs" region="GetPassword" lang="C#" />
        /// </example>
        public static string GetPassword()
        {
            return Config.GetValueForSection(DATABASESECTION, "EmailPassword");
        }

        /// <summary>
        /// Get the port number
        /// </summary>
        /// <returns>The port number</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithoutDriver.cs" region="GetPort" lang="C#" />
        /// </example>
        public static int GetPort()
        {
            return int.Parse(Config.GetValueForSection(DATABASESECTION, "EmailPort", "143"));
        }

        /// <summary>
        /// Get the use access email via SSL boolean 
        /// </summary>
        /// <returns>True if we should use SSL</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithoutDriver.cs" region="GetEmailViaSSL" lang="C#" />
        /// </example>
        public static bool GetEmailViaSSL()
        {
            return GetYesOrNo("ConnectViaSSL", "Yes");
        }

        /// <summary>
        /// Get the skip SSL validation boolean 
        /// </summary>
        /// <returns>True if we are skipping SSL validation</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithoutDriver.cs" region="SkipSSL" lang="C#" />
        /// </example>
        public static bool GetEmailSkipSslValidation()
        {
            return GetYesOrNo("SkipSslValidation", "No");
        }

        /// <summary>
        /// Gets the download directory path for email attachments
        /// </summary>
        /// <returns>String of file path</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithoutDriver.cs" region="DownloadDirectory" lang="C#" />
        /// </example>
        public static string GetAttachmentDownloadDirectory()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Attachments");
            return Config.GetValueForSection(DATABASESECTION, "AttachmentDownloadPath", path);
        }

        /// <summary>
        /// Get the timeout in milliseconds
        /// </summary>
        /// <returns>The timeout r</returns>
        public static int GetTimeout()
        {
            return int.Parse(Config.GetValueForSection(DATABASESECTION, "EmailTimeout", "10000"));
        }

        /// <summary>
        /// Get a yes or no and change into a boolean
        /// </summary>
        /// <param name="key">The key to get the value for</param>
        /// <param name="defaultValue">The default value for the key</param>
        /// <returns>True if the values is yes</returns>
        private static bool GetYesOrNo(string key, string defaultValue)
        {
            switch (Config.GetValueForSection(DATABASESECTION, key, defaultValue).ToUpper())
            {
                case "YES":
                    return true;
                case "NO":
                    return false;
                default:
                    throw new ArgumentException(StringProcessor.SafeFormatter(key + " value '{0}' is not a valid option", Config.GetValueForSection(DATABASESECTION, key, DATABASESECTION)));
            }
        }
    }
}
