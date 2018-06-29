//--------------------------------------------------
// <copyright file="WebServiceBaseTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Web service general unit tests</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseWebServiceTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using WebServiceTesterUnitTesting.Model;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test web service base test
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServiceBaseTests : BaseWebServiceTest
    {
        /// <summary>
        /// Default ProductJson object for use in tests
        /// </summary>
        private static readonly ProductJson Product = new ProductJson
        {
            Category = "ff",
            Id = 4,
            Name = "ff",
            Price = 3.25f
        };

        /// <summary>
        /// Verify that the webService wrapper can be properly set 
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void SetWebServiceWrapper()
        {
            WebServiceDriver wrapper = new WebServiceDriver(this.GetBaseWebServiceUrl());
            this.WebServiceWrapper = wrapper;
            Assert.AreEqual(this.TestObject.WebServiceDriver.ToString(), wrapper.ToString());
        }

        /// <summary>
        /// Verify that SetupNoneEventFiringTester sets the ObjectUnderTest as WebClientWrapper without an event firing wrapper
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void WebServiceSetupNoneEventFiringTesterTest()
        {
            // Turn off logging
            Config.AddTestSettingValues(new Dictionary<string, string> { { "Log", "NO" } }, true);

            WebServiceDriver wrapper = new WebServiceDriver(this.GetBaseWebServiceUrl());
            Assert.AreEqual(this.TestObject.WebServiceDriver.ToString(), wrapper.ToString());
        }

        /// <summary>
        /// Verify that if EnsureSuccessStatusCode gets a false value it throws an exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(AggregateException))]
        public void EnsureSuccessStatusCodeThrownException()
        {
            var result = this.WebServiceWrapper.Post("notaurl", "image/GIF", null);
        }

        /// <summary>
        /// Verify that WebServiceDriver Constructor overload properly creates wrapper
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void WebServiceDriverConstructorTest()
        {
            Uri testUri = new Uri(this.GetBaseWebServiceUrl());
            WebServiceDriver testWrapper = new WebServiceDriver(testUri);
            testWrapper.SetCustomMediaFormatters(new List<MediaTypeFormatter>());
            Assert.IsNotNull(testWrapper);
            Assert.AreEqual(testWrapper.HttpClient.BaseAddress, testUri);
        }

        /// <summary>
        /// Verify that WebServiceDriver Constructor overload properly creates wrapper
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void WebServiceDriverConstructorTest2()
        {
            WebServiceDriver testWrapper = new WebServiceDriver(this.GetBaseWebServiceUrl());
            testWrapper.SetCustomMediaFormatters(new CustomXmlMediaTypeFormatter("application/xml", typeof(string)));
            Assert.IsNotNull(testWrapper);
            Assert.AreEqual(testWrapper.HttpClient.BaseAddress, this.GetBaseWebServiceUrl());
        }

        /// <summary>
        /// Verify that WebServiceDriver Constructor overload properly creates wrapper
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void WebServiceDriverConstructorTest3()
        {
            WebServiceDriver testWrapper = new WebServiceDriver(this.GetBaseWebServiceUrl());
            testWrapper.SetCustomMediaFormatters(new List<MediaTypeFormatter>());
            Assert.IsNotNull(testWrapper);
            Assert.AreEqual(testWrapper.HttpClient.BaseAddress, this.GetBaseWebServiceUrl());
        }

        /// <summary>
        /// Verify that BaseHttpClient setter properly sets the client
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void BaseHttpClientSetterTest()
        {
            WebServiceDriver testWrapper = new WebServiceDriver(this.GetBaseWebServiceUrl());
            testWrapper.SetCustomMediaFormatters(new List<MediaTypeFormatter>());
            HttpClient client = new HttpClient();
            testWrapper.HttpClient = client;
            Assert.AreEqual(testWrapper.HttpClient.ToString(), client.ToString());
            Assert.AreEqual(testWrapper.HttpClient.Timeout.ToString(), client.Timeout.ToString());
        }

        /// <summary>
        /// Verify that WebServiceUtils.DeserializeResponse throws expected exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WebServiceUtilsDeserializeResponseThrownException()
        {
            StringContent content = WebServiceUtils.MakeStringContent<ProductJson>(Product, Encoding.UTF8, "application/json");
            HttpResponseMessage response = this.WebServiceWrapper.PutWithResponse("/api/XML_JSON/GetAnErrorPLZ", "application/json", content, false);
            ProductJson retObject = WebServiceUtils.DeserializeResponse<ProductJson>(response, new List<MediaTypeFormatter> { new CustomXmlMediaTypeFormatter("image/gif", typeof(ProductJson)) });
        }

        /// <summary>
        /// Verify that WebServiceUtils.MakeStreamContent throws expected exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(NotSupportedException))]
        public void MakeStreamContentThrowException()
        {
            StreamContent streamContent = WebServiceUtils.MakeStreamContent<ProductJson>(Product, Encoding.UTF8, "notsupported");
        }

        /// <summary>
        /// Verify that WebServiceUtils.MakeStringContent throws expected exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(NotSupportedException))]
        public void MakeStringContentThrowException()
        {
            StringContent streamContent = WebServiceUtils.MakeStringContent<ProductJson>(Product, Encoding.UTF8, "notsupported");
        }
    }
}
