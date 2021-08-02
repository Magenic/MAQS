//--------------------------------------------------
// <copyright file="IWebServiceTestObject.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Web service test object interface</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using System;
using System.Net.Http;

namespace Magenic.Maqs.BaseWebServiceTest
{
    /// <summary>
    /// Web service test object interface
    /// </summary>
    public interface IWebServiceTestObject : ITestObject
    {
        /// <summary>
        /// Gets the web service driver
        /// </summary>
        WebServiceDriver WebServiceDriver { get; }

        /// <summary>
        /// Gets the web service driver manager
        /// </summary>
        WebServiceDriverManager WebServiceManager { get; }

        /// <summary>
        /// Override the http client
        /// </summary>
        /// <param name="httpClient">Function for getting a new http client</param>
        void OverrideWebServiceDriver(Func<HttpClient> httpClient);

        /// <summary>
        /// Override the http client
        /// </summary>
        /// <param name="httpClient">The new http client</param>
        void OverrideWebServiceDriver(HttpClient httpClient);

        /// <summary>
        /// Override the http client driver
        /// </summary>
        /// <param name="webServiceDriver">An http client driver</param>
        void OverrideWebServiceDriver(WebServiceDriver webServiceDriver);
    }
}