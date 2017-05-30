//--------------------------------------------------
// <copyright file="WebServiceWrapperConfig.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Configuration override tests</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseWebServiceTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Configuration override via properties tests
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServiceWrapperConfig : BaseWebServiceTest
    {
        /// <summary>
        /// Did the logging folder exist at the start of the test run
        /// </summary>
        private static bool loggingFolderExistsBeforeRun = false;

        /// <summary>
        /// Setup before running tests
        /// </summary>
        /// <param name="context">The upcoming test context</param>
        [ClassInitialize]
        public static void CheckBeforeClass(TestContext context)
        {
            loggingFolderExistsBeforeRun = TestHelper.DoesFolderExist();
        }

        /// <summary>
        /// Cleanup after we are done running tests
        /// </summary>
        [ClassCleanup]
        public static void CleanupAfterClass()
        {
            TestHelper.Cleanup(loggingFolderExistsBeforeRun);
        }

        /// <summary>
        /// Setup test with properties
        /// </summary>
        [TestInitialize]
        public void ATestSetup()
        {
            // Set property overrides
            this.TestContext.Properties.Add("SetupTest", "Setup");
            this.TestContext.Properties.Add("SetupTest2", "Setup2");
            this.TestContext.Properties.Add("OverrideTest", "Overridden");

            MethodInfo dynMethod = typeof(BaseWebServiceTest).BaseType.BaseType.GetMethod("UpdateConfigParameters", BindingFlags.NonPublic | BindingFlags.Instance);
            dynMethod.Invoke(this, null);
        }

        /// <summary>
        /// If the property override was for an a value not in existing configuration file does the override work
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void CheckIfOverrideNewWorks()
        {
            // Make sure the new key is not present
            Assert.AreEqual("Setup", Config.GetValue("SetupTest"));
        }

        /// <summary>
        /// If the property override was for an existing configuration does the override work
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void CheckIfOverrideExistingWorks()
        {
            // Make sure the new key is not present
            Assert.AreEqual("Overridden", Config.GetValue("OverrideTest"));
        }

        /// <summary>
        /// Check if you need to explicitly set the override flag to override a value that has already been overridden
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void CheckIfComplexOverrideWorks()
        {
            // Define keys
            string key = "SetupTest2";
            string defaultValue = "Setup2";

            // Get base key values
            Assert.AreEqual(defaultValue, Config.GetValue(key));

            // Try to override something that has already been overriden
            Dictionary<string, string> overrides = new Dictionary<string, string>();
            overrides.Add(key, "ValueThatShouldNotOverride");
            Config.AddTestSettingValues(overrides);

            // The secondary override should fail as we already overrode it once
            Assert.AreEqual(defaultValue, Config.GetValue(key));

            // Try the override again, but this time tell the override to allow itself to be overrode
            defaultValue += "_SecondOverride";
            overrides = new Dictionary<string, string>();
            overrides.Add(key, defaultValue);
            Config.AddTestSettingValues(overrides, true);

            // Make sure the force override worked
            Assert.AreEqual(defaultValue, Config.GetValue(key));
        }
    }
}
