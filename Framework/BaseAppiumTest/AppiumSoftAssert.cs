//--------------------------------------------------
// <copyright file="AppiumSoftAssert.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>This is the Appium soft assert class</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Data;
using System;

namespace Magenic.Maqs.BaseAppiumTest
{
    /// <summary>
    /// Soft Assert override for appium tests
    /// </summary>
    public class AppiumSoftAssert : SoftAssert
    {
        /// <summary>
        /// AppiumDriver to be used
        /// </summary>
        private readonly AppiumTestObject appiumTestObject;

        /// <summary>
        /// Initializes a new instance of the AppiumSoftAssert class
        /// </summary>
        /// <param name="appiumTestObject">The related Appium test object</param>
        public AppiumSoftAssert(AppiumTestObject appiumTestObject)
            : base(appiumTestObject.Log)
        {
            this.appiumTestObject = appiumTestObject;
        }

        /// <summary>
        /// Soft assert method to check if the files are equal
        /// </summary>
        /// <param name="expectedText">Expected text</param>
        /// <param name="actualText">Actual text</param>
        /// <param name="softAssertName">Soft assert name</param>
        /// <param name="message">Exception message if desired</param>
        /// <returns>Boolean if the assert is true</returns>
        [Obsolete("SoftAssert.AreEqual will be deprecated in MAQS 7.0.  Please use SoftAssert.Assert() instead")]
        public override bool AreEqual(string expectedText, string actualText, string softAssertName, string message = "")
        {
            bool didPass = base.AreEqual(expectedText, actualText, softAssertName, message);

            if (!didPass && this.appiumTestObject.GetDriverManager<MobileDriverManager>().IsDriverIntialized())
            {
                if (AppiumConfig.GetSoftAssertScreenshot())
                {
                    AppiumUtilities.CaptureScreenshot(this.appiumTestObject.AppiumDriver, this.appiumTestObject, this.TextToAppend(softAssertName));
                }

                if (AppiumConfig.GetSavePagesourceOnFail())
                {
                    AppiumUtilities.SavePageSource(this.appiumTestObject.AppiumDriver, this.appiumTestObject, StringProcessor.SafeFormatter(" ({0})", this.NumberOfAsserts));
                }

                return false;
            }
            else if (!didPass)
            {
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
            string appendToFileName;

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
