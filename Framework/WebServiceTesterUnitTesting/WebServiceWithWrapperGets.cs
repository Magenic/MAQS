//--------------------------------------------------
// <copyright file="WebServiceWithWrapperGets.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Web service get unit tests with the base test wrapper</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseWebServiceTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
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
    public class WebServiceWithWrapperGets : BaseWebServiceTest
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
        /// Test XML get
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetXmlDeserialized()
        {
            ArrayOfProduct result = this.WebServiceWrapper.Get<ArrayOfProduct>("/api/XML_JSON/GetAllProducts", "application/xml");
            Assert.AreEqual(result.Product.Length, 3, "Expected 3 products to be returned");
        }

        /// <summary>
        /// Test that we can use the web service utility to deserialize XML
        /// </summary>
        #region DeserializeXmlDocument
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetResponseAndDeserializeXml()
        {
            HttpResponseMessage message = this.WebServiceWrapper.GetWithResponse("/api/XML_JSON/GetAllProducts", "application/xml");
            ArrayOfProduct result = WebServiceUtils.DeserializeXmlDocument<ArrayOfProduct>(message);
            Assert.AreEqual(result.Product.Length, 3, "Expected 3 products to be returned");
        }
        #endregion

        /// <summary>
        /// Test Json Get
        /// </summary>
        #region GetWithType
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetJsonDeserialized()
        {
            List<ProductJson> result = this.WebServiceWrapper.Get<List<ProductJson>>("/api/XML_JSON/GetAllProducts", "application/json");
            Assert.AreEqual(result.Count, 3, "Expected 3 products to be returned");
        }
        #endregion

        /// <summary>
        /// Test that we can use the web service utility to deserialize JSON
        /// </summary>
        #region DeserializeJson
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetResponseAndDeserializeJson()
        {
            HttpResponseMessage message = this.WebServiceWrapper.GetWithResponse("/api/XML_JSON/GetAllProducts", "application/json");
            List<ProductJson> result = WebServiceUtils.DeserializeJson<List<ProductJson>>(message);
            Assert.AreEqual(result.Count, 3, "Expected 3 products to be returned");
        }
        #endregion

        /// <summary>
        /// Test string Get
        /// </summary>
        #region GetWithString
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetString()
        {
            string result = this.WebServiceWrapper.Get("/api/String/1", "text/plain");
            Assert.IsTrue(result.Contains("Tomato Soup"), "Was expeting a result with Tomato Soup but instead got - " + result);
        }
        #endregion

        /// <summary>
        /// Test getting an image
        /// </summary>
        #region GetWithImage
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetImage()
        {
            HttpResponseMessage result = this.WebServiceWrapper.GetWithResponse("/api/PNGFile/GetImage?image=Red", "image/png");

            // Get the image
            Image image = Image.FromStream(result.Content.ReadAsStreamAsync().Result);
            Assert.AreEqual(image.Width, 200, "Image width should be 200");
            Assert.AreEqual(image.Height, 200, "Image hight should be 200");
        }
        #endregion
    }
}
