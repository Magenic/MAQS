//--------------------------------------------------
// <copyright file="WebServiceWithWrapperCustomVerb.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>HTTP Request Custom Verb unit tests</summary>
//--------------------------------------------------

using Magenic.MaqsFramework.BaseWebServiceTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using WebServiceTesterUnitTesting.Model;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test web service calls with a custom, user defined verb using the base test wrapper
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServiceWithWrapperCustomVerb : BaseWebServiceTest
    {
        /// <summary>
        /// String to hold the URL
        /// </summary>
        private static string url = Config.GetValue("WebServiceUri");

        /// <summary>
        /// Verify 305 status code is returned
        /// </summary>
        #region CustomVerbStatusCode
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomVerbJSONSerializedVerifyStatusCode()
        {
            var content = WebServiceUtils.MakeStringContent("ZED?", Encoding.UTF8, "application/json");
            var result = this.WebServiceWrapper.CustomWithResponse("ZED", "/api/ZED", "application/json", content, false);
            Assert.AreEqual(HttpStatusCode.UseProxy, result.StatusCode);
        }
        #endregion

        /// <summary>
        /// Verify the stream status code
        /// </summary>
        #region CustomVerbStreamStatusCode
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomVerbJSONStreamSerializedVerifyStatusCode()
        {
            var content = WebServiceUtils.MakeStreamContent(string.Empty, Encoding.UTF8, "application/json");
            var result = this.WebServiceWrapper.CustomWithResponse("ZED", "/api/ZED", "application/json", content, false);
            Assert.AreEqual(HttpStatusCode.UseProxy, result.StatusCode);
        }
        #endregion 

        /// <summary>
        /// Custom Verb send a string without utility to verify status code
        /// </summary>
        #region CustomWithoutContent
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomStringWithoutContentStatusCode()
        {
            var content = WebServiceUtils.MakeStringContent("ZED?", Encoding.UTF8, "application/json");
            var result = this.WebServiceWrapper.CustomWithResponse("ZED", "/api/ZED", "application/json", content.ToString(), Encoding.UTF8, "application/json", true, false);
            Assert.AreEqual(HttpStatusCode.UseProxy, result.StatusCode);
        }
        #endregion

        /// <summary>
        /// Using a custom verb with a generic type
        /// </summary>
        #region CustomVerbGenericType
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomVerbGenericType()
        {
            WebServiceDriver client = new WebServiceDriver(new Uri(url));
        
            var content = WebServiceUtils.MakeStringContent("ZEDTest", Encoding.UTF8, "text/plain");
            var result = client.Custom<string>("ZED", "/api/ZED", "text/plain", content, true);

            Assert.AreEqual(result.ToString(), "ZEDTest");
        }
        #endregion

        /// <summary>
        /// Testing that when making a call with the custom verb the string sent is echoed back
        /// </summary>
        #region CustomVerbFiveArgumentCall
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomVerbFiveArguments()
        {
            WebServiceDriver client = new WebServiceDriver(new Uri(url));

            var content = WebServiceUtils.MakeStringContent("ZEDTest", Encoding.UTF8, "text/plain");
            var result = client.Custom("ZED", "/api/ZED", "text/plain", content, true);

            Assert.AreEqual(result.ToString(), "\"ZEDTest\"");
        }
        #endregion

        /// <summary>
        /// Testing that when making a call with the custom verb the string send is echoed back, with content encoding specified
        /// </summary>
        #region CustomVerbSevenArugmentCall
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomVerbSevenArguments()
        {
            WebServiceDriver client = new WebServiceDriver(new Uri(url));

            var content = WebServiceUtils.MakeStringContent("ZEDTest", Encoding.UTF8, "text/plain");
            var result = client.Custom("ZED", "/api/ZED", "text/plain", "ZEDTest", Encoding.UTF8, "text/plain", true, true);

            Assert.AreEqual(result.ToString(), "\"ZEDTest\"");
        }
        #endregion
    }
}
