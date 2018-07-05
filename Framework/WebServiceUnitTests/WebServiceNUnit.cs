//--------------------------------------------------
// <copyright file="WebServiceNUnit.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Web service get unit tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
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
        private static readonly string Url = Config.GetValue("WebServiceUri");

        /// <summary>
        /// Test XML get
        /// </summary>
        [Test]
        [Category(TestCategories.NUnit)]
        public void GetXmlDeserialized()
        {
            WebServiceDriver client = new WebServiceDriver(new Uri(Url));
            ArrayOfProduct result = client.Get<ArrayOfProduct>("/api/XML_JSON/GetAllProducts", "application/xml", false);
        }
    }
}