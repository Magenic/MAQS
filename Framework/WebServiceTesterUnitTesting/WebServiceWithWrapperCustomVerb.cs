//--------------------------------------------------
// <copyright file="WebServiceWithWrapperCustomVerb.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>HTTP Request Custom Verb unit tests</summary>
//--------------------------------------------------

using Magenic.MaqsFramework.BaseWebServiceTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        /// Verify 305 status code is returned
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomVerbJSONSerializedVerifyStatusCode()
        {
            var content = WebServiceUtils.MakeStringContent(string.Empty, Encoding.UTF8, "application/json");
            var result = this.WebServiceWrapper.CustomWithResponse("ZED", "/api/ZED", "application/json", content, false);
            Assert.AreEqual(HttpStatusCode.UseProxy, result.StatusCode);
        }

        /// <summary>
        /// Verify the stream status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomJSONStreamSerializedVerifyStatusCode()
        {
            var content = WebServiceUtils.MakeStreamContent(string.Empty, Encoding.UTF8, "application/json");
            var result = this.WebServiceWrapper.CustomWithResponse("ZED", "/api/ZED", "application/json", content, false);
            Assert.AreEqual(HttpStatusCode.UseProxy, result.StatusCode);
        }

        /// <summary>
        /// Custom Verb send a string without utility to verify status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomStringWithoutContentStatusCode()
        {
            var result = this.WebServiceWrapper.CustomWithResponse("ZED", "/api/ZED", "text/plain", "Test", Encoding.UTF8, "text/plain", true, false);
            Assert.AreEqual(HttpStatusCode.UseProxy, result.StatusCode);
        }
    }
}
