//--------------------------------------------------
// <copyright file="Config.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting application configuration values</summary>
//--------------------------------------------------
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;

namespace Magenic.MaqsFramework.Utilities.Helper
{
    /// <summary>
    /// Config class
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Thread safe collection of configuration overrides
        /// </summary>
        private static ConcurrentDictionary<string, string> configOverrides = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Configuration mapping for the custom Magenic Maqs section
        /// </summary>
        private static NameValueCollection maqsConfig = ConfigurationManager.GetSection("MagenicMaqs") as NameValueCollection;

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
            string overrideValue;

            if (configOverrides.TryGetValue(key, out overrideValue))
            {
                // Return test run override value
                return overrideValue;
            }
            else if (DoesMaqsKeyExist(key))
            {
                // Return MagenicMaqs specific app.config value
                return maqsConfig[key].ToString();
            }

            // Return app settings specific app.config value or the default if value is not found in app.config
            return ConfigurationManager.AppSettings[key] == null || ConfigurationManager.AppSettings[key].Equals(string.Empty) ? defaultValue : ConfigurationManager.AppSettings[key];
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
            return configOverrides.ContainsKey(key) || DoesMaqsKeyExist(key) || ConfigurationManager.AppSettings[key] != null;
        }

        /// <summary>
        /// Does the Maqs configuration key exist
        /// </summary>
        /// <param name="key">Config file key</param>
        /// <returns>True if the key exists</returns>
        private static bool DoesMaqsKeyExist(string key)
        {
            return maqsConfig != null && maqsConfig[key] != null;
        }
    }
}
