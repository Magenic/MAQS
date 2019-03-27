//--------------------------------------------------
// <copyright file="HttpClientFactory.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Factory for creating HTTP clients</summary>
//--------------------------------------------------

using System;
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
            return new HttpClient
            {
                BaseAddress = baseAddress,
                Timeout = timeout
            };
        }
    }
}