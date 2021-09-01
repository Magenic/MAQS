//--------------------------------------------------
// <copyright file="AppiumSoftAssert.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>This is the Appium soft assert class</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
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
        private readonly IAppiumTestObject appiumTestObject;

        /// <summary>
        /// Initializes a new instance of the AppiumSoftAssert class
        /// </summary>
        /// <param name="appiumTestObject">The related Appium test object</param>
        public AppiumSoftAssert(IAppiumTestObject appiumTestObject)
            : base(appiumTestObject.Log)
        {
            this.appiumTestObject = appiumTestObject;
        }

        /// <summary>
        /// Soft assert method to check if the Action is false
        /// </summary>
        /// <param name="assertFunction">Function to use</param>
        /// <param name="failureMessage">Message to log</param>
        /// <param name="assertName">Soft assert name or name of expected assert being called.</param>
        /// <returns>Boolean of the assert</returns>
        public override bool Assert(Action assertFunction, string assertName, string failureMessage = "")
        {
            bool didPass = base.Assert(assertFunction, assertName, failureMessage);

            if (!didPass && this.appiumTestObject.GetDriverManager<AppiumDriverManager>().IsDriverIntialized())
            {
                if (AppiumConfig.GetSoftAssertScreenshot())
                {
                    AppiumUtilities.CaptureScreenshot(this.appiumTestObject.AppiumDriver, this.appiumTestObject, this.TextToAppend(assertName));
                }

                if (AppiumConfig.GetSavePagesourceOnFail())
                {
                    AppiumUtilities.SavePageSource(this.appiumTestObject.AppiumDriver, this.appiumTestObject, $" ({ this.NumberOfAsserts})");
                }

                return false;
            }
            return didPass;
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
                appendToFileName = $" ({this.NumberOfAsserts})";
            }
            else
            {
                // Make sure that softAssertName has valid file name characters only
                foreach (char invalidChar in System.IO.Path.GetInvalidFileNameChars())
                {
                    softAssertName = softAssertName.Replace(invalidChar, '~');
                }

                // If softAssertName is provided, use combination of softAssertName and AssertNumber
                appendToFileName = " " + softAssertName + $" ({this.NumberOfAsserts})";
            }

            return appendToFileName;
        }
    }
}
