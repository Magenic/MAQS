//-----------------------------------------------------
// <copyright file="SeleniumPageModel.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>A test selenium page object model</summary>
//-----------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.Utilities.Logging;
using Magenic.Maqs.Utilities.Performance;
using OpenQA.Selenium;
using System.Diagnostics.CodeAnalysis;

namespace SeleniumUnitTests
{
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
        public SeleniumPageModel(ISeleniumTestObject testObject) 
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
        public ILogger GetLogger()
        {
            return this.Log;
        }

        /// <summary>
        /// Get test object
        /// </summary>
        /// <returns>The test object</returns>
        public ISeleniumTestObject GetTestObject()
        {
            return this.TestObject;
        }

        /// <summary>
        /// Get performance timer collection
        /// </summary>
        /// <returns>The performance timer collection</returns>
        public IPerfTimerCollection GetPerfTimerCollection()
        {
            return this.PerfTimerCollection;
        }
    }
}
