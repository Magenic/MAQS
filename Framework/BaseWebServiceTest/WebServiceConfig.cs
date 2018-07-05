//--------------------------------------------------
// <copyright file="WebServiceConfig.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting web service specific configuration values</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Helper;

namespace Magenic.Maqs.BaseWebServiceTest
{
    /// <summary>
    /// Config class
    /// </summary>
    public static class WebServiceConfig
    {
        /// <summary>
        /// Get the web service URI string
        /// </summary>
        /// <returns>The URI string</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceConfigTests.cs" region="WebServiceConfig" lang="C#" />
        /// </example>
        public static string GetWebServiceUri()
        {
            return Config.GetValue("WebServiceUri");
        }

        /// <summary>
        /// Get the web service timeout in seconds
        /// </summary>
        /// <returns>The timeout in seconds from the config file or default of 100 seconds when no app.config key is found</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceConfigTests.cs" region="GetWebServiceTimeout" lang="C#" />
        /// </example>
        public static int GetWebServiceTimeout()
        {
            return int.Parse(Config.GetValue("WebServiceTimeout", "-1"));
        }
    }
}
