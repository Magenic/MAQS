//--------------------------------------------------
// <copyright file="EventFiringWebServiceDriverTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Test the EventFiringWebServiceDriver class</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseWebServiceTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test the EventFiringClientWrapper class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EventFiringWebServiceDriverTests : EventFiringWebServiceDriver
    {
        /// <summary>
        /// Default baseAddress for default constructor
        /// </summary>
        private static string baseAddress = WebServiceConfig.GetWebServiceUri();

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiringWebServiceDriverTests"/> class
        /// </summary>
        public EventFiringWebServiceDriverTests() : base(baseAddress)
        {
        }

        /// <summary>
        /// Verify that PostContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(AggregateException))]
        public void PostContentThrowException()
        {
            this.PostContent("BAD", null, null).Wait();
        }

        /// <summary>
        /// Verify that PutContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(AggregateException))]
        public void PutContentThrowException()
        {
             this.PutContent("BAD", null, null).Wait();
        }

        /// <summary>
        /// Verify that DeleteContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(AggregateException))]
        public void DeleteContentThrowException()
        {
            this.DeleteContent("BAD", null).Wait();
        }

        /// <summary>
        /// Verify that GetContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(AggregateException))]
        public void GetContentThrowException()
        {
            this.GetContent("BAD", null).Wait();
        }

        /// <summary>
        /// Verify that CustomContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(AggregateException))]
        public void CustomContentThrowException()
        {
            this.CustomContent("BAD", null, null, null).Wait();
        }
    }
}
