//--------------------------------------------------
// <copyright file="WebServiceWithDriverCustomVerb.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>HTTP Request Custom Verb unit tests</summary>
//--------------------------------------------------

using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using WebServiceTesterUnitTesting.Model;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test web service calls with a custom, user defined verb using the base test driver
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServiceWithDriverCustomVerb : BaseWebServiceTest
    {
        /// <summary>
        /// String to hold the URL
        /// </summary>
        private static string url = WebServiceConfig.GetWebServiceUri();

        /// <summary>
        /// Verify 305 status code is returned
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomVerbJSONSerializedVerifyStatusCode()
        {
            var content = WebServiceUtils.MakeStringContent("ZED?", Encoding.UTF8, "application/json");
            var result = this.WebServiceDriver.CustomWithResponse("ZED", "/api/ZED", "application/json", content, false);
            Assert.AreEqual(HttpStatusCode.UseProxy, result.StatusCode);
        }

        /// <summary>
        /// Verify the stream status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomVerbJSONStreamSerializedVerifyStatusCode()
        {
            var content = WebServiceUtils.MakeStreamContent(string.Empty, Encoding.UTF8, "application/json");
            var result = this.WebServiceDriver.CustomWithResponse("ZED", "/api/ZED", "application/json", content, false);
            Assert.AreEqual(HttpStatusCode.UseProxy, result.StatusCode);
        }

        /// <summary>
        /// Custom Verb send a string without utility to verify status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomStringWithoutContentStatusCode()
        {
            var content = WebServiceUtils.MakeStringContent("ZED?", Encoding.UTF8, "application/json");
            var result = this.WebServiceDriver.CustomWithResponse("ZED", "/api/ZED", "application/json", content.ToString(), Encoding.UTF8, "application/json", true, false);
            Assert.AreEqual(HttpStatusCode.UseProxy, result.StatusCode);
        }

        /// <summary>
        /// Using a custom verb with a generic type
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomVerbGenericType()
        {
            WebServiceDriver client = new WebServiceDriver(new Uri(url));
        
            var content = WebServiceUtils.MakeStringContent("ZEDTest", Encoding.UTF8, "text/plain");
            var result = client.Custom<string>("ZED", "/api/ZED", "text/plain", content, true);

            Assert.AreEqual("ZEDTest", result.ToString());
        }

        /// <summary>
        /// Testing that when making a call with the custom verb the string sent is echoed back
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomVerbFiveArguments()
        {
            WebServiceDriver client = new WebServiceDriver(new Uri(url));

            var content = WebServiceUtils.MakeStringContent("ZEDTest", Encoding.UTF8, "text/plain");
            var result = client.Custom("ZED", "/api/ZED", "text/plain", content, true);

            Assert.AreEqual("\"ZEDTest\"", result.ToString());
        }

        /// <summary>
        /// Testing that when making a call with the custom verb the string send is echoed back, with content encoding specified
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomVerbSevenArguments()
        {
            WebServiceDriver client = new WebServiceDriver(new Uri(url));

            var result = client.Custom("ZED", "/api/ZED", "text/plain", "ZEDTest", Encoding.UTF8, "text/plain", true, true);

            Assert.AreEqual("\"ZEDTest\"", result.ToString());
        }
    }
}
