//--------------------------------------------------
// <copyright file="AppiumWinAppUnitTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Test class for windows application driver related functions</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseAppiumTest;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;

namespace AppiumUnitTests
{
    /// <summary>
    /// Windows application driver related Appium tests
    /// </summary>
    [TestClass]
    [DoNotParallelize]
    public class AppiumWinAppUnitTests : BaseAppiumTest
    {
        /// <summary>
        /// The notepad application
        /// </summary>
        private static NotepadPageModel NotepadApplication;

        /// <summary>
        /// Tests the creation of the Appium Windows application driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void AppiumWinAppDriverTest()
        {
            string testString = "A test this is";

            NotepadApplication = new NotepadPageModel(this.TestObject);
            NotepadApplication.TextEditor.SendKeys(testString);

            this.SoftAssert.Assert(() => Assert.AreEqual(this.Log, NotepadApplication.GetLogger(), "Expected same logger"));
            this.SoftAssert.Assert(() => Assert.AreEqual(this.PerfTimerCollection, NotepadApplication.GetPerfTimerCollection(), "Expected same perf collection"));
            this.SoftAssert.Assert(() => Assert.AreEqual(this.AppiumDriver, NotepadApplication.GetAppiumDriver(), "Expected same logger"));

            this.SoftAssert.Assert(() => Assert.IsTrue(NotepadApplication.IsPageLoaded(), "Expect page is loaded"));
            this.SoftAssert.Assert(() => Assert.IsTrue(NotepadApplication.TextEditor.Enabled, "Expect enabled"));
            this.SoftAssert.Assert(() => Assert.IsTrue(NotepadApplication.TextEditor.Displayed, "Expect displayed"));
            this.SoftAssert.Assert(() => Assert.IsFalse(NotepadApplication.DontSave.ExistsNow, "Expect not to exist now"));

            this.SoftAssert.Assert(() => Assert.AreEqual(testString, this.AppiumDriver.FindElementByName("Text Editor").Text));

            NotepadApplication.OverrideDriver(null);
            this.SoftAssert.Assert(() => Assert.AreEqual(null, NotepadApplication.GetAppiumDriver()));

            NotepadApplication.OverrideDriver(this.AppiumDriver);

            this.SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Cleanup after application
        /// </summary>
        protected override void BeforeLoggingTeardown(TestResultType resultType)
        {
            // Make sure we get all the logging info
            base.BeforeLoggingTeardown(resultType);

            // Cleanup after the app
            NotepadApplication?.CloseAndDontSave();
            NotepadApplication = null;
        }

        /// <summary>
        /// Sets capabilities for testing the Windows application driver creation
        /// </summary>
        /// <returns>Windows application driver instance of the Appium Driver</returns>
        protected override AppiumDriver<IWebElement> GetMobileDevice()
        {
            AppiumOptions options = new AppiumOptions();
            options.AddAdditionalCapability("app", $"{Environment.SystemDirectory}\\notepad.exe");
            return AppiumDriverFactory.GetWindowsDriver(new Uri("http://127.0.0.1:4723"), options, TimeSpan.FromSeconds(30));
        }
    }
}
