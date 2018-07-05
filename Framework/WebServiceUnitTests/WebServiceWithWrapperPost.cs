//--------------------------------------------------
// <copyright file="WebServiceWithWrapperPost.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Web service post unit tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using WebServiceTesterUnitTesting.Model;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test class for unit tests
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServiceWithWrapperPost : BaseWebServiceTest
    {
        /// <summary>
        /// Post JSON request to verify status codes
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostJSONSerializedVerifyStatusCode()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var result = this.WebServiceWrapper.PostWithResponse("/api/XML_JSON/Post", "application/json", content, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Post JSON stream request to verify status codes
        /// </summary>
        #region PostWithResponseContent
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostJSONStreamSerializedVerifyStatusCode()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStreamContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var result = this.WebServiceWrapper.PostWithResponse("/api/XML_JSON/Post", "application/json", content, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
        #endregion

        /// <summary>
        /// Post XML request to verify status codes
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostXMLSerializedVerifyStatusCode()
        {
            Product p = new Product();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStringContent<Product>(p, Encoding.UTF8, "application/xml");
            var result = this.WebServiceWrapper.PostWithResponse("/api/XML_JSON/Post", "application/xml", content, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Post XML request to verify status codes
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostXMLStreamSerializedVerifyStatusCode()
        {
            Product p = new Product();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStreamContent<Product>(p, Encoding.UTF8, "application/xml");
            var result = this.WebServiceWrapper.PostWithResponse("/api/XML_JSON/Post", "application/xml", content, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Post with JSON
        /// </summary>
        #region PostWithType
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostWithJson()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStreamContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var result = this.WebServiceWrapper.Post<ProductJson>("/api/XML_JSON/Post", "application/json", content, true);
            Assert.IsTrue(result == null);
        }
        #endregion

        /// <summary>
        /// Post XML to verify no string is returned
        /// </summary>
        #region PostWithString
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostXMLSerializedVerifyEmptyString()
        {
            Product p = new Product();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStringContent<Product>(p, Encoding.UTF8, "application/xml");
            var result = this.WebServiceWrapper.Post("/api/XML_JSON/Post", "application/xml", content, true);
            Assert.AreEqual(string.Empty, result);
        }
        #endregion

        /// <summary>
        /// Post string without utility
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostStringWithoutMakeContent()
        {
            var result = this.WebServiceWrapper.Post("/api/String", "text/plain", "Test", Encoding.UTF8, "text/plain");
            Assert.AreEqual(string.Empty, result);
        }

        /// <summary>
        /// Post stream without utility
        /// </summary>
        #region PostWithoutCreatingContent
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostStreamWithoutMakeContent()
        {
            var result = this.WebServiceWrapper.Post("/api/String", "text/plain", "Test", Encoding.UTF8, "text/plain", false, true);
            Assert.AreEqual(string.Empty, result);
        }
        #endregion

        /// <summary>
        /// Post string with utility
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostStringWithMakeContent()
        {
            var content = WebServiceUtils.MakeStringContent("Test", Encoding.UTF8, "text/plain");
            var result = this.WebServiceWrapper.Post("/api/String", "text/plain", content, true);
            Assert.AreEqual(string.Empty, result);
        }

        /// <summary>
        /// Post string without utility to verify status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostStringWithoutContentStatusCode()
        {
            var result = this.WebServiceWrapper.PostWithResponse("/api/String", "text/plain", "Test", Encoding.UTF8, "text/plain", true, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Post stream without utility to verify status code
        /// </summary>
        #region PostWithResponse
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostStreamWithoutContentStatusCode()
        {
            var result = this.WebServiceWrapper.PostWithResponse("/api/String", "text/plain", "Test", Encoding.UTF8, "text/plain", false, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
        #endregion

        /// <summary>
        /// Post string with utility to verify status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostStringMakeContentStatusCode()
        {
            var content = WebServiceUtils.MakeStringContent("Test", Encoding.UTF8, "text/plain");
            var result = this.WebServiceWrapper.PostWithResponse("/api/String", "text/plain", content, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Verifying other http status codes
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostExpectContentError()
        {
            var result = this.WebServiceWrapper.PostWithResponse("/api/String", "text/plain", null, false);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }

        /// <summary>
        /// Testing string returned
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostExpectStringError()
        {
            var result = this.WebServiceWrapper.Post("/api/String", "text/plain", null, false);
            Assert.AreEqual("{\"Message\":\"value is required\"}", result);
        }
    }
}
