﻿//--------------------------------------------------
// <copyright file="Config.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting application configuration values</summary>
//--------------------------------------------------
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using VSTestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;

namespace Magenic.Maqs.Utilities.Helper
{
    /// <summary>
    /// Config class
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// The default MAQS session
        /// </summary>
        private const ConfigSection DEFAULTMAQSSECTION = ConfigSection.MagenicMaqs;

        /// <summary>
        /// Thread safe collection of configuration overrides
        /// </summary>
        private static ConcurrentDictionary<string, ConcurrentDictionary<string, string>> configOverrides = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();

        /// <summary>
        /// Thread safe collection of configurations
        /// </summary>
        private static ConcurrentDictionary<string, ConcurrentDictionary<string, string>> configValues = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();

        /// <summary>
        /// Configuration mapping for the custom Magenic Maqs section
        /// </summary>
        private static IConfigurationRoot maqsConfig = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true)
            .AddInMemoryCollection(GetXml())
            .Build();

        /// <summary>
        /// Initializes static members of the <see cref="Config" /> class
        /// </summary>
        static Config()
        {
            // Get all the config sections for all providers
            foreach (IConfigurationSection names in maqsConfig.GetChildren())
            {
                string topKey = names.Key.ToLower();
                ConcurrentDictionary<string, string> values = configValues.GetOrAdd(topKey, new ConcurrentDictionary<string, string>());

                // Get all elements for each section
                foreach (var element in names.GetChildren())
                {
                    // Keys and values will exists at this level App.config files, and 1 level deeper for appsettings
                    if (element.Value != null)
                    {
                        values.TryAdd(element.Key, element.Value);
                    }
                    else
                    {
                        foreach (var item in element.GetChildren())
                        {
                            values.TryAdd(item.Key, item.Value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get a specific config section
        /// </summary>
        /// <param name="section">The configuration section</param>
        /// <returns>Section values, with overrides respected</returns>
        public static Dictionary<string, string> GetSection(ConfigSection section)
        {
            return GetSection(section.ToString());
        }

        /// <summary>
        /// Get a specific config section
        /// </summary>
        /// <param name="section">The name of the section</param>
        /// <returns>Section values, with overrides respected</returns>
        public static Dictionary<string, string> GetSection(string section)
        {
            section = section.ToLower();
            Dictionary<string, string> sectionValues = new Dictionary<string, string>();

            // Get the list of keys for both configuration and configuration overrides
            List<string> keyList = configValues.TryGetValue(section, out ConcurrentDictionary<string, string> configSectionValues) ? new List<string>(configSectionValues.Keys) : new List<string>();

            if (configOverrides.TryGetValue(section, out ConcurrentDictionary<string, string> configsectionOverrideValues))
            {
                keyList = keyList.Union(configsectionOverrideValues.Keys).ToList();
            }

            // Loop over all the key value pairs
            foreach (var key in keyList)
            {
                sectionValues.Add(key, GetValueForSection(section, key));
            }

            return sectionValues;
        }

        /// <summary>
        /// Add VSTest property/parameter overrides
        /// </summary>
        /// <param name="context">The VSTest context</param>
        public static void UpdateWithVSTestContext(VSTestContext context)
        {
            Dictionary<string, string> paramValues = new Dictionary<string, string>();

            // Update configuration settings for Visual Studio unit test
            List<string> propeties = new List<string>();
            IDictionary<string, object> contextProperties = (IDictionary<string, object>)context.GetType().InvokeMember("Properties", BindingFlags.GetProperty, null, context, null);

            // Get a list of framework reserved properties so we can exclude them
            foreach (var property in context.GetType().GetProperties())
            {
                propeties.Add(property.Name);
            }

            foreach (KeyValuePair<string, object> property in contextProperties)
            {
                if (!propeties.Contains(property.Key) && property.Value is string)
                {
                    // Add the override properties
                    paramValues.Add(property.Key, property.Value as string);
                }
            }

            AddTestSettingValues(paramValues);
        }

        /// <summary>
        /// Add NUnit property/parameter overrides
        /// </summary>
        /// <param name="testParameters">NUnit test parameters</param>
        public static void UpdateWithNUnitTestContext(TestParameters testParameters)
        {
            Dictionary<string, string> paramValues = new Dictionary<string, string>();

            foreach (string propertyName in testParameters.Names)
            {
                // Add the override properties
                paramValues.Add(propertyName, testParameters[propertyName]);
            }

            AddTestSettingValues(paramValues);
        }

        /// <summary>
        /// Add configuration override values
        /// </summary>
        /// <param name="configurations">Dictionary of configuration overrides</param>
        /// <param name="overrideExisting">If the override already exists should we override it</param>
        public static void AddGeneralTestSettingValues(IDictionary<string, string> configurations, bool overrideExisting = false)
        {
            AddTestSettingValues(configurations, DEFAULTMAQSSECTION, overrideExisting);
        }

        /// <summary>
        /// Add configuration override values
        /// </summary>
        /// <param name="configurations">Dictionary of configuration overrides</param>
        /// <param name="section">What section it should be added to</param>
        /// <param name="overrideExisting">If the override already exists should we override it</param>
        public static void AddTestSettingValues(IDictionary<string, string> configurations, ConfigSection section = DEFAULTMAQSSECTION, bool overrideExisting = false)
        {
            AddTestSettingValues(configurations, section.ToString(), overrideExisting);
        }

        /// <summary>
        /// Add configuration override values
        /// </summary>
        /// <param name="configurations">Dictionary of configuration overrides</param>
        /// <param name="section">What section it should be added to</param>
        /// <param name="overrideExisting">If the override already exists should we override it</param>
        public static void AddTestSettingValues(IDictionary<string, string> configurations, string section, bool overrideExisting = false)
        {
            section = section.ToLower();

            // Loop over all the configuration overrides
            foreach (KeyValuePair<string, string> configuration in configurations)
            {
                string configKey = configuration.Key;
                string specificSection = section.ToLower();

                // If the key follows this format SECTION:KEY extract the section and key from it
                if (configKey != null && configKey.Contains(":") && configKey.Split(':')[0].Length > 0 && configKey.Split(':')[1].Length > 0)
                {
                    specificSection = configKey.Split(':')[0].ToLower();
                    configKey = configKey.Split(':')[1];
                }

                // Make sure the section exists
                if (!configOverrides.ContainsKey(specificSection))
                {
                    configOverrides.TryAdd(specificSection, new ConcurrentDictionary<string, string>());
                }

                // See if we need to add a new key value pair
                if (!configOverrides[specificSection].ContainsKey(configKey))
                {
                    configOverrides[specificSection].TryAdd(configKey, configuration.Value);
                }
                else if (overrideExisting)
                {
                    // We want to override existing values
                    configOverrides[specificSection].AddOrUpdate(configKey, configuration.Value, (key, oldValue) => configuration.Value);
                }
            }
        }

        /// <summary>
        /// Get value from the general (MAGENICMAQS) section of the config file
        /// </summary>
        /// <param name="key">Config file key</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The configuration value - Returns the empty string if the key is not found</returns>
        /// <example>
        /// <code source = "../UtilitiesUnitTests/ConfigUnitTests.cs" region="GetValueString" lang="C#" />
        /// </example>
        public static string GetGeneralValue(string key, string defaultValue = "")
        {
            return GetValueForSection(DEFAULTMAQSSECTION, key, defaultValue);
        }

        /// <summary>
        /// Get the value from a specific section
        /// </summary>
        /// <param name="section">The configuration section</param>
        /// <param name="key">The key</param>
        /// <returns>The configuration value - Returns the empty string if the key is not found</returns>
        public static string GetValueForSection(ConfigSection section, string key)
        {
            return GetValueForSection(section.ToString(), key, string.Empty);
        }

        /// <summary>
        /// Get the value from a specific section
        /// </summary>
        /// <param name="section">The section name</param>
        /// <param name="key">The key</param>
        /// <returns>The configuration value - Returns the empty string if the key is not found</returns>
        public static string GetValueForSection(string section, string key)
        {
            return GetValueForSection(section, key, string.Empty);
        }

        /// <summary>
        /// Get the configuration value for a specific key
        /// </summary>
        /// <param name="section">The configuration section</param>
        /// <param name="key">Config file key</param>
        /// <param name="defaultValue">Default value - Returned the key cannot be found</param>
        /// <returns>The configuration value</returns>
        public static string GetValueForSection(ConfigSection section, string key, string defaultValue)
        {
            return GetValueForSection(section.ToString(), key, defaultValue);
        }

        /// <summary>
        /// Get the configuration value for a specific key
        /// </summary>
        /// <param name="section">The section name</param>
        /// <param name="key">Config file key</param>
        /// <param name="defaultValue">Default value - Returned the key cannot be found</param>
        /// <returns>The configuration value</returns>
        /// <example>
        /// <code source = "../UtilitiesUnitTests/ConfigUnitTests.cs" region="GetValueWithDefault" lang="C#" />
        /// </example>
        public static string GetValueForSection(string section, string key, string defaultValue)
        {
            section = section.ToLower();

            if (configOverrides.ContainsKey(section) && TryGetSectionValue(key, configOverrides[section], out string overrideValue))
            {
                // Return test run override value
                return overrideValue;
            }
            else if (TryGetDefaultSectionValue(key, section, out string value))
            {
                // Return MagenicMaqs specific app.config value
                return value;
            }

            // Return the default if value is not found in appsettings.json
            return defaultValue;
        }

        /// <summary>
        /// Does the configuration key exist
        /// </summary>
        /// <param name="key">Config file key</param>
        /// <param name="section">The configuration section</param>
        /// <returns>True if the key exists</returns>
        [Browsable(false)]
        public static bool DoesKeyExist(string key, ConfigSection section = ConfigSection.MagenicMaqs)
        {
            return DoesKeyExist(key, section.ToString());
        }

        /// <summary>
        /// Does the configuration key exist
        /// </summary>
        /// <param name="key">Config file key</param>
        /// <param name="section">The section name</param>
        /// <returns>True if the key exists</returns>
        /// <example>
        /// <code source = "../UtilitiesUnitTests/ConfigUnitTests.cs" region="DoesKeyExist" lang="C#" />
        /// </example>
        [Browsable(false)]
        public static bool DoesKeyExist(string key, string section)
        {
            section = section.ToLower();

            return (configOverrides.ContainsKey(section) && TryGetSectionValue(key, configOverrides[section], out string value)) || TryGetDefaultSectionValue(key, section, out string value2);
        }

        /// <summary>
        /// Try to get a value for a given key in the default config area
        /// </summary>
        /// <param name="key">The value key</param>
        /// <param name="section">The section name</param>
        /// <param name="value">The out value</param>
        /// <returns>True if the value was found</returns>
        private static bool TryGetDefaultSectionValue(string key, string section, out string value)
        {
            section = section.ToLower();
            value = null;

            if (configValues.TryGetValue(section, out ConcurrentDictionary<string, string> values))
            {
                TryGetSectionValue(key, values, out value);
            }

            return value != null;
        }

        /// <summary>
        /// Try to get a value for a dictionary
        /// </summary>
        /// <param name="key">The value key</param>
        /// <param name="dictionary">Dictionary of keys and values</param>
        /// <param name="value">The out value</param>
        /// <returns>True if the value was found</returns>
        private static bool TryGetSectionValue(string key, ConcurrentDictionary<string, string> dictionary, out string value)
        {
            value = null;

            if (dictionary != null)
            {
                if (dictionary.ContainsKey(key))
                {
                    // Case sensitive match
                    value = dictionary[key];
                }
                else if (dictionary.Any(i => i.Key.Equals(key, System.StringComparison.CurrentCultureIgnoreCase)))
                {
                    // Case insensitive match
                    value = dictionary.First(i => i.Key.Equals(key, System.StringComparison.CurrentCultureIgnoreCase)).Value;
                }
            }

            return value != null;
        }

        /// <summary>
        /// Reads the xml config file and adds the keys
        /// </summary>
        /// <returns>A dictionary of key value pairs</returns>
        private static IEnumerable<KeyValuePair<string, string>> GetXml()
        {
            // get the file path
            string defaultPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string name = "App.config";
            string path = Path.Combine(defaultPath, name);

            // read the xml file
            XElement rootElement;
            try
            {
                rootElement = XDocument.Parse(File.ReadAllText(path)).Root;
            }
            catch (FileNotFoundException)
            {
                // xml file is not required
                return new List<KeyValuePair<string, string>>();
            }

            // create a list of key pairs
            Dictionary<string, string> keys = new Dictionary<string, string>();

            // find the values MAQS is looking for
            foreach (XElement node in rootElement.Elements())
            {
                // set if keys under this node would be saved and if they should overwrite previous keys
                if (node.Name.LocalName.ToLower().Contains("runtime") || node.Name.LocalName.ToLower().Contains("configsections"))
                {
                    continue;
                }

                foreach (XElement config in node.Elements())
                {
                    IEnumerable<XAttribute> attributes = config.Attributes();

                    if (attributes.Any(x => x.Name.LocalName.Equals("key")) && attributes.Any(y => y.Name.LocalName.Equals("value")))
                    {
                        keys.Add(node.Name.LocalName + ":" + config.Attribute("key").Value, config.Attribute("value").Value);
                    }
                }
            }

            return keys;
        }
    }
}