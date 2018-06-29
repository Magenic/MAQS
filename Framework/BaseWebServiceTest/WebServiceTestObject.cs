//--------------------------------------------------
// <copyright file="WebServiceTestObject.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Holds web service context data</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Logging;
using Magenic.MaqsFramework.WebServiceTester;
using System;
using System.Net.Http;

namespace Magenic.MaqsFramework.BaseWebServiceTest
{
    /// <summary>
    /// Web service test context data
    /// </summary>
    public class WebServiceTestObject : BaseTestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceTestObject" /> class
        /// </summary>
        /// <param name="httpClient">The test's http client wrapper</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public WebServiceTestObject(Func<HttpClient> httpClient, Logger logger, string fullyQualifiedTestName) : base(logger, fullyQualifiedTestName)
        {
            this.ManagerStore.Add(typeof(WebServiceDriverManager).FullName, new WebServiceDriverManager(httpClient, this));
        }

        /// <summary>
        /// Gets the web service wrapper
        /// </summary>
        public WebServiceDriver WebServiceDriver
        {
            get
            {
                WebServiceDriverManager serviceDriver = this.WebServiceDriverManager;
                return serviceDriver.Get();
            }
        }

        /// <summary>
        /// Gets the web service wrapper
        /// </summary>
        public WebServiceDriverManager WebServiceDriverManager
        {
            get
            {
                return this.ManagerStore[typeof(WebServiceDriverManager).FullName] as WebServiceDriverManager;
            }
        }

        /// <summary>
        /// Override the http client
        /// </summary>
        /// <param name="httpClient">The new http client</param>
        public void OverrideWebServiceDriver(HttpClient httpClient)
        {
            this.OverrideDriverManager(typeof(WebServiceDriverManager).FullName, new WebServiceDriverManager(() => httpClient, this));
        }

        /// <summary>
        /// Override the http client
        /// </summary>
        /// <param name="httpClient">Function for getting a new http client</param>
        public void OverrideWebServiceDriver(Func<HttpClient> httpClient)
        {
            this.OverrideDriverManager(typeof(WebServiceDriverManager).FullName, new WebServiceDriverManager(httpClient, this));
        }

        /// <summary>
        /// Override the http client driver
        /// </summary>
        /// <param name="webServiceDriver">An http client wrapper</param>
        public void OverrideWebServiceDriver(WebServiceDriver webServiceDriver)
        {
            (this.ManagerStore[typeof(WebServiceDriverManager).FullName] as WebServiceDriverManager).OverrideWrapper(webServiceDriver);
        }
    }
}