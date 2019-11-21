﻿//--------------------------------------------------
// <copyright file="WebServiceWithDriverPut.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Put unit tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using WebServiceTesterUnitTesting.Model;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test web service gets using the base test driver
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServiceWithDriverPut : BaseWebServiceTest
    {
        /// <summary>
        /// Verify the string status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutJSONSerializedVerifyStatusCode()
        {
            ProductJson p = new ProductJson
            {
                Category = "ff",
                Id = 4,
                Name = "ff",
                Price = 3.25f
            };
            var content = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var result = this.WebServiceDriver.PutWithResponse("/api/XML_JSON/Put/1", "application/json", content, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Put With JSON Type
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutJSONWithType()
        {
            ProductJson p = new ProductJson
            {
                Category = "ff",
                Id = 4,
                Name = "ff",
                Price = 3.25f
            };
            var content = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var result = this.WebServiceDriver.Put<ProductJson>("/api/XML_JSON/Put/1", "application/json", content, true);
            Assert.AreEqual(null, result);
        }

        /// <summary>
        /// Verify the stream status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutJSONStreamSerializedVerifyStatusCode()
        {
            ProductJson p = new ProductJson
            {
                Category = "ff",
                Id = 4,
                Name = "ff",
                Price = 3.25f
            };
            var content = WebServiceUtils.MakeStreamContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var result = this.WebServiceDriver.PutWithResponse("/api/XML_JSON/Put/1", "application/json", content, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// XML string verify status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutXMLSerializedVerifyStatusCode()
        {
            Product p = new Product
            {
                Category = "ff",
                Id = 4,
                Name = "ff",
                Price = 3.25f
            };
            var content = WebServiceUtils.MakeStringContent<Product>(p, Encoding.UTF8, "application/xml");
            var result = this.WebServiceDriver.PutWithResponse("/api/XML_JSON/Put/1", "application/xml", content);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// XML stream verify status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutXMLStreamSerializedVerifyStatusCode()
        {
            Product p = new Product
            {
                Category = "ff",
                Id = 4,
                Name = "ff",
                Price = 3.25f
            };
            var content = WebServiceUtils.MakeStreamContent<Product>(p, Encoding.UTF8, "application/xml");
            var result = this.WebServiceDriver.PutWithResponse("/api/XML_JSON/Put/1", "application/xml", content);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Verify put returns an empty string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutXMLSerializedVerifyEmptyString()
        {
            Product p = new Product
            {
                Category = "ff",
                Id = 4,
                Name = "ff",
                Price = 3.25f
            };
            var content = WebServiceUtils.MakeStringContent<Product>(p, Encoding.UTF8, "application/xml");
            var result = this.WebServiceDriver.Put("/api/XML_JSON/Put/1", "application/xml", content, true);
            Assert.AreEqual(string.Empty, result);
        }

        /// <summary>
        /// String without using the utility
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutStringWithoutMakeContent()
        {
            var result = this.WebServiceDriver.Put("/api/String/Put/1", "text/plain", "Test", Encoding.UTF8, "text/plain");
            Assert.AreEqual(string.Empty, result);
        }

        /// <summary>
        /// Stream without using the utility
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutStreamWithoutMakeContent()
        {
            var result = this.WebServiceDriver.Put("/api/String/Put/1", "text/plain", "Test", Encoding.UTF8, "text/plain", false, true);
            Assert.AreEqual(string.Empty, result);
        }

        /// <summary>
        /// String using the utility
        /// </summary>        
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutStringWithMakeStringContent()
        {
            var content = WebServiceUtils.MakeStringContent("Test", Encoding.UTF8, "text/plain");
            var result = this.WebServiceDriver.Put("/api/String/Put/1", "text/plain", content, true);
            Assert.AreEqual(string.Empty, result);
        }

        /// <summary>
        /// Stream using the utility
        /// </summary>        
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutStringWithMakeStreamContent()
        {
            StreamContent content = WebServiceUtils.MakeStreamContent("Test", Encoding.UTF8, "text/plain");
            var result = this.WebServiceDriver.Put("/api/String/Put/1", "text/plain", content, true);
            Assert.AreEqual(string.Empty, result);
        }

        /// <summary>
        /// Make stream content with a stream
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutStreamWithMakeStreamContent()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write("TestStream");
            writer.Flush();
            stream.Position = 0;
            
            StreamContent content = WebServiceUtils.MakeStreamContent(stream, "text/plain");
            var result = this.WebServiceDriver.Put("/api/String/Put/1", "text/plain", content, true);
            Assert.AreEqual(string.Empty, result);
        }

        /// <summary>
        /// Put string without utility
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutStringWithoutContentStatusCode()
        {
            var result = this.WebServiceDriver.PutWithResponse("/api/String/Put/1", "text/plain", "Test", Encoding.UTF8, "text/plain", true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Put string with utility
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutStringMakeContentStatusCode()
        {
            var content = WebServiceUtils.MakeStringContent("Test", Encoding.UTF8, "text/plain");
            var result = this.WebServiceDriver.PutWithResponse("/api/String/Put/1", "text/plain", content, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Test other status codes
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutExpectContentError()
        {
            var result = this.WebServiceDriver.PutWithResponse("/api/String/Put/1", "text/plain", null, false);
            Assert.AreEqual(HttpStatusCode.Conflict, result.StatusCode);
        }

        /// <summary>
        /// Test string response
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutExpectStringError()
        {
            var result = this.WebServiceDriver.Put("/api/String/Put/1", "text/plain", null, false);
            Assert.AreEqual("{\"Message\":\"No Product found for name = 1 \"}", result);
        }
    }
}
