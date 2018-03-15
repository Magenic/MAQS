//--------------------------------------------------
// <copyright file="WebServiceConfigTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test web service configuration test</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseWebServiceTest;
using Magenic.MaqsFramework.Utilities.Helper;
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
            #region WebServiceConfig
            string url = WebServiceConfig.GetWebServiceUri();
            #endregion
            Assert.AreEqual("http://magenicautomation.azurewebsites.net", url);
        }

        /// <summary>
        /// Gets the web service timeout value
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetWebServiceTimeout()
        {
            #region GetWebServiceTimeout
            int timeout = WebServiceConfig.GetWebServiceTimeout();
            #endregion
            Assert.AreEqual(timeout, 10);
        }
    }
}
