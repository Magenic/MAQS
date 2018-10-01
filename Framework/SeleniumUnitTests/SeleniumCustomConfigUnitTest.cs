//-----------------------------------------------------
// <copyright file="SeleniumCustomConfigUnitTest.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Test the selenium framework with a custom configuration</summary>
//-----------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Reflection;

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
        private readonly string googleUrl = "https://www.magenic.com";

        /// <summary>
        /// Verify WaitForAbsentElement wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void VerifyCustomBrowserUsed()
        {
            this.WebDriver.Navigate().GoToUrl(this.googleUrl);

            Assert.AreEqual(501, this.WebDriver.Manage().Window.Size.Width, "Override sets the with to 501");
            Assert.AreEqual(199, this.WebDriver.Manage().Window.Size.Height, "Override sets the height to 199");
        }

        /// <summary>
        /// Override the web driver we user for these tests
        /// </summary>
        /// <returns>The web driver we want to use - Web driver override</returns>
        protected override IWebDriver GetBrowser()
        {
            IWebDriver driver = SeleniumConfig.Browser("Chrome");
            driver.Manage().Window.Size = new Size(501, 199);

            return driver;
        }
    }
}