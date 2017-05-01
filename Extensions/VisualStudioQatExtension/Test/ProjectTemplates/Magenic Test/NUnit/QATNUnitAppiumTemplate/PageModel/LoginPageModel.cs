using Magenic.MaqsFramework.BaseAppiumTest;
using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using NUnit.Framework;

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
        private static By userNameInput = By.XPath("//*/UIATextField[@value='Username']");

        /// <summary>
        /// The password input element 'By' finder
        /// </summary>
        private static By passwordInput = By.XPath("//*/UIASecureTextField[@value='Password']");

        /// <summary>
        /// The login button element 'By' finder
        /// </summary>
        private static By loginButton = By.XPath("//*/UIAButton[@name='Login']");

        /// <summary>
        /// The login error message element 'By' finder
        /// </summary>
        private static By loginErrorMessage = By.XPath("//*/UIAStaticText[@label='ErrorMessage']");

        /// <summary>
        /// Appium test object
        /// </summary>
        private AppiumTestObject testObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="$safeitemname$" /> class.
        /// </summary>
        /// <param name="testObject">The appium test object</param>
        public $safeitemname$(AppiumTestObject testObject)
        {
            this.testObject = testObject;
        }

        /// <summary>
        /// Enter the use credentials
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <param name="password">The user password</param>
        public void EnterCredentials(string userName, string password)
        {
            this.testObject.AppiumDriver.Wait().ForVisibleElement(userNameInput).SendKeys(userName);
            this.testObject.AppiumDriver.Wait().ForVisibleElement(passwordInput).SendKeys(password);
        }

        /// <summary>
        /// Enter the use credentials and try to log in - Verify login failed
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <param name="password">The user password</param>
        public void LoginWithInvalidCredentials(string userName, string password)
        {
            this.testObject.AppiumDriver.Wait().ForVisibleElement(userNameInput).SendKeys(userName);
            this.testObject.AppiumDriver.Wait().ForVisibleElement(passwordInput).SendKeys(password);
            this.testObject.AppiumDriver.FindElement(loginButton).Click();
            this.testObject.AppiumDriver.Wait().ForVisibleElement(loginErrorMessage);
        }

        /// <summary>
        /// Enter the use credentials and log in - Navigation sample
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <param name="password">The user password</param>
        /// <returns>Home Page Object Model</returns>
        public HomePageModel LoginWithValidCredentials(string userName, string password)
        {
            this.testObject.AppiumDriver.Wait().ForVisibleElement(userNameInput).SendKeys(userName);
            this.testObject.AppiumDriver.Wait().ForVisibleElement(passwordInput).SendKeys(password);
            this.testObject.AppiumDriver.FindElement(loginButton).Click();
            return new HomePageModel(this.testObject);
        }

        /// <summary>
        /// Get text from error message label
        /// </summary>
        /// <returns>Error Message text string</returns>
        public string GetErrorMessage()
        {
            return this.testObject.AppiumDriver.Wait().ForVisibleElement(loginErrorMessage).Text;
        }
    } 
}