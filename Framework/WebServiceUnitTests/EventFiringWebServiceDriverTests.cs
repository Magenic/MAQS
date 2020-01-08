//--------------------------------------------------
// <copyright file="EventFiringWebServiceDriverTests.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Test the EventFiringWebServiceDriver class</summary>
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
    /// Test the EventFiringClientDriver class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EventFiringWebServiceDriverTests : EventFiringWebServiceDriver
    {
        /// <summary>
        /// Default baseAddress for default constructor
        /// </summary>
        private static string baseAddress = WebServiceConfig.GetWebServiceUri();

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
        [ExpectedException(typeof(AggregateException))]
        public void PostContentThrowException()
        {
            PostContent("BAD", null, null).Wait();
        }

        /// <summary>
        /// Verify that PutContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(AggregateException))]
        public void PutContentThrowException()
        {
            PutContent("BAD", null, null).Wait();
        }

        /// <summary>
        /// Verify that DeleteContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(AggregateException))]
        public void DeleteContentThrowException()
        {
            DeleteContent("BAD", null).Wait();
        }

        /// <summary>
        /// Verify that GetContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(AggregateException))]
        public void GetContentThrowException()
        {
            GetContent("BAD", null).Wait();
        }

        /// <summary>
        /// Verify that CustomContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(AggregateException))]
        public void CustomContentThrowException()
        {
            CustomContent("BAD", null, null, null).Wait();
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
            var result = PostContent("/api/XML_JSON/Post", "application/xml", content, true).Result;
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
            var result = CustomContent("/api/ZED", "ZED", "application/json", content, false).Result;
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
            var result = PutContent("/api/XML_JSON/Put/1", "application/xml", content).Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Verify delete content works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void VerifyDeleteContent()
        {
            var result = DeleteContent("/api/String/Delete/1", "text/plain", true).Result;
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Verify get content works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void VerifyGetContent()
        {
            var result = GetContent("/api/String/1", "text/plain").Result;
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
