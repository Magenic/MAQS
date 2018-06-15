// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigTests.cs" company="Magenic">
//   Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>
//   Class for config unit tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CoreUnitTests
{
    /// <summary>
    /// The config unit tests.
    /// </summary>
    [TestClass]
    public class ConfigTests
    {
        /// <summary>
        /// Open page test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.UtilitiesCore)]
        public void Testconfig()
        {
            Config.DoesKeyExist("Log");
            Assert.AreEqual(true, Config.DoesKeyExist("Log"));
            Assert.AreEqual(true, Config.DoesKeyExist("Browser"));
            Assert.AreEqual("OnFail", Config.GetValue("Log", "NO")); 
            Assert.AreEqual("PhantomJS", Config.GetValue("Browser", "NO"));
        }

        /// <summary>
        /// Gets a value from a string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.UtilitiesCore)]
        public void GetValueWithString()
        {
            #region GetValueString
            string value = Config.GetValue("WaitTime");
            #endregion
            Assert.AreEqual("100", value);
        }

        /// <summary>
        /// Gets a value with a string or default
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.UtilitiesCore)]
        public void GetValueWithStringAndDefault()
        {
            #region GetValueWithDefault
            string value = Config.GetValue("DoesNotExist", "Default");
            #endregion
            Assert.AreEqual("Default", value);
        }

        /// <summary>
        /// Checks if a key exists
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.UtilitiesCore)]
        public void DoesKeyExist()
        {
            #region DoesKeyExist
            bool value = Config.DoesKeyExist("DoesNotExist");
            #endregion
            Assert.AreEqual(false, value);
        }

        /// <summary>
        ///  Verify simple override of an existing configuration
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.UtilitiesCore)]
        public void SimpleOverrideConfig()
        {
            // Simple override data
            string key = "SimpleOverride";
            string baseValue = Config.GetValue(key);
            string overrideValue = baseValue + "_Override";

            // Override the configuration
            var overrides = new Dictionary<string, string>();
            overrides.Add(key, overrideValue);
            Config.AddTestSettingValues(overrides);

            // Make sure it worked
            Assert.AreEqual(overrideValue, Config.GetValue(key));
        }

        /// <summary>
        ///  Verify simple override of a new configuration
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.UtilitiesCore)]
        public void OverrideNewConfig()
        {
            string key = "AddNewKey";
            string value = "TestValue";

            // Make sure the new key is not present
            Assert.AreEqual(string.Empty, Config.GetValue(key));

            // Set the override
            var overrides = new Dictionary<string, string>();
            overrides.Add(key, value);
            Config.AddTestSettingValues(overrides);

            // Make sure the override worked
            Assert.AreEqual(value, Config.GetValue(key));
        }

        /// <summary>
        ///  Verify complex configuration overrides
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.UtilitiesCore)]
        public void ComplexOverrideConfig()
        {
            // Define keys
            string key = "Override";
            string key2 = "Override2";

            // Get base key values
            string baseValue = Config.GetValue(key);
            string baseValue2 = Config.GetValue(key2);

            // Set override value
            string overrideValue = baseValue + "_Override";

            // Override first key value
            var overrides = new Dictionary<string, string>();
            overrides.Add(key, overrideValue);
            Config.AddTestSettingValues(overrides);

            // Try to override something that has already been overriden
            overrides = new Dictionary<string, string>();
            overrides.Add(key, "ValueThatShouldNotOverride");
            Config.AddTestSettingValues(overrides);

            // The secondary override should fail as we already overrode it once
            Assert.AreEqual(overrideValue, Config.GetValue(key));

            // Try the override again, but this time tell the override to allow itself to be overrode
            overrideValue += "_SecondOverride";
            overrides = new Dictionary<string, string>();
            overrides.Add(key, overrideValue);
            Config.AddTestSettingValues(overrides, true);

            // Make sure the force override worked
            Assert.AreEqual(overrideValue, Config.GetValue(key));

            // Make sure the value we didn't override was not affected
            Assert.AreEqual(baseValue2, Config.GetValue(key2));
        }
    }
}