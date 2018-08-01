//--------------------------------------------------
// <copyright file="WebServiceWithDriver.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Web service general unit tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using WebServiceTesterUnitTesting.Model;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test web service driver status code testing
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServiceStatusCodes : BaseWebServiceTest
    {
        /// <summary>
        /// Test type parameterized Get request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetTypeParamWithExpectedStatus()
        {
            var res = this.WebServiceDriver.Get<ArrayOfProduct>("/api/XML_JSON/GetAllProducts", "application/xml", HttpStatusCode.OK);
            Assert.IsNotNull(res);
        }

        /// <summary>
        /// Test Get request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetWithExpectedStatus()
        {
            var res = this.WebServiceDriver.Get("/api/XML_JSON/GetAllProducts", "application/xml", HttpStatusCode.OK);
            Assert.IsNotNull(res);
        }

        /// <summary>
        /// Test Get with response request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetWithResponseWithExpectedStatus()
        {
            var res = this.WebServiceDriver.GetWithResponse("/api/XML_JSON/GetAllProducts", "application/xml", HttpStatusCode.OK);
            Assert.IsNotNull(res);
        }

        /// <summary>
        /// Test type parameterized Post request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostTypeParamWithExpectedStatus()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var req = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var res = this.WebServiceDriver.Post<ProductJson>("/api/XML_JSON/Post", "application/json", req, HttpStatusCode.OK);
            Assert.IsNull(res);
        }

        /// <summary>
        /// Test Post request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostWithExpectedStatus()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var req = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var res = this.WebServiceDriver.Post("/api/XML_JSON/Post", "application/json", req, HttpStatusCode.OK);
            Assert.IsNotNull(res);
        }

        /// <summary>
        /// Test more parameters Post request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostMoreParamsWithExpectedStatus()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var req = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var res = this.WebServiceDriver.Post("/api/XML_JSON/Post", "application/json", JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json", HttpStatusCode.OK, true);
            Assert.IsNotNull(res);
        }

        /// <summary>
        /// Test more parameters Post with response request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostMoreParamsWithResponseWithExpectedStatus()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var req = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var res = this.WebServiceDriver.PostWithResponse("/api/XML_JSON/Post", "application/json", JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json", HttpStatusCode.OK, true);
            Assert.IsNotNull(res);
        }
        
        /// <summary>
        /// Test Post with response request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PostWithResponseWithExpectedStatus()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var req = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var res = this.WebServiceDriver.PostWithResponse("/api/XML_JSON/Post", "application/json", req, HttpStatusCode.OK);
            Assert.IsNotNull(res);
        }

        /// <summary>
        /// Test type parameterized Put request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutTypeParamWithExpectedStatus()
        {
            ProductJson p = new ProductJson
            {
                Category = "ff",
                Id = 4,
                Name = "ff",
                Price = 3.25f
            };
            var req = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var res = this.WebServiceDriver.Put<ProductJson>("/api/XML_JSON/Put/1", "application/json", req, HttpStatusCode.OK);
            Assert.IsNull(res);
        }

        /// <summary>
        /// Test Put request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutWithExpectedStatus()
        {
            ProductJson p = new ProductJson
            {
                Category = "ff",
                Id = 4,
                Name = "ff",
                Price = 3.25f
            };
            var req = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var res = this.WebServiceDriver.Put("/api/XML_JSON/Put/1", "application/json", req, HttpStatusCode.OK);
            Assert.IsNotNull(res);
        }

        /// <summary>
        /// Test more params Put request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutMoreParamsWithExpectedStatus()
        {
            ProductJson p = new ProductJson
            {
                Category = "ff",
                Id = 4,
                Name = "ff",
                Price = 3.25f
            };
            var req = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var res = this.WebServiceDriver.Put("/api/XML_JSON/Put/1", "application/json", req.ToString(), Encoding.UTF8, "application/json", HttpStatusCode.Conflict, true);
            Assert.IsNotNull(res);
        }

        /// <summary>
        /// Test more params Put with response request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutMoreParamsWithResponseWithExpectedStatus()
        {
            ProductJson p = new ProductJson
            {
                Category = "ff",
                Id = 4,
                Name = "ff",
                Price = 3.25f
            };
            var req = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var res = this.WebServiceDriver.PutWithResponse("/api/XML_JSON/Put/1", "application/json", req.ToString(), Encoding.UTF8, "application/json", HttpStatusCode.Conflict, true);
            Assert.IsNotNull(res);
        }

        /// <summary>
        /// Test Put with response request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PutWithResponseWithExpectedStatus()
        {
            ProductJson p = new ProductJson
            {
                Category = "ff",
                Id = 4,
                Name = "ff",
                Price = 3.25f
            };
            var req = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var res = this.WebServiceDriver.PutWithResponse("/api/XML_JSON/Put/1", "application/json", req, HttpStatusCode.OK);
            Assert.IsNotNull(res);
        }

        /// <summary>
        /// Test type parameterized Patch request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchTypeParamWithExpectedStatus()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var req = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var res = this.WebServiceDriver.Patch<ProductJson>("/api/XML_JSON/Patch/1", "application/json", req, HttpStatusCode.OK);
            Assert.IsNotNull(res);
        }

        /// <summary>
        /// Test Patch request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchWithExpectedStatus()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var req = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var res = this.WebServiceDriver.Patch("/api/XML_JSON/Patch/1", "application/json", req, HttpStatusCode.OK);
            Assert.IsNotNull(res);
        }

        /// <summary>
        /// Test more params Patch request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchMoreParamsWithExpectedStatus()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var req = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var res = this.WebServiceDriver.Patch("/api/XML_JSON/Patch/1", "application/json", JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json", HttpStatusCode.OK, true);
            Assert.IsNotNull(res);
        }

        /// <summary>
        /// Test more params Patch with response request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchMoreParamsWithResponseWithExpectedStatus()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var req = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var res = this.WebServiceDriver.PatchWithResponse("/api/XML_JSON/Patch/1", "application/json", JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json", HttpStatusCode.OK, true);
            Assert.IsNotNull(res);
        }

        /// <summary>
        /// Test Patch with response request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void PatchWithResponseWithExpectedStatus()
        {
            ProductJson p = new ProductJson();
            p.Category = "ff";
            p.Id = 4;
            p.Name = "ff";
            p.Price = 3.25f;
            var req = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
            var res = this.WebServiceDriver.PatchWithResponse("/api/XML_JSON/Patch/1", "application/json", req, HttpStatusCode.OK);
            Assert.IsNotNull(res);
        }

        /// <summary>
        /// Test type parameterized Delete request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void DeleteTypeParamWithExpectedStatus()
        {
            var result = this.WebServiceDriver.Delete<ProductJson>("/api/XML_JSON/Delete/1", "application/json", HttpStatusCode.OK);
            Assert.AreEqual(result, null);
        }

        /// <summary>
        /// Test Delete request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void DeleteWithExpectedStatus()
        {
            var result = this.WebServiceDriver.Delete("/api/XML_JSON/Delete/1", "application/json", HttpStatusCode.OK);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test Delete with response request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void DeleteWithResponseWithExpectedStatus()
        {
            var result = this.WebServiceDriver.DeleteWithResponse("/api/XML_JSON/Delete/1", "application/json", HttpStatusCode.OK);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test type param Custom request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomTypeParamWithExpectedStatus()
        {
            var content = WebServiceUtils.MakeStringContent("ZED?", Encoding.UTF8, "application/json");
            var result = this.WebServiceDriver.Custom<object>("ZED", "/api/ZED", "application/json", content, HttpStatusCode.UseProxy);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test Custom request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomWithExpectedStatus()
        {
            var content = WebServiceUtils.MakeStringContent("ZED?", Encoding.UTF8, "application/json");
            var result = this.WebServiceDriver.Custom("ZED", "/api/ZED", "application/json", content, HttpStatusCode.UseProxy);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test Custom with response request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomWithResponseWithExpectedStatus()
        {
            var content = WebServiceUtils.MakeStringContent("ZED?", Encoding.UTF8, "application/json");
            var result = this.WebServiceDriver.CustomWithResponse("ZED", "/api/ZED", "application/json", content, HttpStatusCode.UseProxy);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test more params Custom request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomMoreParamsWithExpectedStatus()
        {
            var content = WebServiceUtils.MakeStringContent("ZED?", Encoding.UTF8, "application/json");
            var result = this.WebServiceDriver.Custom("ZED", "/api/ZED", "application/json", content.ToString(), Encoding.UTF8, "application/json", HttpStatusCode.UseProxy, true);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test more params Custom with response request with expected status
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void CustomMoreParamsWithResponseWithExpectedStatus()
        {
            var content = WebServiceUtils.MakeStringContent("ZED?", Encoding.UTF8, "application/json");
            var result = this.WebServiceDriver.CustomWithResponse("ZED", "/api/ZED", "application/json", content.ToString(), Encoding.UTF8, "application/json", HttpStatusCode.UseProxy, true);
            Assert.IsNotNull(result);
        }
    }
}
