//--------------------------------------------------
// <copyright file="BaseWebServiceTest.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>This is the base web service test class</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;
using System;
using System.Net.Http;

namespace Magenic.Maqs.BaseWebServiceTest
{
    /// <summary>
    /// Generic base web service test class
    /// </summary>
    public class BaseWebServiceTest : BaseExtendableTest<WebServiceTestObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseWebServiceTest"/> class.
        /// Setup the web service client for each test class
        /// </summary>
        public BaseWebServiceTest()
        {
        }

        /// <summary>
        /// Gets or sets the web service driver
        /// </summary>
        public WebServiceDriver WebServiceDriver
        {
            get
            {
                return this.TestObject.WebServiceDriver;
            }

            set
            {
                this.TestObject.OverrideWebServiceDriver(value);
            }
        }

        /// <summary>
        /// Get a new http client
        /// </summary>
        /// <returns>A new http client</returns>
        protected virtual HttpClient GetHttpClient()
        {
            return HttpClientFactory.GetClient(this.GetBaseWebServiceUri(), WebServiceConfig.GetWebServiceTimeout(), this.GetUseProxy(), this.GetProxyAddress());
        }

        /// <summary>
        /// Get the base web service url
        /// </summary>
        /// <returns>The base web service url</returns>
        protected virtual Uri GetBaseWebServiceUri()
        {
            return new Uri(this.GetBaseWebServiceUrl());
        }

        /// <summary>
        /// Get the base web service url
        /// </summary>
        /// <returns>The base web service url</returns>
        protected virtual string GetBaseWebServiceUrl()
        {
            return WebServiceConfig.GetWebServiceUri();
        }

        /// <summary>
        /// Get if proxy should be used
        /// </summary>
        /// <returns>True if should use proxy</returns>
        protected virtual bool GetUseProxy()
        {
            return WebServiceConfig.GetUseProxy();
        }

        /// <summary>
        /// Get proxy address
        /// </summary>
        /// <returns>The proxy address and port</returns>
        protected virtual string GetProxyAddress()
        {
            return WebServiceConfig.GetProxyAddress();
        }

        /// <summary>
        /// Create a web service test object
        /// </summary>
        protected override void CreateNewTestObject()
        {
            Logger newLogger = this.CreateLogger();
            this.TestObject = new WebServiceTestObject(() => this.GetHttpClient(), newLogger, this.GetFullyQualifiedTestClassName());
        }
    }
}
