using Magenic.MaqsFramework.BaseAppiumTest;
using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace $safeprojectname$
{
    /// <summary>
    /// Page object for $safeitemname$
    /// </summary>
    public class $safeitemname$
    {
		/// <summary>
        /// The user name input element 'By' finder
        /// </summary>
        private static By userNameLabel = By.XPath("//*/UIAStaticText[@value='loginUsername']");

        /// <summary>
        /// The password input element 'By' finder
        /// </summary>
        private static By passwordLabel = By.XPath("//*/UIAStaticText[@value='loginPassword']");

        /// <summary>
        /// Appium test object
        /// </summary>
        private AppiumTestObject testObject;

        /// <summary>
        /// Initializes a new instance of the <see cref=" $safeitemname$" /> class.
        /// </summary>
        /// <param name="testObject">The appium test object</param>
        public  $safeitemname$(AppiumTestObject testObject)
        {
            this.testObject = testObject;
        }

        /// <summary>
        /// Get username text from label
        /// </summary>
        /// <returns>username text string</returns>
        public string GetLoggedInUsername()
        {
            return this.testObject.AppiumDriver.Wait().ForVisibleElement(userNameLabel).Text;
        }

        /// <summary>
        /// Get password text from label
        /// </summary>
        /// /// <returns>password text string</returns>
        public string GetLoggedInPassword()
        {
            return this.testObject.AppiumDriver.Wait().ForVisibleElement(passwordLabel).Text;
        }
    }
}