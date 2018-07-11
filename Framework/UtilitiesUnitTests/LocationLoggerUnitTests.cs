//--------------------------------------------------
// <copyright file="LocationLoggerUnitTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>LocationLoggerUnitTests class</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace UtilitiesUnitTesting
{
    /// <summary>
    /// Custom logging location test class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LocationLoggerUnitTests : BaseTest
    {
        /// <summary>
        /// Custom log file path
        /// </summary>
        private static string customPath = Config.GetGeneralValue("CustomLogPath");

        /// <summary>
        /// Cleanup after the custom log path
        /// </summary>
        [ClassCleanup]
        public static void DeleteArtificats()
        {
            Directory.Delete(customPath, true);
        }

        /// <summary>
        /// Setup test with properties
        /// </summary>
        [TestInitialize]
        public void LoggingSetup()
        {
            // Set property overrides
            this.TestContext.Properties.Add("FileLoggerPath", customPath);
            MethodInfo dynMethod = typeof(BaseTest).GetMethod("UpdateConfigParameters", BindingFlags.NonPublic | BindingFlags.Instance);
            dynMethod.Invoke(this, null);

            // Create a new file
            this.TestObject.Log = this.CreateLogger();
        }

        /// <summary>
        /// Test that the file does not exist if we didn't write to it
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void NoFile()
        {
            string path = ((FileLogger)this.Log).FilePath;
            Assert.IsFalse(System.IO.File.Exists(path), path + " exists, but it should not");
        }

        /// <summary>
        /// Test that the file does  exist if we write to it
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void File()
        {
            string path = ((FileLogger)this.Log).FilePath;
            this.Log.LogMessage(MessageType.ERROR, "Error message");
            Assert.IsTrue(System.IO.File.Exists(path), path + " does not exist, but it should");
            Assert.IsTrue(path.StartsWith(customPath), path + " should be under" + @"C:\FrameworksCustom\");
        }
    }
}
