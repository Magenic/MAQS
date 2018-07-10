//--------------------------------------------------
// <copyright file="WebServiceNonStandardHttpContentTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Web service get unit tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using WebServiceTesterUnitTesting.Model;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test web service gets
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServiceNonStandardHttpContentTests : BaseWebServiceTest
    {
        /// <summary>
        /// String to hold the URL
        /// </summary>
        private static readonly string Url = Config.GetGeneralValue("WebServiceUri");

        #region NonStandardStreamContentWithStream
        /// <summary>
        /// Verify the string status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void MakeNonStandardStreamContentStreamTest()
        {
            // Non Standard Content Type stuff
            var randomData = Guid.NewGuid();
            var randomData2 = Guid.NewGuid();
            string formDataBoundary = $"----------{randomData}";

            MultipartFormDataContent multiPartContent = new MultipartFormDataContent(formDataBoundary);

            //// Write stream
            byte[] data = Encoding.ASCII.GetBytes(randomData.ToString());
            byte[] data2 = Encoding.ASCII.GetBytes(randomData2.ToString());

            MemoryStream memStream = new MemoryStream(data.Length);
            memStream.Write(data, 0, data.Length);

            MemoryStream memStream2 = new MemoryStream(data2.Length);
            memStream2.Write(data2, 0, data2.Length);

            //// Method to Test
            var content = WebServiceUtils.MakeNonStandardStreamContent(memStream, "multipart/form-data");
            var content2 = WebServiceUtils.MakeNonStandardStreamContent(memStream2, "multipart/form-data");

            multiPartContent.Add(content, "MyTaxReturns2017", "RandomTestData.abc");
            multiPartContent.Add(content2, "MyTripPhoto", "RandomTestData2.def");

            var result = this.TestObject.WebServiceDriver.Post<FilesUploaded>("api/upload", "application/json", multiPartContent, true);

            var file1 = result.Files.FirstOrDefault();
            var file2 = result.Files.LastOrDefault();

            Assert.IsNotNull(file1);
            Assert.IsNotNull(file2);

            Assert.AreEqual("RandomTestData.abc", file1.FileName, $"File uploaded did not match 'RandomTestData.abc'. Actual is '{file1.FileName}'");
            Assert.AreEqual("RandomTestData2.def", file2.FileName, $"File uploaded did not match 'RandomTestData2.def'. Actual is '{file2.FileName}'");

            Assert.AreEqual("MyTaxReturns2017", file1.ContentName, $"File uploaded did not match 'MyTaxReturns2017'. Actual is '{file1.ContentName}'");
            Assert.AreEqual("MyTripPhoto", file2.ContentName, $"File uploaded did not match 'MyTripPhoto'. Actual is '{file2.ContentName}'");
        }
        #endregion

        #region NonStandardStreamContentWithString
        /// <summary>
        /// Verify the string status code
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void MakeNonStandardStreamContentStringTest()
        {
            // Non Standard Content Type stuff
            var randomData = Guid.NewGuid();
            var randomData2 = Guid.NewGuid();
            string formDataBoundary = $"----------{randomData}";

            MultipartFormDataContent multiPartContent = new MultipartFormDataContent(formDataBoundary);

            //// Method to Test
            var content = WebServiceUtils.MakeNonStandardStreamContent(randomData.ToString(), Encoding.ASCII, "multipart/form-data");
            var content2 = WebServiceUtils.MakeNonStandardStreamContent(randomData2.ToString(), Encoding.ASCII, "multipart/form-data");

            multiPartContent.Add(content, "MyResume", "Resume.abc");
            multiPartContent.Add(content2, "MyDefintion", "MyDefintion.def");

            var result = this.TestObject.WebServiceDriver.Post<FilesUploaded>("api/upload", "application/json", multiPartContent, true);

            var file1 = result.Files.FirstOrDefault();
            var file2 = result.Files.LastOrDefault();

            Assert.IsNotNull(file1);
            Assert.IsNotNull(file2);

            Assert.AreEqual("Resume.abc", file1.FileName, $"File uploaded did not match 'Resume.abc'. Actual is '{file1.FileName}'");
            Assert.AreEqual("MyDefintion.def", file2.FileName, $"File uploaded did not match 'MyDefintion.def'. Actual is '{file2.FileName}'");

            Assert.AreEqual("MyResume", file1.ContentName, $"File uploaded did not match 'MyResume'. Actual is '{file1.ContentName}'");
            Assert.AreEqual("MyDefintion", file2.ContentName, $"File uploaded did not match 'MyDefintion'. Actual is '{file2.ContentName}'");
        }
        #endregion
    }
}