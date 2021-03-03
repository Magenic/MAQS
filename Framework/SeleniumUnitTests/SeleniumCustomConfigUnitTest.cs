﻿//-----------------------------------------------------
// <copyright file="SeleniumCustomConfigUnitTest.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Test the selenium framework with a custom configuration</summary>
//-----------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Test the selenium framework
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SeleniumCustomConfigUnitTest : BaseSeleniumTest
    {
        /// <summary>
        /// Google URL
        /// </summary>
        private readonly string urlToNavigate = "https://github.com/Magenic/MAQS";

        /// <summary>
        /// Verify WaitForAbsentElement wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void VerifyCustomBrowserUsed()
        {
            this.WebDriver.Navigate().GoToUrl(this.urlToNavigate);

            Assert.AreEqual(701, this.WebDriver.Manage().Window.Size.Width, "Override sets the with to 701");
            Assert.AreEqual(199, this.WebDriver.Manage().Window.Size.Height, "Override sets the height to 199");
        }

        /// <summary>
        /// Override the web driver we user for these tests
        /// </summary>
        /// <returns>The web driver we want to use - Web driver override</returns>
        protected override IWebDriver GetBrowser()
        {
            IWebDriver driver = WebDriverFactory.GetBrowserWithDefaultConfiguration(BrowserType.Firefox);
            driver.Manage().Window.Size = new Size(701, 199);

            return driver;
        }
    }
}