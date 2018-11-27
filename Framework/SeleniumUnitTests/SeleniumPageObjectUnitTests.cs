//-----------------------------------------------------
// <copyright file="SeleniumPageObjectUnitTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Test the base selenium page object model</summary>
//-----------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Magenic.Maqs.Utilities.Performance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
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

    /// <summary>
    /// Selenium page model class for testing
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SeleniumPageModel : BaseSeleniumPageModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeleniumPageModel"/> class
        /// </summary>
        /// <param name="testObject">The base Selenium test object</param>
        public SeleniumPageModel(SeleniumTestObject testObject) 
            : base(testObject)
        {
        }

        /// <summary>
        /// Gets a parent element
        /// </summary>
        public LazyElement FlowerTableLazyElement
        {
            get { return this.GetLazyElement(By.CssSelector("#FlowerTable"), "Flower table"); }
        }

        /// <summary>
        /// Gets a child element, the second table caption
        /// </summary>
        public LazyElement FlowerTableCaptionWithParent
        {
            get { return this.GetLazyElement(this.FlowerTableLazyElement, By.CssSelector("CAPTION > Strong"), "Flower table caption"); }
        }

        /// <summary>
        /// Check if the page has been loaded
        /// </summary>
        /// <returns>True if the page was loaded</returns>
        public override bool IsPageLoaded()
        {
            return true;
        }

        /// <summary>
        /// Get web driver
        /// </summary>
        /// <returns>The web driver</returns>
        public IWebDriver GetWebDriver()
        {
            return this.WebDriver;
        }

        /// <summary>
        /// Get logger
        /// </summary>
        /// <returns>The logger</returns>
        public Logger GetLogger()
        {
            return this.Log;
        }

        /// <summary>
        /// Get test object
        /// </summary>
        /// <returns>The test object</returns>
        public SeleniumTestObject GetTestObject()
        {
            return this.testObject;
        }

        /// <summary>
        /// Get performance timer collection
        /// </summary>
        /// <returns>The performance timer collection</returns>
        public PerfTimerCollection GetPerfTimerCollection()
        {
            return this.PerfTimerCollection;
        }
    }
}
