using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
using Magenic.MaqsFramework.BaseAppiumTest;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using NUnit.Framework;

namespace $safeprojectname$.Appium
{
    /// <summary>
    /// Page object
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
        /// Initializes a new instance of the <see cref="$safeitemname$" /> class.
        /// </summary>
        /// <param name="testObject">The Appium test object</param>
        public $safeitemname$(AppiumTestObject testObject)
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
