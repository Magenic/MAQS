//--------------------------------------------------
// <copyright file="WebServiceMediaType.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Web service get unit tests using MediaType strings</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
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
    /// Test web service gets using the base test driver
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServiceMediaType : BaseWebServiceTest
    {
        /// <summary>
        /// Test XML get
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService), TestCategory(TestCategories.MediaType)]
        public void GetXmlDeserialized()
        {
            ArrayOfProduct result = this.WebServiceDriver.Get<ArrayOfProduct>("/api/XML_JSON/GetAllProducts", "application/xml");
            Assert.AreEqual(result.Product.Length, 3, "Expected 3 products to be returned");
        }

        /// <summary>
        /// Test that we can use the web service utility to deserialize XML
        /// </summary>
        #region DeserializeXmlDocument
        [TestMethod]
        [TestCategory(TestCategories.WebService), TestCategory(TestCategories.MediaType)]
        public void GetResponseAndDeserializeXml()
        {
            HttpResponseMessage message = this.WebServiceDriver.GetWithResponse("/api/XML_JSON/GetAllProducts", MediaType.AppXml);
            ArrayOfProduct result = WebServiceUtils.DeserializeXmlDocument<ArrayOfProduct>(message);
            Assert.AreEqual(result.Product.Length, 3, "Expected 3 products to be returned");
        }
        #endregion

        /// <summary>
        /// Test Json Get
        /// </summary>
        #region GetWithType
        [TestMethod]
        [TestCategory(TestCategories.WebService), TestCategory(TestCategories.MediaType)]
        public void GetJsonDeserialized()
        {
            List<ProductJson> result = this.WebServiceDriver.Get<List<ProductJson>>("/api/XML_JSON/GetAllProducts", MediaType.AppJson);
            Assert.AreEqual(result.Count, 3, "Expected 3 products to be returned");
        }
        #endregion

        /// <summary>
        /// Test that we can use the web service utility to deserialize JSON
        /// </summary>
        #region DeserializeJson
        [TestMethod]
        [TestCategory(TestCategories.WebService), TestCategory(TestCategories.MediaType)]
        public void GetResponseAndDeserializeJsonM()
        {
            HttpResponseMessage message = this.WebServiceDriver.GetWithResponse("/api/XML_JSON/GetAllProducts", MediaType.AppJson);
            List<ProductJson> result = WebServiceUtils.DeserializeJson<List<ProductJson>>(message);
            Assert.AreEqual(result.Count, 3, "Expected 3 products to be returned");
        }
        #endregion

        /// <summary>
        /// Test string Get
        /// </summary>
        #region GetWithString
        [TestMethod]
        [TestCategory(TestCategories.WebService), TestCategory(TestCategories.MediaType)]
        public void GetString()
        {
            string result = this.WebServiceDriver.Get("/api/String/1", MediaType.PlainText);
            Assert.IsTrue(result.Contains("Tomato Soup"), "Was expeting a result with Tomato Soup but instead got - " + result);
        }
        #endregion

        /// <summary>
        /// Test getting an image
        /// </summary>
        #region GetWithImage
        [TestMethod]
        [TestCategory(TestCategories.WebService), TestCategory(TestCategories.MediaType)]
        public void GetImage()
        {
            HttpResponseMessage result = this.WebServiceDriver.GetWithResponse("/api/PNGFile/GetImage?image=Red", MediaType.ImagePng);

            // Get the image
            Image image = Image.FromStream(result.Content.ReadAsStreamAsync().Result);
            Assert.AreEqual(image.Width, 200, "Image width should be 200");
            Assert.AreEqual(image.Height, 200, "Image hight should be 200");
        }
        #endregion
    }
}
