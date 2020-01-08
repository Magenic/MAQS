//-----------------------------------------------------
// <copyright file="SeleniumNUnitTest.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>NUnit test the selenium framework</summary>
//-----------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Test the selenium framework for NUnit
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SeleniumNUnitTest : BaseSeleniumTest
    {
        /// <summary>
        /// Make sure we can open a browser
        /// </summary>
        [Test]
        [Category(TestCategories.NUnit)]
        public void OpenBrowser()
        {
            this.WebDriver.Navigate().GoToUrl("https://www.google.com");
        }

        /// <summary>
        /// Make sure we are not doing duplicate logging
        /// </summary>
        [Test]
        [Category(TestCategories.NUnit)]
        public void NoLogFileDuplication()
        {
            string badSelector = "BADNOPENOTGOOD";
            this.Log.SetLoggingLevel(MessageType.INFORMATION);

            this.WebDriver.SetWaitDriver(new WebDriverWait(new SystemClock(), WebDriver, TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100)));

            try
            {
                this.WebDriver.Wait().UntilClickableElement(By.CssSelector(badSelector));
                this.WebDriver.Wait().ForClickableElement(By.CssSelector(badSelector));
            }
            catch
            {
                string text = File.ReadAllText((this.Log as FileLogger).FilePath);
                Assert.AreEqual(1, Regex.Matches(text, badSelector).Count);
                return;
            }

            Assert.Fail("Test should not have found bad selector: " +  badSelector);
        }
    }
}