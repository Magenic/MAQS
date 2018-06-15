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
            this.DriversStore.Add(typeof(WebServiceDriver).FullName, new WebServiceDriverStore(httpClient, this));
        }

        /// <summary>
        /// Gets the web service wrapper
        /// </summary>
        public WebServiceDriver HttpClientWrapper
        {
            get
            {
                WebServiceDriverStore serviceDriver = this.DriversStore[typeof(WebServiceDriver).FullName] as WebServiceDriverStore;
                return serviceDriver.Get();
            }
        }

        /// <summary>
        /// Override the http client
        /// </summary>
        /// <param name="httpClient">The new http client</param>
        public void OverrideWebServiceDriver(HttpClient httpClient)
        {
            this.OverrideDriver(typeof(WebServiceDriver).FullName, new WebServiceDriverStore(() => httpClient, this));
        }

        /// <summary>
        /// Override the http client
        /// </summary>
        /// <param name="httpClient">Function for getting a new http client</param>
        public void OverrideWebServiceDriver(Func<HttpClient> httpClient)
        {
            this.OverrideDriver(typeof(WebServiceDriver).FullName, new WebServiceDriverStore(httpClient, this));
        }

        /// <summary>
        /// Override the http client driver
        /// </summary>
        /// <param name="httpClientWrapper">An http client wrapper</param>
        public void OverrideWebServiceDriver(WebServiceDriver httpClientWrapper)
        {
            (this.DriversStore[typeof(WebServiceDriver).FullName] as WebServiceDriverStore).OverrideWrapper(httpClientWrapper);
        }
    }
}