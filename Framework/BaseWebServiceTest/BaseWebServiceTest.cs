//--------------------------------------------------
// <copyright file="BaseWebServiceTest.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
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
        /// Gets or sets the web service wrapper
        /// </summary>
        public WebServiceDriver WebServiceWrapper
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
            HttpClient client = new HttpClient
            {
                BaseAddress = this.GetBaseWebServiceUri()
            };
            client.DefaultRequestHeaders.Accept.Clear();

            return client;
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
        /// Create a web service test object
        /// </summary>
        protected override void CreateNewTestObject()
        {
            Logger newLogger = this.CreateLogger();
            this.TestObject = new WebServiceTestObject(() => this.GetHttpClient(), newLogger, this.GetFullyQualifiedTestClassName());
        }
    }
}
