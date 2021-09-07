//--------------------------------------------------
// <copyright file="ConfigUnitTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test configuration tests</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace UtilitiesUnitTesting
{
    /// <summary>
    /// Configuration unit test class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    [DoNotParallelize]
    public class ConfigUnitTests
    {
        /// <summary>
        /// Setup hierarchical configuration
        /// </summary>
        /// <param name="context">Test context</param>
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            // Add environment settings
            Environment.SetEnvironmentVariable("MagenicMaqs:ConfigJsonEnvRunOverride", "ENV");
            Environment.SetEnvironmentVariable("MagenicMaqs:ConfigJsonEnvRun", "ENV");
            Environment.SetEnvironmentVariable("MagenicMaqs:ConfigJsonEnv", "ENV");
            Environment.SetEnvironmentVariable("MagenicMaqs:EnvOnly", "ENV");

            // Add runtime settings
            Config.UpdateWithVSTestContext(context);

            // Add direct overrides
            Config.AddGeneralTestSettingValues("ConfigJsonEnvRunOverride", "OVERRIDE");
            Config.AddGeneralTestSettingValues("OverrideOnly", "OVERRIDE");
        }

        /// <summary>
        /// Configuration hierarchy is respected
        /// </summary>
        /// <param name="generalKey">Configuration general key</param>
        /// <param name="expected">Expected value for key</param>
        [DataTestMethod]
        [DataRow("ConfigJsonEnvRunOverride", "OVERRIDE")]
        [DataRow("OverrideOnly", "OVERRIDE")]
        [DataRow("ConfigJsonEnvRun", "RUN")]
        [DataRow("RunOnly", "RUN")]
        [DataRow("ConfigJsonEnv", "ENV")]
        [DataRow("EnvOnly", "ENV")]
        [DataRow("ConfigJson", "JSON")]
        [DataRow("JsonOnly", "JSON")]
        [DataRow("ConfigOnly", "XML")]
        public void ConfigHierarchy(string generalKey, string expected)
        {
            Assert.AreEqual(expected, Config.GetGeneralValue(generalKey));
        }

        /// <summary>
        /// Configuration hierarchy is respected
        /// </summary>
        /// <param name="generalKey">Configuration general key</param>
        /// <param name="expected">Expected value for key</param>
        [DataTestMethod]
        [DataRow("TopTest:MidTest:0:LowerTest", "A")]
        [DataRow("TopTest:MidTest:0:Lower:LowestTest", "Lowest")]
        [DataRow("TopTest:MidTest:1:LowerTest", "B")]
        [DataRow("TopTest:AnotherMid:Lowerest", "AnotherLow")]
        public void AdditionalTiers(string generalKey, string expected)
        {
            Assert.AreEqual(expected, Config.GetValueByPath(generalKey).value);
            Assert.AreEqual(expected, Config.GetValueByPath(generalKey.Split(':')).value);
        }

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
            bool value = Config.DoesGeneralKeyExist("DoesNotExist");
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
            Dictionary<string, string> overrides = new Dictionary<string, string>
            {
                { key, overrideValue }
            };

            Config.AddTestSettingValues(overrides);

            // Make sure it worked
            Assert.AreEqual(overrideValue, Config.GetGeneralValue(key));
        }

        /// <summary>
        ///  Verify simple override of a single configuration
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SimpleSingleConfig()
        {
            // Simple override data
            string key = "SimpleOverrideSingle";
            string baseValue = Config.GetGeneralValue(key);
            string overrideValue = baseValue + "_Override";

            // Override the configuration
            Config.AddTestSettingValues(key, overrideValue);

            // Make sure it worked
            Assert.AreEqual(overrideValue, Config.GetGeneralValue(key));
        }

        /// <summary>
        ///  Verify simple override of a single general configuration
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SimpleSingleGeneralConfig()
        {
            // Simple override data
            string key = "SimpleOverrideSingleGeneral";
            string baseValue = Config.GetGeneralValue(key);
            string overrideValue = baseValue + "_Override";

            // Override the configuration
            Config.AddGeneralTestSettingValues(key, overrideValue);

            // Make sure it worked
            Assert.AreEqual(overrideValue, Config.GetGeneralValue(key));
        }

        /// <summary>
        ///  Verify simple override of a single configuration for a specific section
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SimpleSingleForConfigSection()
        {
            // Simple override data
            string key = "SimpleOverrideSingleInSection";
            string baseValue = Config.GetGeneralValue(key);
            string overrideValue = baseValue + "_Override";

            // Override the configuration
            Config.AddTestSettingValues(key, overrideValue, ConfigSection.MagenicMaqs);

            // Make sure it worked
            Assert.AreEqual(overrideValue, Config.GetGeneralValue(key));
        }

        /// <summary>
        ///  Verify simple override of a single configuration for a specific section
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SimpleSingleForConfigSectionString()
        {
            // Simple override data
            string key = "SimpleOverrideSingleInSectionString";
            string baseValue = Config.GetGeneralValue(key);
            string overrideValue = baseValue + "_Override";

            // Override the configuration
            Config.AddTestSettingValues(key, overrideValue, ConfigSection.MagenicMaqs.ToString());

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
            Dictionary<string, string> overrides = new Dictionary<string, string>
            {
                { key, value }
            };

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
            Dictionary<string, string> overrides = new Dictionary<string, string>
            {
                { key, overrideValue }
            };

            Config.AddTestSettingValues(overrides);

            // The secondary override should fail as we already overrode it once
            Assert.AreEqual(overrideValue, Config.GetGeneralValue(key));

            // Try the override again, but this time tell the override to allow itself to be overrode
            overrideValue += "_SecondOverride";
            overrides = new Dictionary<string, string>
            {
                { key, overrideValue }
            };

            Config.AddGeneralTestSettingValues(overrides);

            // Make sure the force override worked
            Assert.AreEqual(overrideValue, Config.GetGeneralValue(key));

            // Make sure the value we didn't override was not affected
            Assert.AreEqual(baseValue2, Config.GetGeneralValue(key2));


            Config.ClearOverrides();
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
        /// Tests that we can use the inner exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(MaqsConfigException))]
        public void MaqsConfigInnerException()
        {
            throw new MaqsConfigException(string.Empty, new MaqsConfigException(string.Empty));
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

                Config.AddGeneralTestSettingValues(overrides);

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

                Config.AddGeneralTestSettingValues(overrides);
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
