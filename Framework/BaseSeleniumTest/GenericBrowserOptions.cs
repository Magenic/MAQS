//--------------------------------------------------
// <copyright file="GenericBrowserOptions.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Genreic browser options</summary>
//--------------------------------------------------
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;

namespace Magenic.MaqsFramework.BaseSeleniumTest
{
    /// <summary>
    /// Generic browser options
    /// </summary>
    public class GenericBrowserOptions : DriverOptions
    {
        /// <summary>
        /// The dictionary of capabilities
        /// </summary>
        private readonly Dictionary<string, object> capabilities = new Dictionary<string, object>();

        /// <summary>
        /// Add new capabilities
        /// </summary>
        /// <param name="capabilityName">Capability name</param>
        /// <param name="capabilityValue">Capabilities value, which cannot be null or empty</param>
        public override void AddAdditionalCapability(string capabilityName, object capabilityValue)
        {
            if (string.IsNullOrEmpty(capabilityName))
            {
                throw new ArgumentException("Capability name may not be null an empty string.", "capabilityName");
            }

            this.capabilities[capabilityName] = capabilityValue;
        }

        /// <summary>
        /// Turn the capabilities into an desired capability
        /// </summary>
        /// <returns>A desired capability</returns>
        public override ICapabilities ToCapabilities()
        {
            DesiredCapabilities capabilities = this.GenerateDesiredCapabilities(false);

            foreach (KeyValuePair<string, object> pair in this.capabilities)
            {
                capabilities.SetCapability(pair.Key, pair.Value);
            }

            return capabilities;
        }
    }
}