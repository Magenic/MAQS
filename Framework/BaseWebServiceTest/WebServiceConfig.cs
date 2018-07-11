//--------------------------------------------------
// <copyright file="WebServiceConfig.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting web service specific configuration values</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Helper;
using System;

namespace Magenic.Maqs.BaseWebServiceTest
{
    /// <summary>
    /// Config class
    /// </summary>
    public static class WebServiceConfig
    {
        /// <summary>
        ///  Static name for the web service configuration section
        /// </summary>
        private const string WEBSERVICESECTION = "WebServiceMaqs";

        /// <summary>
        /// Get the web service URI string
        /// </summary>
        /// <returns>The URI string</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceConfigTests.cs" region="WebServiceConfig" lang="C#" />
        /// </example>
        public static string GetWebServiceUri()
        {
            return Config.GetValueForSection(WEBSERVICESECTION, "WebServiceUri");
        }

        /// <summary>
        /// Get the web service timeout
        /// </summary>
        /// <returns>The web service timeout span</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceConfigTests.cs" region="GetWebServiceTimeout" lang="C#" />
        /// </example>
        public static TimeSpan GetWebServiceTimeout()
        {
            return TimeSpan.FromMilliseconds(int.Parse(Config.GetValueForSection(WEBSERVICESECTION, "WebServiceTimeout", "-1")));
        }
    }
}
