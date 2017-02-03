//--------------------------------------------------
// <copyright file="SeleniumSoftAssert.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Selenium override for the soft asserts</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.BaseTest;
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Helper;
using Magenic.MaqsFramework.Utilities.Logging;
using OpenQA.Selenium;
using System;

namespace Magenic.MaqsFramework.BaseSeleniumTest
{
    /// <summary>
    /// Soft Assert override for selenium tests
    /// </summary>
    public class SeleniumSoftAssert : SoftAssert
    {
        /// <summary>
        /// WebDriver to be used
        /// </summary>
        private IWebDriver webDriver;

        /// <summary>
        /// Initializes a new instance of the SeleniumSoftAssert class
        /// </summary>
        /// <param name="webDriver">The webdriver to use</param>
        /// <param name="logger">The logger to use</param>
        public SeleniumSoftAssert(IWebDriver webDriver, Logger logger)
            : base(logger)
        {
            this.webDriver = webDriver;
        }

        /// <summary>
        /// Soft assert method to check if the files are equal
        /// </summary>
        /// <param name="expectedText">Expected text</param>
        /// <param name="actualText">Actual text</param>
        /// <param name="message">Exception message if desired</param>
        /// <returns>Boolean if the assert is true</returns>
        public override bool AreEqual(string expectedText, string actualText, string message = "")
        {
            return this.AreEqual(expectedText, actualText, string.Empty, message);
        }

        /// <summary>
        /// Soft assert method to check if the strings are equal
        /// </summary>
        /// <param name="expectedText">Expected text</param>
        /// <param name="actualText">Actual text</param>
        /// <param name="softAssertName">Soft assert name to use</param>
        /// <param name="message">Exception message if desired</param>
        /// <returns>Boolean if the assert is true</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="SoftAssertAreEqual" lang="C#" />
        /// </example>
        public override bool AreEqual(string expectedText, string actualText, string softAssertName, string message = "")
        {
            bool didPass = base.AreEqual(expectedText, actualText, softAssertName, message);

            if (!didPass)
            {
                if (Config.GetValue("SoftAssertScreenshot", "No").ToUpper().Equals("YES"))
                {
                    SeleniumUtilities.CaptureScreenshot(this.webDriver, this.Log, StringProcessor.SafeFormatter(" ({0})", this.NumberOfAsserts));
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Soft assert method to check if the boolean is false
        /// </summary>
        /// <param name="condition">Boolean condition</param>
        /// <param name="softAssertName">Soft assert name</param>
        /// <param name="failureMessage">Failure message</param>
        /// <returns>Boolean of the assert</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="SoftAssertIsFalse" lang="C#" />
        /// </example>
        public override bool IsFalse(bool condition, string softAssertName, string failureMessage = "")
        {
            bool didPass = base.IsFalse(condition, softAssertName, failureMessage);

            if (!didPass)
            {
                if (Config.GetValue("SoftAssertScreenshot", "No").ToUpper().Equals("YES"))
                {
                    SeleniumUtilities.CaptureScreenshot(this.webDriver, this.Log, StringProcessor.SafeFormatter(" ({0})", this.NumberOfAsserts));
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Soft assert method to check if the boolean is false
        /// </summary>
        /// <param name="condition">Boolean condition</param>
        /// <param name="softAssertName">Soft assert name</param>
        /// <param name="failureMessage">Failure message</param>
        /// <returns>Boolean of the assert</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="SoftAssertIsTrue" lang="C#" />
        /// </example>
        public override bool IsTrue(bool condition, string softAssertName, string failureMessage = "")
        {
            bool didPass = base.IsTrue(condition, softAssertName, failureMessage);

            if (!didPass)
            {
                if (Config.GetValue("SoftAssertScreenshot", "No").ToUpper().Equals("YES"))
                {
                    SeleniumUtilities.CaptureScreenshot(this.webDriver, this.Log, StringProcessor.SafeFormatter(" ({0})", this.NumberOfAsserts));
                }

                return false;
            }

            return true;
        }
    }
}
