//--------------------------------------------------
// <copyright file="AppiumSoftAssert.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>This is the Appium soft assert class</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.BaseTest;
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Helper;
using Magenic.MaqsFramework.Utilities.Logging;
using OpenQA.Selenium.Appium;

namespace Magenic.MaqsFramework.BaseAppiumTest
{
    /// <summary>
    /// Soft Assert override for appium tests
    /// </summary>
    public class AppiumSoftAssert : SoftAssert
    {
        /// <summary>
        /// AppiumDriver to be used
        /// </summary>
        private AppiumDriver<AppiumWebElement> appiumDriver;

        /// <summary>
        /// Initializes a new instance of the AppiumSoftAssert class
        /// </summary>
        /// <param name="driver">The appium driver to use</param>
        /// <param name="logger">The logger to use</param>
        public AppiumSoftAssert(AppiumDriver<AppiumWebElement> driver, Logger logger)
            : base(logger)
        {
            this.appiumDriver = driver;
        }

        /// <summary>
        /// Soft assert method to check if the files are equal
        /// </summary>
        /// <param name="expectedText">Expected text</param>
        /// <param name="actualText">Actual text</param>
        /// <param name="softAssertName">Soft assert name</param>
        /// <param name="message">Exception message if desired</param>
        /// <returns>Boolean if the assert is true</returns>
        public override bool AreEqual(string expectedText, string actualText, string softAssertName, string message = "")
        {
            bool didPass = base.AreEqual(expectedText, actualText, softAssertName, message);
            if (!didPass)
            {
                if (Config.GetValue("SoftAssertScreenshot", "No").ToUpper().Equals("YES"))
                {
                    AppiumUtilities.CaptureScreenshot(this.appiumDriver, this.Log, StringProcessor.SafeFormatter(" ({0})", this.NumberOfAsserts));
                }

                return false;
            }

            return true;
        }
    }
}
