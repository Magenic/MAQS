//--------------------------------------------------
// <copyright file="WebServiceConfigTests.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test web service configuration test</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test class for web service configurations
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServiceConfigTests
    {
        /// <summary>
        /// Gets the web service url
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetWebServiceUrl()
        {
            string url = WebServiceConfig.GetWebServiceUri();
            Assert.AreEqual("https://magenicautomation.azurewebsites.net", url);
        }

        /// <summary>
        /// Gets the web service timeout value
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetWebServiceTimeout()
        {
            TimeSpan timeout = WebServiceConfig.GetWebServiceTimeout();
            Assert.AreEqual(10, timeout.Seconds);
        }

        /// <summary>
        /// Get expected UseProxy configuration
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetUseProxy()
        {
            Assert.IsFalse(WebServiceConfig.GetUseProxy());
        }

        /// <summary>
        /// Get expected proxy address configuration
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetProxyAddress()
        {
            Assert.AreEqual("127.0.0.1:8001", WebServiceConfig.GetProxyAddress());
        }
    }
}
