//--------------------------------------------------
// <copyright file="WebServiceGets.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Web service get unit tests</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseWebServiceTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Net.Http;
using WebServiceTesterUnitTesting.Model;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test web service gets
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServiceGets
    {
        /// <summary>
        /// String to hold the URL
        /// </summary>
        private static string url = Config.GetValue("WebServiceUri");

        /// <summary>
        /// Make sure the web service have been woken up
        /// </summary>
        /// <param name="context">The test context</param>
        [AssemblyInitialize]
        public static void PrimeSite(TestContext context)
        {
            try
            {
                HttpClientWrapper client = new HttpClientWrapper(new Uri("http://magenicautomation.azurewebsites.net"));
                string result = client.Get("/api/String/1", "text/plain", false);
            }
            catch
            {
                // eat expected error for priming
            }
        }

        /// <summary>
        /// Test XML get
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetXmlDeserialized()
        {
            HttpClientWrapper client = new HttpClientWrapper(new Uri(url));
            ArrayOfProduct result = client.Get<ArrayOfProduct>("/api/XML_JSON/GetAllProducts", "application/xml", false);

            Assert.AreEqual(result.Product.Length, 3, "Expected 3 products to be returned");
        }

        /// <summary>
        /// Test Json Get
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetJsonDeserialized()
        {
            HttpClientWrapper client = new HttpClientWrapper(new Uri(url));
            List<ProductJson> result = client.Get<List<ProductJson>>("/api/XML_JSON/GetAllProducts", "application/json", false);

            Assert.AreEqual(result.Count, 3, "Expected 3 products to be returned");
        }

        /// <summary>
        /// Test string Get
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetString()
        {
            HttpClientWrapper client = new HttpClientWrapper(new Uri("http://magenicautomation.azurewebsites.net")); 
            string result = client.Get("/api/String/1", "text/plain", false);

            Assert.IsTrue(result.Contains("Tomato Soup"), "Was expeting a result with Tomato Soup but instead got - " + result);
        }

        /// <summary>
        /// Test getting an image
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetImage()
        {
            HttpClientWrapper client = new HttpClientWrapper(new Uri(url));
            HttpResponseMessage result = client.GetWithResponse("/api/PNGFile/GetImage?image=Red", "image/png", false);

            // Get the image
            Image image = Image.FromStream(result.Content.ReadAsStreamAsync().Result);

            Assert.AreEqual(image.Width, 200, "Image width should be 200");
            Assert.AreEqual(image.Height, 200, "Image hight should be 200");
        }
    }
}