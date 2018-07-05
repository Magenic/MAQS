//--------------------------------------------------
// <copyright file="Config.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting application configuration values</summary>
//--------------------------------------------------
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

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
        private const string DEFAULTMAQSSECTION = "magenicmaqs";

        /// <summary>
        /// Thread safe collection of configuration overrides
        /// </summary>
        private static ConcurrentDictionary<string, string> configOverrides = new ConcurrentDictionary<string, string>();

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
        /// <param name="section">The name of the section</param>
        /// <returns>Section values, with overrides respected</returns>
        public static Dictionary<string, string> GetSection(string section)
        {
            Dictionary<string, string> sectionValues = new Dictionary<string, string>();

            // Check if the section exists
            if (configValues.TryGetValue(section.ToLower(), out ConcurrentDictionary<string, string> keysAndValues))
            {
                // Loop over all the key value pairs
                foreach (var keyAndValue in keysAndValues)
                {
                    // Always default to our override values
                    if (TryGetSectionValue(keyAndValue.Key, configOverrides, out string overrideValue))
                    {
                        sectionValues.Add(keyAndValue.Key, overrideValue);
                    }
                    else if (TryGetSectionValue(keyAndValue.Key, keysAndValues, out string outValue))
                    {
                        sectionValues.Add(keyAndValue.Key, outValue);
                    }
                }
            }

            return sectionValues;
        }

        /// <summary>
        /// Add configuration override values
        /// </summary>
        /// <param name="configurations">Dictionary of configuration overrides</param>
        /// <param name="overrideExisting">If the override already exists should we override it</param>
        public static void AddTestSettingValues(IDictionary<string, string> configurations, bool overrideExisting = false)
        {
            // Loop over all the configuration overrides
            foreach (KeyValuePair<string, string> configuration in configurations)
            {
                // If it has not been addeded yet, add it
                if (!configOverrides.ContainsKey(configuration.Key))
                {
                    configOverrides.TryAdd(configuration.Key, configuration.Value);
                }
                else if (overrideExisting)
                {
                    // We want to override existing values
                    configOverrides.AddOrUpdate(configuration.Key, configuration.Value, (key, oldValue) => configuration.Value);
                }
            }
        }

        /// <summary>
        /// Get the configuration value for a specific key
        /// </summary>
        /// <param name="key">Config file key</param>
        /// <returns>The configuration value - Returns the empty string if the key is not found</returns>
        /// <example>
        /// <code source = "../UtilitiesUnitTests/ConfigUnitTests.cs" region="GetValueString" lang="C#" />
        /// </example>
        public static string GetValue(string key)
        {
            return GetValue(key, string.Empty);
        }

        /// <summary>
        /// Get the configuration value for a specific key
        /// </summary>
        /// <param name="key">Config file key</param>
        /// <param name="defaultValue">Default value - Returned the key cannot be found</param>
        /// <returns>The configuration value</returns>
        /// <example>
        /// <code source = "../UtilitiesUnitTests/ConfigUnitTests.cs" region="GetValueWithDefault" lang="C#" />
        /// </example>
        public static string GetValue(string key, string defaultValue)
        {
            if (TryGetSectionValue(key, configOverrides, out string overrideValue))
            {
                // Return test run override value
                return overrideValue;
            }
            else if (TryGetDefaultSectionValue(key, out string value))
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
        /// <returns>True if the key exists</returns>
        /// <example>
        /// <code source = "../UtilitiesUnitTests/ConfigUnitTests.cs" region="DoesKeyExist" lang="C#" />
        /// </example>
        [Browsable(false)]
        public static bool DoesKeyExist(string key)
        {
            return TryGetSectionValue(key, configOverrides, out string value) || TryGetDefaultSectionValue(key, out string value2);
        }

        /// <summary>
        /// Try to get a value for a given key in the default config area
        /// </summary>
        /// <param name="key">The value key</param>
        /// <param name="value">The out value</param>
        /// <returns>True if the value was found</returns>
        private static bool TryGetDefaultSectionValue(string key, out string value)
        {
            value = null;

            if (configValues.TryGetValue(DEFAULTMAQSSECTION, out ConcurrentDictionary<string, string> values))
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
                if (dictionary.Keys.Equals(key))
                {
                    // Case sensative match
                    value = dictionary[key];
                }
                else if (dictionary.Any(i => i.Key.Equals(key, System.StringComparison.CurrentCultureIgnoreCase)))
                {
                    // Case insensative match
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
                // set if keys under this node whould be saved and if they should overwrite previous keys
                if (!node.Name.LocalName.ToLower().Contains("maqs"))
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