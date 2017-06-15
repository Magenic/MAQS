//--------------------------------------------------
// <copyright file="WebServiceWithWrapperPatch.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Put unit tests</summary>
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
    /// Test web service gets using the base test wrapper
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServiceWithWrapperPatch : BaseWebServiceTest
    {
        /// <summary>
        /// Did the logging folder exist at the start of the test run
        /// </summary>
        private static bool loggingFolderExistsBeforeRun = false;

        /// <summary>
        /// Setup before running tests
        /// </summary>
        /// <param name="context">The upcoming test context</param>
        [ClassInitialize]
        public static void CheckBeforeClass(TestContext context)
        {
            loggingFolderExistsBeforeRun = TestHelper.DoesFolderExist();
        }

        /// <summary>
        /// Cleanup after we are done running tests
        /// </summary>
        [ClassCleanup]
        public static void CleanupAfterClass()
        {
            TestHelper.Cleanup(loggingFolderExistsBeforeRun);
        }

        /// <summary>
        /// Verify the string status code
        /// </summary>
        #region PatchWithResponseContent
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchJSONSerializedVerifyStatusCode()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var result = this.WebServiceWrapper.PatchWithResponse("/api/XML_JSON/Patch/1", "application/json", content, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
        #endregion

        /// <summary>
        /// Patch With JSON Type
        /// </summary>
        #region PatchWithType
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchJSONWithType()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var result = this.WebServiceWrapper.Patch<ProductJson>("/api/XML_JSON/Patch/1", "application/json", content, true);
            Assert.AreEqual(p.Category, result.Category);
            Assert.AreEqual(p.Id, result.Id);
            Assert.AreEqual(p.Name, result.Name);
            Assert.AreEqual(p.Price, result.Price);
        }
        #endregion

        /// <summary>
        /// Verify the stream status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchJSONStreamSerializedVerifyStatusCode()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStreamContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var result = this.WebServiceWrapper.PatchWithResponse("/api/XML_JSON/Patch/1", "application/json", content, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// XML string verify status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchXMLSerializedVerifyStatusCode()
        {
            Product p = new Product();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStringContent<Product>(p, Encoding.UTF8, "application/xml");
            var result = this.WebServiceWrapper.PatchWithResponse("/api/XML_JSON/Patch/1", "application/xml", content);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// XML stream verify status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchXMLStreamSerializedVerifyStatusCode()
        {
            Product p = new Product();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStreamContent<Product>(p, Encoding.UTF8, "application/xml");
            var result = this.WebServiceWrapper.PatchWithResponse("/api/XML_JSON/Patch/1", "application/xml", content);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Verify put returns an empty string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchXMLWithType()
        {
            Product p = new Product();
            p.Category = "food";
            p.Id = 1;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStringContent<Product>(p, Encoding.UTF8, "application/xml");
            var result = this.WebServiceWrapper.Patch<Product>("/api/XML_JSON/Patch/1", "application/xml", content, true);
            Assert.AreEqual(p.Category, result.Category);
            Assert.AreEqual(p.Id, result.Id);
            Assert.AreEqual(p.Name, result.Name);
            Assert.AreEqual(p.Price, result.Price);
        }
        
        /// <summary>
        /// Patch string without utility
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchStringWithoutMakeContent()
        {
            var result = this.WebServiceWrapper.Patch("/api/String/Patch/1", "text/plain", "Test", Encoding.UTF8, "text/plain");
            Assert.AreEqual("\"Patched\"", result);
        }

        /// <summary>
        /// Patch stream without utility
        /// </summary>
        #region PatchWithoutCreatingContent
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchStreamWithoutMakeContent()
        {
            var result = this.WebServiceWrapper.Patch("/api/String/Patch/1", "text/plain", "Test", Encoding.UTF8, "text/plain", false, true);
            Assert.AreEqual("\"Patched\"", result);
        }
        #endregion

        /// <summary>
        /// Patch string with utility
        /// </summary>
        #region PatchWithString
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchStringWithMakeContent()
        {
            var content = WebServiceUtils.MakeStringContent("Test", Encoding.UTF8, "text/plain");
            var result = this.WebServiceWrapper.Patch("/api/String/Patch/1", "text/plain", content, true);
            Assert.AreEqual("\"Patched\"", result);
        }
        #endregion

        /// <summary>
        /// Patch string with utility
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchStreamWithMakeContent()
        {
            var content = WebServiceUtils.MakeStreamContent("Test", Encoding.UTF8, "text/plain");
            var result = this.WebServiceWrapper.Patch("/api/String/Patch/1", "text/plain", content, true);
            Assert.AreEqual("\"Patched\"", result);
        }

        /// <summary>
        /// Patch string without utility to verify status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchStringWithoutContentStatusCode()
        {
            var result = this.WebServiceWrapper.PatchWithResponse("/api/String/Patch/1", "text/plain", "Test", Encoding.UTF8, "text/plain", true, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Patch stream without utility to verify status code
        /// </summary>
        #region PatchWithResponse
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchStreamWithoutContentStatusCode()
        {
            var result = this.WebServiceWrapper.PatchWithResponse("/api/String/Patch/1", "text/plain", "Test", Encoding.UTF8, "text/plain", false, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
        #endregion

        /// <summary>
        /// Patch string with utility to verify status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchStringMakeContentStatusCode()
        {
            var content = WebServiceUtils.MakeStringContent("Test", Encoding.UTF8, "text/plain");
            var result = this.WebServiceWrapper.PatchWithResponse("/api/String/Patch/1", "text/plain", content, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Verifying other http status codes
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchExpectContentError()
        {
            var result = this.WebServiceWrapper.PatchWithResponse("/api/String/Patch/1", "text/plain", null, false);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        /// <summary>
        /// Testing string returned for Patch
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchExpectStringError()
        {
            var result = this.WebServiceWrapper.Patch("/api/String/Patch/1", "text/plain", null, false);
            var expected = "{\"Message\":\"Value is required\"}";
            Assert.IsTrue(result.Equals(expected), $"Assert Failed. Expected:{expected}, Actual:{result}");
        }
    }
}
