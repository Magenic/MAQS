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
        private static ProductJson product = new ProductJson
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
            HttpClientWrapper wrapper = new HttpClientWrapper(this.GetBaseWebServiceUrl());
            this.WebServiceWrapper = wrapper;
            Assert.AreEqual(this.ObjectUnderTest.ToString(), wrapper.ToString());
        }

        /// <summary>
        /// Verify that SetupNoneEventFiringTester sets the ObjectUnderTest as WebClientWrapper without an event firing wrapper
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void WebServiceSetupNoneEventFiringTesterTest()
        {
            HttpClientWrapper wrapper = new HttpClientWrapper(this.GetBaseWebServiceUrl());
            this.SetupNoneEventFiringTester();
            Assert.AreEqual(this.ObjectUnderTest.ToString(), wrapper.ToString());
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
        /// Verify that HttpClientWrapper Constructor overload properly creates wrapper
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void HttpClientWrapperConstructorTest()
        {
            Uri testUri = new Uri(this.GetBaseWebServiceUrl());
            HttpClientWrapper testWrapper = new HttpClientWrapper(testUri, new List<MediaTypeFormatter>());
            Assert.IsNotNull(testWrapper);
            Assert.AreEqual(testWrapper.BaseUriAddress, testUri);
        }

        /// <summary>
        /// Verify that HttpClientWrapper Constructor overload properly creates wrapper
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void HttpClientWrapperConstructorTest2()
        {
            HttpClientWrapper testWrapper = new HttpClientWrapper(this.GetBaseWebServiceUrl(), new CustomXmlMediaTypeFormatter("application/xml", typeof(string)));
            Assert.IsNotNull(testWrapper);
            Assert.AreEqual(testWrapper.BaseUriAddress, this.GetBaseWebServiceUrl());
        }

        /// <summary>
        /// Verify that HttpClientWrapper Constructor overload properly creates wrapper
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void HttpClientWrapperConstructorTest3()
        {
            HttpClientWrapper testWrapper = new HttpClientWrapper(this.GetBaseWebServiceUrl(), new List<MediaTypeFormatter>());
            Assert.IsNotNull(testWrapper);
            Assert.AreEqual(testWrapper.BaseUriAddress, this.GetBaseWebServiceUrl());
        }

        /// <summary>
        /// Verify that BaseHttpClient setter properly sets the client
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void BaseHttpClientSetterTest()
        {
            HttpClientWrapper testWrapper = new HttpClientWrapper(this.GetBaseWebServiceUrl(), new List<MediaTypeFormatter>());
            HttpClient client = new HttpClient();
            testWrapper.BaseHttpClient = client;
            Assert.AreEqual(testWrapper.BaseHttpClient.ToString(), client.ToString());
            Assert.AreEqual(testWrapper.BaseHttpClient.Timeout.ToString(), client.Timeout.ToString());
        }

        /// <summary>
        /// Verify that WebServiceUtils.DeserializeResponse throws expected exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(Exception))]
        public void WebServiceUtilsDeserializeResponseThrownException()
        {
            StringContent content = WebServiceUtils.MakeStringContent<ProductJson>(product, Encoding.UTF8, "application/json");
            HttpResponseMessage response = this.WebServiceWrapper.PutWithResponse("/api/XML_JSON/GetAnErrorPLZ", "application/json", content, false);
            ProductJson retObject = WebServiceUtils.DeserializeResponse<ProductJson>(response, new List<MediaTypeFormatter> { new CustomXmlMediaTypeFormatter("image/gif", typeof(ProductJson)) });
        }

        /// <summary>
        /// Verify that WebServiceUtils.MakeStreamContent throws expected exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(Exception))]
        public void MakeStreamContentThrowException()
        {
            StreamContent streamContent = WebServiceUtils.MakeStreamContent<ProductJson>(product, Encoding.UTF8, "notsupported");
        }

        /// <summary>
        /// Verify that WebServiceUtils.MakeStringContent throws expected exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(Exception))]
        public void MakeStringContentThrowException()
        {
            StringContent streamContent = WebServiceUtils.MakeStringContent<ProductJson>(product, Encoding.UTF8, "notsupported");
        }
    }
}
