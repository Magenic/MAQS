﻿//--------------------------------------------------
// <copyright file="ConfigUnitTests.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test configuration tests</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace UtilitiesUnitTesting
{
    /// <summary>
    /// Configuration unit test class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConfigUnitTests
    {
        /// <summary>
        /// Gets a value from a string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void GetValueWithString()
        {
            string value = Config.GetGeneralValue("WaitTime");
            Assert.AreEqual("100", value);
        }

        /// <summary>
        /// Gets a value with a string or default
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void GetValueWithStringAndDefault()
        {
            string value = Config.GetGeneralValue("DoesNotExist", "Default");
            Assert.AreEqual("Default", value);
        }

        /// <summary>
        /// Checks if a key exists
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void DoesKeyExist()
        {
            bool value = Config.DoesKeyExist("DoesNotExist");
            Assert.AreEqual(false, value);
        }

        /// <summary>
        ///  Verify simple override of an existing configuration
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SimpleOverrideConfig()
        {
            // Simple override data
            string key = "SimpleOverride";
            string baseValue = Config.GetGeneralValue(key);
            string overrideValue = baseValue + "_Override";

            // Override the configuration
            Dictionary<string, string> overrides = new Dictionary<string, string>();
            overrides.Add(key, overrideValue);
            Config.AddTestSettingValues(overrides);

            // Make sure it worked
            Assert.AreEqual(overrideValue, Config.GetGeneralValue(key));
        }

        /// <summary>
        ///  Verify simple override of a new configuration
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void OverrideNewConfig()
        {
            string key = "AddNewKey";
            string value = "TestValue";

            // Make sure the new key is not present
            Assert.AreEqual(string.Empty, Config.GetGeneralValue(key));

            // Set the override
            Dictionary<string, string> overrides = new Dictionary<string, string>();
            overrides.Add(key, value);
            Config.AddTestSettingValues(overrides);

            // Make sure the override worked
            Assert.AreEqual(value, Config.GetGeneralValue(key));
        }

        /// <summary>
        ///  Verify complex configuration overrides
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void ComplexOverrideConfig()
        {
            // Define keys
            string key = "Override";
            string key2 = "Override2";

            // Get base key values
            string baseValue = Config.GetGeneralValue(key);
            string baseValue2 = Config.GetGeneralValue(key2);

            // Set override value
            string overrideValue = baseValue + "_Override";

            // Override first key value
            Dictionary<string, string> overrides = new Dictionary<string, string>();
            overrides.Add(key, overrideValue);
            Config.AddTestSettingValues(overrides);

            // Try to override something that has already been overridden
            overrides = new Dictionary<string, string>();
            overrides.Add(key, "ValueThatShouldNotOverride");
            Config.AddTestSettingValues(overrides);

            // The secondary override should fail as we already overrode it once
            Assert.AreEqual(overrideValue, Config.GetGeneralValue(key));

            // Try the override again, but this time tell the override to allow itself to be overrode
            overrideValue += "_SecondOverride";
            overrides = new Dictionary<string, string>();
            overrides.Add(key, overrideValue);
            Config.AddGeneralTestSettingValues(overrides, true);

            // Make sure the force override worked
            Assert.AreEqual(overrideValue, Config.GetGeneralValue(key));

            // Make sure the value we didn't override was not affected
            Assert.AreEqual(baseValue2, Config.GetGeneralValue(key2));
        }

        /// <summary>
        /// Tests that the config is validated when there are no required fields
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void ConfigNoRequiredFields()
        {
            ConfigValidation configValidation = new ConfigValidation()
            {
                RequiredFields = new List<string>()
            };

            Config.Validate(ConfigSection.MagenicMaqs, configValidation);
        }

        /// <summary>
        /// Tests that an exception is thrown when a field is not present
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(MaqsConfigException))]
        public void ConfigFieldsMissing()
        {
            ConfigValidation configValidation = new ConfigValidation()
            {
                RequiredFields = new List<string>
                {
                    "Invalid_Config_Field_For_Validation"
                }
            };

            Config.Validate(ConfigSection.WebServiceMaqs, configValidation);
        }

        /// <summary>
        /// Tests that we can skip the validation 
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [DoNotParallelize]
        public void ConfigFieldsMissingButValidationSkipped()
        {
            try
            {
                Dictionary<string, string> overrides = new Dictionary<string, string>
                {
                    { "SkipConfigValidation", "Yes" }
                };

                Config.AddGeneralTestSettingValues(overrides, true);

                ConfigValidation configValidation = new ConfigValidation()
                {
                    RequiredFields = new List<string>
                    {
                        "Invalid_Config_Field_For_Validation"
                    }
                };

                Config.Validate(ConfigSection.WebServiceMaqs, configValidation);
            }
            finally
            {
                Dictionary<string, string> overrides = new Dictionary<string, string>
                {
                    { "SkipConfigValidation", "No" }
                };

                Config.AddGeneralTestSettingValues(overrides, true);
            }
        }

        /// <summary>
        /// Tests that an exception is thrown when the fields to validate is null
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(MaqsConfigException))]
        public void ConfigFieldsNull()
        {
            Config.Validate(ConfigSection.WebServiceMaqs, null);
        }

    }
}
