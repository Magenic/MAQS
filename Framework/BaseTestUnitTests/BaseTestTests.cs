//--------------------------------------------------
// <copyright file="BaseTestTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Tests for testing base test directly</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace BaseTestUnitTests
{
    /// <summary>
    /// Base test tests
    /// </summary>
    [TestClass]
    [DoNotParallelize]
    public class BaseTestTests : BaseTest
    {
        // Cached config settings
        Dictionary<string, string> general;

        /// <summary>
        /// Cache config settings
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            general = Config.GetSection(ConfigSection.MagenicMaqs);
            Config.AddTestSettingValues("Log", "Default", "MagenicMaqs", true);
        }

        /// <summary>
        /// Restore config settings
        /// </summary>
        [TestCleanup]
        public void CleanUp()
        {
            Config.AddTestSettingValues(general, ConfigSection.MagenicMaqs, true);
        }
        
        /// <summary>
        /// Test duplicate name
        /// </summary>
        [TestMethod]
        public void DuplicateTestName()
        {
            Assert.IsTrue(true);
        }

        /// <summary>
        /// Also test duplicate name
        /// </summary>
        /// <param name="boolean">True or false</param>
        [DataTestMethod]
        [DataRow(true)]
        public void DuplicateTestName(bool boolean)
        {
            Assert.IsTrue(boolean);
        }

        /// <summary>
        /// Make sure we get a console logger if the log configuration is invalid
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void InvalidCreateLoggerMakesConsoleLogger()
        {
            Assert.IsInstanceOfType(CreateLogger("test", "test", MessageType.INFORMATION, LoggingEnabled.YES), typeof(ConsoleLogger));
        }

        /// <summary>
        /// Make sure the try log will fall back to console logging if needed
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void TryLogFallsBackToConsole()
        {
            // Calculate a file path
            string path = Path.GetTempFileName();
            this.Log = new BrokenLogger();
            string logMessage = "TryLogFail";

            try
            {
                // Pipe the console to this file
                using (ConsoleCopy consoleCopy = new ConsoleCopy(path))
                {
                    this.TryToLog(MessageType.INFORMATION, logMessage);
                }
            }
            finally
            {
                this.Log = new ConsoleLogger();
                string logContents = File.ReadAllText(path);
                File.Delete(path);

                SoftAssert.Assert(() => Assert.IsTrue(logContents.Contains(logMessage), $"Log message was '{logContents}'  which does not contain '{logMessage}'"));
                SoftAssert.Assert(() => Assert.IsTrue(logContents.Contains("Logging failed because"), $"Log message was '{logContents}'  which does not contain 'Logging failed because'"));
            }

            SoftAssert.FailTestIfAssertFailed();
        }
    }
}

