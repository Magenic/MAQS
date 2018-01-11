//--------------------------------------------------
// <copyright file="EventFiringHttpClientWrapperTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Test the EventFiringHttpClientWrapper class</summary>
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
    public class EventFiringHttpClientWrapperTests : EventFiringHttpClientWrapper
    {
        /// <summary>
        /// Default baseAddress for default constructor
        /// </summary>
        private static string baseAddress = WebServiceConfig.GetWebServiceUri();

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiringHttpClientWrapperTests"/> class
        /// </summary>
        public EventFiringHttpClientWrapperTests() : base(baseAddress)
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
            this.PostContent(null, null, null).Wait();
        }

        /// <summary>
        /// Verify that PutContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(AggregateException))]
        public void PutContentThrowException()
        {
             this.PutContent(null, null, null).Wait();
        }

        /// <summary>
        /// Verify that DeleteContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(AggregateException))]
        public void DeleteContentThrowException()
        {
            this.DeleteContent(null, null).Wait();
        }

        /// <summary>
        /// Verify that GetContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(AggregateException))]
        public void GetContentThrowException()
        {
            this.GetContent(null, null).Wait();
        }

        /// <summary>
        /// Verify that CustomContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(AggregateException))]
        public void CustomContentThrowException()
        {
            this.CustomContent(null, null, null, null).Wait();
        }
    }
}
