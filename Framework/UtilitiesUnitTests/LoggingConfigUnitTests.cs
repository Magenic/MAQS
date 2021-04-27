using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace UtilitiesUnitTesting
{
    [TestClass]
    public class LoggingConfigUnitTests
    {
        [TestCleanup]
        public void CleanUp()
        {
            Config.AddTestSettingValues("Log", LoggingEnabled.ONFAIL.ToString(), "MagenicMaqs", true);
            Config.AddTestSettingValues("LogLevel", MessageType.INFORMATION.ToString(), "MagenicMaqs", true);
            Config.AddTestSettingValues("LogType", "TXT", "MagenicMaqs", true);
        }

        /// <summary>
        /// Test getting the LoggingEnabledSettings
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void GetLoggingEnabledSettings()
        {
            LoggingEnabled[] loggingEnableds = (LoggingEnabled[])Enum.GetValues(typeof(LoggingEnabled));
    
            for (int i = 0; i < loggingEnableds.Length; i++)
            {
                Config.AddTestSettingValues("Log", loggingEnableds[i].ToString(), "MagenicMaqs", true);
                Assert.AreEqual(loggingEnableds[i] , LoggingConfig.GetLoggingEnabledSetting());
            }
        }

        /// <summary>
        /// Test getting the LoggingEnabledSettings default, it should throw an exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(ArgumentException))]
        public void GetLoggingEnabledSettingsDefault()
        {
            Config.AddTestSettingValues("Log", "Default", "MagenicMaqs", true);
            LoggingConfig.GetLoggingEnabledSetting();
        }

        /// <summary>
        /// Tests getting the LoggingLevelSettings
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void GetLoggingLevelSettings()
        {
            MessageType[] messageTypes = (MessageType[])Enum.GetValues(typeof(MessageType));
          
            for (int i = 0; i < messageTypes.Length; i++) 
            {
                Config.AddTestSettingValues("LogLevel", messageTypes[i].ToString(), "MagenicMaqs", true);
                Assert.AreEqual(messageTypes[i], LoggingConfig.GetLoggingLevelSetting());
            }
        }

        /// <summary>
        /// Tests getting the default LoggingLevelSettings, this should throw an exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(ArgumentException))]
        public void GetLoggingLevelSettingsDefault()
        {
            Config.AddTestSettingValues("LogLevel", "Random", "MagenicMaqs", true);
            LoggingConfig.GetLoggingLevelSetting();
        }

        /// <summary>
        /// Tests getting the Console Logger 
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void GetConsoleLogger()
        {
            LoggingEnabled[] loggingEnableds = (LoggingEnabled[])Enum.GetValues(typeof(LoggingEnabled));
            Config.AddTestSettingValues("LogType", "CONSOLE", "MagenicMaqs", true);

            for (int i = 0; i < loggingEnableds.Length; i++)
            {
                if (loggingEnableds[i] != LoggingEnabled.ONFAIL)
                {
                    Config.AddTestSettingValues("Log", loggingEnableds[i].ToString(), "MagenicMaqs", true);
                    var logger = LoggingConfig.GetLogger(StringProcessor.SafeFormatter(
                    "{0} - {1}",
                    "Test",
                    DateTime.UtcNow.ToString("yyyy-MM-dd-hh-mm-ss-ffff", CultureInfo.InvariantCulture)));
                    Assert.AreEqual(typeof(ConsoleLogger), logger.GetType());
                }       
            }
        }

        /// <summary>
        /// Tests Getting the File Logger
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void GetFileLogger()
        {
            Config.AddTestSettingValues("LogLevel", MessageType.VERBOSE.ToString(), "MagenicMaqs", true);
            Config.AddTestSettingValues("Log", LoggingEnabled.YES.ToString(), "MagenicMaqs", true);
            Config.AddTestSettingValues("LogType", "TXT", "MagenicMaqs", true);
            var logger = LoggingConfig.GetLogger(StringProcessor.SafeFormatter(
                    "{0} - {1}",
                    "Test",
                    DateTime.UtcNow.ToString("yyyy-MM-dd-hh-mm-ss-ffff", CultureInfo.InvariantCulture)));
            Assert.AreEqual(typeof(FileLogger), logger.GetType());
        }

        /// <summary>
        /// Tests getting the HTML Logger
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void GetHTMLLogger()
        {
            Config.AddTestSettingValues("LogLevel", MessageType.VERBOSE.ToString(), "MagenicMaqs", true);
            Config.AddTestSettingValues("Log", LoggingEnabled.YES.ToString(), "MagenicMaqs", true);
            Config.AddTestSettingValues("LogType", "HTML", "MagenicMaqs", true);
            var logger = LoggingConfig.GetLogger(StringProcessor.SafeFormatter(
                    "{0} - {1}",
                    "Test",
                    DateTime.UtcNow.ToString("yyyy-MM-dd-hh-mm-ss-ffff", CultureInfo.InvariantCulture)));

            Assert.AreEqual(typeof(HtmlFileLogger), logger.GetType());

            Config.AddTestSettingValues("LogType", "HTM", "MagenicMaqs", true);
            logger = LoggingConfig.GetLogger(StringProcessor.SafeFormatter(
                    "{0} - {1}",
                    "Test",
                    DateTime.UtcNow.ToString("yyyy-MM-dd-hh-mm-ss-ffff", CultureInfo.InvariantCulture)));

            Assert.AreEqual(typeof(HtmlFileLogger), logger.GetType());
        }

        /// <summary>
        /// Test getting the logger default, it should throw an exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(ArgumentException))]
        public void GetLoggerDefault()
        {
            Config.AddTestSettingValues("Log", LoggingEnabled.YES.ToString(), "MagenicMaqs", true);
            Config.AddTestSettingValues("LogType", "Default", "MagenicMaqs", true);
            LoggingConfig.GetLogger(StringProcessor.SafeFormatter(
                    "{0} - {1}",
                    "Test",
                    DateTime.UtcNow.ToString("yyyy-MM-dd-hh-mm-ss-ffff", CultureInfo.InvariantCulture)));
        }
    }
}
