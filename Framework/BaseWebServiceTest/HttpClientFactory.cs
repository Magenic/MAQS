//--------------------------------------------------
// <copyright file="HttpClientFactory.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Factory for creating HTTP clients</summary>
//--------------------------------------------------

using System;
using System.Net;
using System.Net.Http;

namespace Magenic.Maqs.BaseWebServiceTest
{
    /// <summary>
    /// Http client factory
    /// </summary>
    public static class HttpClientFactory
    {
        /// <summary>
        /// Gets a HTTP client based on configuration values
        /// </summary>
        /// <returns>A HTTP client</returns>
        public static HttpClient GetDefaultClient()
        {
            return GetClient(new Uri(WebServiceConfig.GetWebServiceUri()), WebServiceConfig.GetWebServiceTimeout());
        }

        /// <summary>
        /// Gets a HTTP client based on configuration values
        /// </summary>
        /// <param name="baseAddress">Base service uri</param>
        /// <param name="timeout">Web service timeout</param>
        /// <returns>A HTTP client</returns>
        public static HttpClient GetClient(Uri baseAddress, TimeSpan timeout)
        {
            HttpClientHandler handler = new HttpClientHandler();

            if (WebServiceConfig.GetUseProxy())
            {
                handler.Proxy = new WebProxy(WebServiceConfig.GetProxyAddress(), false);
                handler.UseProxy = true;
            }

            return new HttpClient(handler)
            {
                BaseAddress = baseAddress,
                Timeout = timeout
            };
        }
    }
}