//--------------------------------------------------
// <copyright file="WebServiceTestObject.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Holds web service context data</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;
using System;
using System.Net.Http;

namespace Magenic.Maqs.BaseWebServiceTest
{
    /// <summary>
    /// Web service test context data
    /// </summary>
    public class WebServiceTestObject : BaseTestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceTestObject" /> class
        /// </summary>
        /// <param name="httpClient">The test's http client driver</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public WebServiceTestObject(Func<HttpClient> httpClient, ILogger logger, string fullyQualifiedTestName) : base(logger, fullyQualifiedTestName)
        {
            this.ManagerStore.Add(typeof(WebServiceDriverManager).FullName, new WebServiceDriverManager(httpClient, this));
        }

        /// <summary>
        /// Gets the web service driver
        /// </summary>
        public WebServiceDriver WebServiceDriver
        {
            get
            {
                return this.WebServiceManager.GetWebServiceDriver();
            }
        }

        /// <summary>
        /// Gets the web service driver manager
        /// </summary>
        public WebServiceDriverManager WebServiceManager
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
            this.WebServiceManager.OverrideDriver(httpClient);
        }

        /// <summary>
        /// Override the http client
        /// </summary>
        /// <param name="httpClient">Function for getting a new http client</param>
        public void OverrideWebServiceDriver(Func<HttpClient> httpClient)
        {
            this.WebServiceManager.OverrideDriver(httpClient);
        }

        /// <summary>
        /// Override the http client driver
        /// </summary>
        /// <param name="webServiceDriver">An http client driver</param>
        public void OverrideWebServiceDriver(WebServiceDriver webServiceDriver)
        {
            this.WebServiceManager.OverrideDriver(webServiceDriver);
        }
    }
}