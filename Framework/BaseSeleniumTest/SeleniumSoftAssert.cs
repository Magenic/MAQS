//--------------------------------------------------
// <copyright file="SeleniumSoftAssert.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Selenium override for the soft asserts</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Helper;

namespace Magenic.Maqs.BaseSeleniumTest
{
    /// <summary>
    /// Soft Assert override for selenium tests
    /// </summary>
    public class SeleniumSoftAssert : SoftAssert
    {
        /// <summary>
        /// WebDriver to be used
        /// </summary>
        private readonly SeleniumTestObject testObject;

        /// <summary>
        /// Initializes a new instance of the SeleniumSoftAssert class
        /// </summary>
        /// <param name="seleniumTestObject">The related Selenium test object</param>
        public SeleniumSoftAssert(SeleniumTestObject seleniumTestObject)
            : base(seleniumTestObject.Log)
        {
            this.testObject = seleniumTestObject;
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
                if (SeleniumConfig.GetSoftAssertScreenshot())
                {
                    SeleniumUtilities.CaptureScreenshot(this.testObject.WebDriver, this.Log, this.testObject, this.TextToAppend(softAssertName));
                }

                if (SeleniumConfig.GetSavePagesourceOnFail())
                {
                    SeleniumUtilities.SavePageSource(this.testObject.WebDriver, this.Log, this.testObject, StringProcessor.SafeFormatter(" ({0})", this.NumberOfAsserts));
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
                if (SeleniumConfig.GetSoftAssertScreenshot())
                {
                    SeleniumUtilities.CaptureScreenshot(this.testObject.WebDriver, this.Log, this.testObject, this.TextToAppend(softAssertName));
                }

                if (SeleniumConfig.GetSavePagesourceOnFail())
                {
                    SeleniumUtilities.SavePageSource(this.testObject.WebDriver, this.Log, this.testObject, StringProcessor.SafeFormatter(" ({0})", this.NumberOfAsserts));
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
                if (SeleniumConfig.GetSoftAssertScreenshot())
                {
                    SeleniumUtilities.CaptureScreenshot(this.testObject.WebDriver, this.Log, this.testObject, this.TextToAppend(softAssertName));
                }

                if (SeleniumConfig.GetSavePagesourceOnFail())
                {
                    SeleniumUtilities.SavePageSource(this.testObject.WebDriver, this.Log, this.testObject, StringProcessor.SafeFormatter(" ({0})", this.NumberOfAsserts));
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Method to determine the text to be appended to the screenshot file names
        /// </summary>
        /// <param name="softAssertName">Soft assert name</param>
        /// <returns>String to be appended</returns>
        private string TextToAppend(string softAssertName)
        {
            string appendToFileName = string.Empty;

            // If softAssertName name is not provided only append the AssertNumber
            if (softAssertName == string.Empty)
            {
                appendToFileName = StringProcessor.SafeFormatter(" ({0})", this.NumberOfAsserts);
            }
            else
            {
                // Make sure that softAssertName has valid file name characters only
                foreach (char invalidChar in System.IO.Path.GetInvalidFileNameChars())
                {
                    softAssertName = softAssertName.Replace(invalidChar, '~');
                }

                // If softAssertName is provided, use combination of softAssertName and AssertNumber 
                appendToFileName = " " + softAssertName + StringProcessor.SafeFormatter(" ({0})", this.NumberOfAsserts);
            }

            return appendToFileName;
        }
    }
}
