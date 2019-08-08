//--------------------------------------------------
// <copyright file="WebServiceDriverOverrides.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Web service override unit tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test web service driver override testing
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServiceDriverOverrides : BaseWebServiceTest
    {
        /// <summary>
        /// Base test url
        /// </summary>
        private const string BaseUrl = "https://github.com/Magenic";

        /// <summary>
        /// Expected updated url
        /// </summary>
        private const string UpdatedUrl  = "https://github.com/Magenic/MAQS";

        /// <summary>
        /// Timeout override
        /// </summary>
        private const int MillisecondTimeout = 1;

        /// <summary>
        /// Make sure the overrides end up making it into the http client
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [TestCategory(TestCategories.Utilities)]
        public void OverrideRespectedInDriverInit()
        {
            Assert.AreEqual(UpdatedUrl, this.WebServiceDriver.HttpClient.BaseAddress.OriginalString);
            Assert.AreEqual(MillisecondTimeout, this.WebServiceDriver.HttpClient.Timeout.TotalMilliseconds);
        }

        /// <summary>
        /// Make sure the override of the uri is respected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [TestCategory(TestCategories.Utilities)]
        public void OverrideRespectedForUri()
        {
            Assert.AreEqual(UpdatedUrl, this.GetBaseWebServiceUri().OriginalString);
        }

        /// <summary>
        /// Make sure the override of the url is respected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [TestCategory(TestCategories.Utilities)]
        public void OverrideRespectedForUrl()
        {
            Assert.AreEqual(BaseUrl, this.GetBaseWebServiceUrl());
        }

        /// <summary>
        /// Make sure the override of the url is respected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [TestCategory(TestCategories.Utilities)]
        public void OverrideWorksForFactory()
        {
            HttpClient factoryClient = Magenic.Maqs.BaseWebServiceTest.HttpClientFactory.GetDefaultClient();
            Assert.AreEqual(WebServiceConfig.GetWebServiceUri() + "/", factoryClient.BaseAddress.ToString());
        }

        /// <summary>
        /// Get a new http client
        /// </summary>
        /// <returns>A new http client</returns>
        protected override HttpClient GetHttpClient()
        {
            HttpClient baseClient = base.GetHttpClient();
            baseClient.Timeout = TimeSpan.FromMilliseconds(MillisecondTimeout);
            return baseClient;
        }

        /// <summary>
        /// Get the base web service url
        /// </summary>
        /// <returns>The base web service url</returns>
        protected override Uri GetBaseWebServiceUri()
        {
            return new Uri(this.GetBaseWebServiceUrl() + "/MAQS");
        }

        /// <summary>
        /// Get the base web service url
        /// </summary>
        /// <returns>The base web service url</returns>
        protected override string GetBaseWebServiceUrl()
        {
            return BaseUrl;
        }
    }
}
