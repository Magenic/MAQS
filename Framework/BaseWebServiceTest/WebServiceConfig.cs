//--------------------------------------------------
// <copyright file="WebServiceConfig.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting web service specific configuration values</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Helper;
using System;
using System.Collections.Generic;

namespace Magenic.Maqs.BaseWebServiceTest
{
    /// <summary>
    /// Config class
    /// </summary>
    public static class WebServiceConfig 
    {
        /// <summary>
        /// Checks the config file for required values upon class load
        /// </summary>
        static WebServiceConfig()
        {
            CheckConfig();
        }

        /// <summary>
        /// Logic to check for required values in app.config
        /// </summary>
        private static void CheckConfig()
        {
            var validator = new ConfigValidation()
            {
                RequiredFields = new List<string>()
                {
                    "WebServiceTimeout",
                    "WebServiceUri"
                },
                Funcs = new List<Func<ValidationPal>>()
                {
                    () => {
                        var requiredKey = "ProxyAddress";
                        var validation = new ValidationPal()
                        {
                            AreFieldsPresent = true
                        };

                        if (Config.DoesKeyExist("UseProxy", ConfigSection.WebServiceMaqs))
                        {
                            if (!Config.DoesKeyExist(requiredKey, ConfigSection.WebServiceMaqs))
                            {
                                validation.AreFieldsPresent = false;
                                validation.MissingField = requiredKey;

                                return validation;
                            }
                            validation.AreFieldsPresent = true;
                            return validation;
                        }
                        return validation;
                    }
                }
            };
            Config.Validate(ConfigSection.WebServiceMaqs, validator);
        }

        /// <summary>
        /// Get the web service URI string
        /// </summary>
        /// <returns>The URI string</returns>
        public static string GetWebServiceUri()
        {
            return Config.GetValueForSection(ConfigSection.WebServiceMaqs, "WebServiceUri");
        }

        /// <summary>
        /// Get the web service timeout
        /// </summary>
        /// <returns>The web service timeout span</returns>
        public static TimeSpan GetWebServiceTimeout()
        {
            return TimeSpan.FromMilliseconds(int.Parse(Config.GetValueForSection(ConfigSection.WebServiceMaqs, "WebServiceTimeout", "-1")));
        }

        /// <summary>
        /// Get if we want to use a proxy for the web driver traffic
        /// </summary>
        /// <returns>True if we want to use the proxy</returns>
        public static bool GetUseProxy()
        {
            return Config.GetValueForSection(ConfigSection.WebServiceMaqs, "UseProxy", "NO").Equals("Yes", StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Get the proxy address to use
        /// </summary>
        /// <returns>The proxy address</returns>
        public static string GetProxyAddress()
        {
            return Config.GetValueForSection(ConfigSection.WebServiceMaqs, "ProxyAddress");
        }
    }
}
