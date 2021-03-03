//--------------------------------------------------
// <copyright file="HttpClientFactory.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
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
            return GetClient(baseAddress, timeout, WebServiceConfig.GetUseProxy(), WebServiceConfig.GetProxyAddress());
        }

        /// <summary>
        /// Gets a HTTP client based on configuration values
        /// </summary>
        /// <param name="baseAddress">Base service uri</param>
        /// <param name="timeout">Web service timeout</param>
        /// <param name="useProxy">Use a proxy</param>
        /// <param name="proxyAddress">The proxy address to use</param>
        /// <returns></returns>
        public static HttpClient GetClient(Uri baseAddress, TimeSpan timeout, bool useProxy, string proxyAddress)
        {
            HttpClientHandler handler = new HttpClientHandler();

            if (useProxy)
            {
                handler.Proxy = new WebProxy(proxyAddress, false);
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