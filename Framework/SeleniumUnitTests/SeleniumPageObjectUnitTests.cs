//-----------------------------------------------------
// <copyright file="SeleniumPageObjectUnitTests.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Test the base selenium page object model</summary>
//-----------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Test the base selenium page object model
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SeleniumPageObjectUnitTests : BaseSeleniumTest
    {
        /// <summary>
        /// Base Selenium page model
        /// </summary>
        private SeleniumPageModel basePageModel;

        /// <summary>
        /// Setup test Selenium page model
        /// </summary>
        [TestInitialize]
        public void CreateSeleniumPageModel()
        {
            this.WebDriver.Navigate().GoToUrl(SeleniumConfig.GetWebSiteBase() + "Automation");
            this.WebDriver.Wait().ForPageLoad();
            this.basePageModel = new SeleniumPageModel(this.TestObject);
        }

        /// <summary>
        /// Verify test object is the same
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void PageModelTestObject()
        {
            Assert.AreEqual(this.TestObject, this.basePageModel.GetTestObject());
        }

        /// <summary>
        /// Verify web driver is the same
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void PageModelWebDriver()
        {
            Assert.AreEqual(this.WebDriver, this.basePageModel.GetWebDriver());
        }

        /// <summary>
        /// Verify logger is the same
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void PageModelLogger()
        {
            Assert.AreEqual(this.Log, this.basePageModel.GetLogger());
        }

        /// <summary>
        /// Verify perf timer collection is the same
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void PageModelPerfTimerCollection()
        {
            Assert.AreEqual(this.PerfTimerCollection, this.basePageModel.GetPerfTimerCollection());
        }

        /// <summary>
        /// Verify existing lazy element is returned from element store
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetLazyElementReturnsExistingElement()
        {
            var lazyElement = this.basePageModel.FlowerTableLazyElement;
            lazyElement.GetTheExistingElement();
            Assert.AreEqual(lazyElement.CachedElement, this.basePageModel.FlowerTableLazyElement.CachedElement);
        }

        /// <summary>
        /// Verify existing lazy element with parent is returned from element store
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetLazyElementReturnsExistingElementWithParent()
        {
            var lazyElement = this.basePageModel.FlowerTableCaptionWithParent;
            lazyElement.GetTheExistingElement();
            Assert.AreEqual(lazyElement.CachedElement, this.basePageModel.FlowerTableCaptionWithParent.CachedElement);
        }
    }
}
