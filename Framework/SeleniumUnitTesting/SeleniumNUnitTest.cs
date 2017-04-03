//-----------------------------------------------------
// <copyright file="SeleniumNUnitTest.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>NUnit test the selenium framework</summary>
//-----------------------------------------------------
using Magenic.MaqsFramework.BaseSeleniumTest;
using Magenic.MaqsFramework.Utilities.Helper;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

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
        /// Did the logging folder exist at the start of the test run
        /// </summary>
        private static bool loggingFolderExistsBeforeRun = false;

        /// <summary>
        /// Setup before running tests
        /// </summary>
        [OneTimeSetUp]
        public static void CheckBeforeClass()
        {
            loggingFolderExistsBeforeRun = TestHelper.DoesFolderExist();
        }

        /// <summary>
        /// Cleanup after we are done running tests
        /// </summary>
        [OneTimeTearDown]
        public static void CleanupAfterClass()
        {
            TestHelper.Cleanup(loggingFolderExistsBeforeRun);
        }

        /// <summary>
        /// Make sure we can open a browser
        /// </summary>
        [Test]
        [Category(TestCategories.NUnit)]
        public void OpenBrowser()
        {
            this.WebDriver.Navigate().GoToUrl("https://www.google.com");
        }
    }
}