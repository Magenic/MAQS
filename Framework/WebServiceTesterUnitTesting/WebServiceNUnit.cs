//--------------------------------------------------
// <copyright file="WebServiceNUnit.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Web service get unit tests</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseWebServiceTest;
using Magenic.MaqsFramework.Utilities.Helper;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using WebServiceTesterUnitTesting.Model;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test web service gets
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class WebServiceNUnit
    {
        /// <summary>
        /// String to hold the URL
        /// </summary>
        private static string url = Config.GetValue("WebServiceUri");

        /// <summary>
        /// Test XML get
        /// </summary>
        [Test]
        [Category(TestCategories.NUnit)]
        public void GetXmlDeserialized()
        {
            HttpClientWrapper client = new HttpClientWrapper(new Uri(url));
            ArrayOfProduct result = client.Get<ArrayOfProduct>("/api/XML_JSON/GetAllProducts", "application/xml", false);
        }
    }
}