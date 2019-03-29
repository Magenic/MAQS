//--------------------------------------------------
// <copyright file="WebServiceConfig.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
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
    }
}
