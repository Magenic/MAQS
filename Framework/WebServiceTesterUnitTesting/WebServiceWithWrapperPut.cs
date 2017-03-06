//--------------------------------------------------
// <copyright file="WebServiceWithWrapperPut.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Put unit tests</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseWebServiceTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text;
using WebServiceTesterUnitTesting.Model;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test web service gets using the base test wrapper
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServiceWithWrapperPut : BaseWebServiceTest
    {
        /// <summary>
        /// Did the logging folder exist at the start of the test run
        /// </summary>
        private static bool loggingFolderExistsBeforeRun = false;

        /// <summary>
        /// Setup before we start running selenium tests
        /// </summary>
        /// <param name="context">The upcoming test context</param>
        [ClassInitialize]
        public static void CheckBeforeClass(TestContext context)
        {
            loggingFolderExistsBeforeRun = TestHelper.DoesFolderExist();
        }

        /// <summary>
        /// Cleanup after we are done running selenium tests
        /// </summary>
        [ClassCleanup]
        public static void CleanupAfterClass()
        {
            TestHelper.Cleanup(loggingFolderExistsBeforeRun);
        }

        /// <summary>
        /// Verify the string status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutJSONSerializedVerifyStatusCode()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var result = this.WebServiceWrapper.PutWithResponse("/api/XML_JSON/Put/1", "application/json", content, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Put With JSON Type
        /// </summary>
        #region PutWithType
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutJSONWithType()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var result = this.WebServiceWrapper.Put<ProductJson>("/api/XML_JSON/Put/1", "application/json", content, true);
            Assert.AreEqual(result, null);
        }
        #endregion

        /// <summary>
        /// Verify the stream status code
        /// </summary>
        #region PutWithResponseJSON
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutJSONStreamSerializedVerifyStatusCode()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStreamContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var result = this.WebServiceWrapper.PutWithResponse("/api/XML_JSON/Put/1", "application/json", content, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
        #endregion

        /// <summary>
        /// XML string verify status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutXMLSerializedVerifyStatusCode()
        {
            Product p = new Product();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStringContent<Product>(p, Encoding.UTF8, "application/xml");
            var result = this.WebServiceWrapper.PutWithResponse("/api/XML_JSON/Put/1", "application/xml", content);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// XML stream verify status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutXMLStreamSerializedVerifyStatusCode()
        {
            Product p = new Product();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStreamContent<Product>(p, Encoding.UTF8, "application/xml");
            var result = this.WebServiceWrapper.PutWithResponse("/api/XML_JSON/Put/1", "application/xml", content);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Verify put returns an empty string
        /// </summary>
        #region PutWithXML
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutXMLSerializedVerifyEmptyString()
        {
            Product p = new Product();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStringContent<Product>(p, Encoding.UTF8, "application/xml");
            var result = this.WebServiceWrapper.Put("/api/XML_JSON/Put/1", "application/xml", content, true);
            Assert.AreEqual(string.Empty, result);
        }
        #endregion

        /// <summary>
        /// String without using the utility
        /// </summary>
        #region PutWithString
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutStringWithoutMakeContent()
        {
            var result = this.WebServiceWrapper.Put("/api/String/Put/1", "text/plain", "Test", Encoding.UTF8, "text/plain");
            Assert.AreEqual(string.Empty, result);
        }
        #endregion

        /// <summary>
        /// Stream without using the utility
        /// </summary>
        #region PutWithStringContent
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutStreamWithoutMakeContent()
        {
            var result = this.WebServiceWrapper.Put("/api/String/Put/1", "text/plain", "Test", Encoding.UTF8, "text/plain", false, true);
            Assert.AreEqual(string.Empty, result);
        }
        #endregion

        /// <summary>
        /// String using the utility
        /// </summary>        
        #region MakeStringContent
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutStringWithMakeStringContent()
        {
            var content = WebServiceUtils.MakeStringContent("Test", Encoding.UTF8, "text/plain");
            var result = this.WebServiceWrapper.Put("/api/String/Put/1", "text/plain", content, true);
            Assert.AreEqual(string.Empty, result);
        }
        #endregion

        /// <summary>
        /// Stream using the utility
        /// </summary>        
        #region MakeStreamContent
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutStringWithMakeStreamContent()
        {
            StreamContent content = WebServiceUtils.MakeStreamContent("Test", Encoding.UTF8, "text/plain");
            var result = this.WebServiceWrapper.Put("/api/String/Put/1", "text/plain", content, true);
            Assert.AreEqual(string.Empty, result);
        }
        #endregion

        /// <summary>
        /// Put string without utility
        /// </summary>
        #region PutWithResponse
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutStringWithoutContentStatusCode()
        {
            var result = this.WebServiceWrapper.PutWithResponse("/api/String/Put/1", "text/plain", "Test", Encoding.UTF8, "text/plain", true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
        #endregion

        /// <summary>
        /// Put string with utility
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutStringMakeContentStatusCode()
        {
            var content = WebServiceUtils.MakeStringContent("Test", Encoding.UTF8, "text/plain");
            var result = this.WebServiceWrapper.PutWithResponse("/api/String/Put/1", "text/plain", content, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Test other status codes
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutExpectContentError()
        {
            var result = this.WebServiceWrapper.PutWithResponse("/api/String/Put/1", "text/plain", null, false);
            Assert.AreEqual(HttpStatusCode.Conflict, result.StatusCode);
        }

        /// <summary>
        /// Test string response
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutExpectStringError()
        {
            var result = this.WebServiceWrapper.Put("/api/String/Put/1", "text/plain", null, false);
            Assert.AreEqual("{\"Message\":\"No Product found for name = 1 \"}", result);
        }
    }
}
