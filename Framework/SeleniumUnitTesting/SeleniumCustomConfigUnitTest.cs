//-----------------------------------------------------
// <copyright file="SeleniumCustomConfigUnitTest.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Test the selenium framework with a custom configuration</summary>
//-----------------------------------------------------
using Magenic.MaqsFramework.BaseSeleniumTest;
using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System.Diagnostics.CodeAnalysis;
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
        /// Did the logging folder exist at the start of the test run
        /// </summary>
        private static bool loggingFolderExistsBeforeRun = false;

        /// <summary>
        /// Google URL
        /// </summary>
        private string googleUrl = "https://www.google.com";

        /// <summary>
        /// Google search box css selector for standard visual browser
        /// <para>This selector should not be included in the Phantom JS configuration this code uses</para>
        /// </summary>
        private string searchBoxCssSelector = "#lst-ib";

        /// <summary>
        /// Setup before running tests
        /// </summary>
        /// <param name="context">The upcoming test context</param>
        [ClassInitialize]
        public static void CheckBeforeClass(TestContext context)
        {
            loggingFolderExistsBeforeRun = TestHelper.DoesFolderExist();
        }

        /// <summary>
        /// Cleanup after we are done running tests
        /// </summary>
        [ClassCleanup]
        public static void CleanupAfterClass()
        {
            TestHelper.Cleanup(loggingFolderExistsBeforeRun);
        }

        /// <summary>
        /// Verify WaitForAbsentElement wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void VerifyCustomBrowserUsed()
        {
            this.WebDriver.Navigate().GoToUrl(this.googleUrl);

            // With our default driver this selector will be found
            this.WebDriver.Wait().ForAbsentElement(By.CssSelector(this.searchBoxCssSelector));
        }

        /// <summary>
        /// Override the web driver we user for these tests
        /// </summary>
        /// <returns>The web driver we want to use - Web driver override</returns>
        protected override IWebDriver GetBrowser()
        {
            IWebDriver webDriver;

            string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string binaryPath = Path.GetFullPath(Path.Combine(currentPath, "..\\..\\..\\Binaries"));

            if (File.Exists(Path.Combine(currentPath, "phantomjs.exe")))
            {
                webDriver = new PhantomJSDriver(currentPath);
            }
            else
            {
                webDriver = new PhantomJSDriver(binaryPath);
            }

            return webDriver;
        }
    }
}