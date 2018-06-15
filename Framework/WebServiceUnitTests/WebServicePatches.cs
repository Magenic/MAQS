//--------------------------------------------------
// <copyright file="WebServicePatches.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Web service get unit tests</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseWebServiceTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using WebServiceTesterUnitTesting.Model;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test web service gets
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServicePatches
    {
        /// <summary>
        /// String to hold the URL
        /// </summary>
        private static string url = Config.GetValue("WebServiceUri");
        
        /// <summary>
        /// Verify the string status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchJSONWithoutBaseTest()
        {
            WebServiceDriver client = new WebServiceDriver(new Uri(url));
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var result = client.PatchWithResponse("/api/XML_JSON/Patch/1", "application/json", content, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Patch With JSON Type
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchJSONWithTypeWithoutBaseTest()
        {
            WebServiceDriver client = new WebServiceDriver(new Uri(url));
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var content = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var result = client.Patch<ProductJson>("/api/XML_JSON/Patch/1", "application/json", content, true);
            Assert.AreEqual(p.Category, result.Category);
            Assert.AreEqual(p.Id, result.Id);
            Assert.AreEqual(p.Name, result.Name);
            Assert.AreEqual(p.Price, result.Price);
        }

        /// <summary>
        /// Patch stream without utility
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchStreamWithoutBaseTest()
        {
            WebServiceDriver client = new WebServiceDriver(new Uri(url));
            var result = client.Patch("/api/String/Patch/1", "text/plain", "Test", Encoding.UTF8, "text/plain", false, true);
            Assert.AreEqual("\"Patched\"", result);
        }

        /// <summary>
        /// Patch string with utility
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchStringWithoutBaseTest()
        {
            WebServiceDriver client = new WebServiceDriver(new Uri(url));
            var content = WebServiceUtils.MakeStringContent("Test", Encoding.UTF8, "text/plain");
            var result = client.Patch("/api/String/Patch/1", "text/plain", content, true);
            Assert.AreEqual("\"Patched\"", result);
        }

        /// <summary>
        /// Patch stream without utility to verify status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchStreamWithoutContentWithoutBaseTest()
        {
            WebServiceDriver client = new WebServiceDriver(new Uri(url));
            var result = client.PatchWithResponse("/api/String/Patch/1", "text/plain", "Test", Encoding.UTF8, "text/plain", false, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}