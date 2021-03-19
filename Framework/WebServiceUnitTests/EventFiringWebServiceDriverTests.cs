//--------------------------------------------------
// <copyright file="EventFiringWebServiceDriverTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Test the EventFiringWebServiceDriver class</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text;
using WebServiceTesterUnitTesting.Model;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test the EventFiringClientDriver class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EventFiringWebServiceDriverTests : EventFiringWebServiceDriver
    {
        /// <summary>
        /// Default baseAddress for default constructor
        /// </summary>
        private static readonly string baseAddress = WebServiceConfig.GetWebServiceUri();

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiringWebServiceDriverTests"/> class
        /// </summary>
        public EventFiringWebServiceDriverTests() : base(baseAddress)
        {
        }

        /// <summary>
        /// Verify that PostContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(HttpRequestException))]
        public void PostContentThrowException()
        {
            CallContentWithResponse(WebServiceVerb.Post, "BAD", null, null);
        }

        /// <summary>
        /// Verify that PutContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(HttpRequestException))]
        public void PutContentThrowException()
        {
            CallContentWithResponse(WebServiceVerb.Put, "BAD", null, null);
        }

        /// <summary>
        /// Verify that DeleteContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(HttpRequestException))]
        public void DeleteContentThrowException()
        {
            CallWithResponse(WebServiceVerb.Delete, "BAD", null);
        }

        /// <summary>
        /// Verify that GetContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(HttpRequestException))]
        public void GetContentThrowException()
        {
            CallWithResponse(WebServiceVerb.Get, "BAD", null);
        }

        /// <summary>
        /// Verify that CustomContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(HttpRequestException))]
        public void CustomContentThrowException()
        {
            CallContentWithResponse("BAD", "BAD", null, null);
        }

        /// <summary>
        /// Verify post content works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void VerifyPostContent()
        {
            Product p = new Product
            {
                Category = "ff",
                Id = 4,
                Name = "ff",
                Price = 3.25f
            };

            var content = WebServiceUtils.MakeStreamContent<Product>(p, Encoding.UTF8, "application/xml");
            var result = CallContentWithResponse(WebServiceVerb.Post, "/api/XML_JSON/Post", "application/xml", content, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Verify custom content works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void VerifyCustomContent()
        {
            var content = WebServiceUtils.MakeStringContent("ZED?", Encoding.UTF8, "application/json");
            var result = CallContentWithResponse("ZED", "/api/ZED", "application/json", content, false);
            Assert.AreEqual(HttpStatusCode.UseProxy, result.StatusCode);
        }

        /// <summary>
        /// Verify custom content works with a specific response code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void VerifyCustomContentWithResponseCode()
        {
            var content = WebServiceUtils.MakeStringContent("ZED?", Encoding.UTF8, "application/json");
            var result = CallContentWithResponse("ZED", "/api/ZED", "application/json", content, HttpStatusCode.UseProxy);
            Assert.AreEqual(HttpStatusCode.UseProxy, result.StatusCode);
        }

        /// <summary>
        /// Verify custom content works with non standard content type
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void VerifyCustomNonStandardContent()
        {
            var content = WebServiceUtils.MakeStringContent("ZED?", Encoding.UTF8, "application/json");
            var result = CallContentWithResponse("ZED", "/api/ZED", "application/nosj", content, false);
            Assert.AreEqual(HttpStatusCode.UseProxy, result.StatusCode);
        }

        /// <summary>
        /// Verify put content works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void VerifyPutContent()
        {
            Product p = new Product
            {
                Category = "ff",
                Id = 4,
                Name = "ff",
                Price = 3.25f
            };
            var content = WebServiceUtils.MakeStringContent<Product>(p, Encoding.UTF8, "application/xml");
            var result = CallContentWithResponse(WebServiceVerb.Put, "/api/XML_JSON/Put/1", "application/xml", content);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Verify delete content works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void VerifyDeleteContent()
        {
            var result = CallWithResponse(WebServiceVerb.Delete, "/api/String/Delete/1", "text/plain", true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Verify get content works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void VerifyGetContent()
        {
            var result = CallWithResponse(WebServiceVerb.Get, "/api/String/1", "text/plain");
            Assert.IsFalse(string.IsNullOrEmpty(result.Content.ReadAsStringAsync().Result), "Expected a result to be returned");
        }

        /// <summary>
        /// Verify get content works with a specific response code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void VerifyGetContentWithResponseCode()
        {
            var result = CallWithResponse(WebServiceVerb.Get, "/api/String/1", "text/plain", HttpStatusCode.OK);
            Assert.IsFalse(string.IsNullOrEmpty(result.Content.ReadAsStringAsync().Result), "Expected a result to be returned");
        }

        /// <summary>
        /// Verify that CustomContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void UriRespected()
        {
            EventFiringWebServiceDriver standAlone = new EventFiringWebServiceDriver(new Uri(baseAddress));
            Assert.IsTrue(standAlone.HttpClient.BaseAddress.Equals(baseAddress), $"Expected {baseAddress} but got {standAlone.HttpClient.BaseAddress}");
        }
    }
}
