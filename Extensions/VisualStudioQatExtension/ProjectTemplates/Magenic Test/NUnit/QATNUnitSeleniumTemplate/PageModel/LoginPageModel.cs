using Magenic.MaqsFramework.BaseSeleniumTest;
using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
using Magenic.MaqsFramework.Utilities.Helper;
using NUnit.Framework;
using OpenQA.Selenium;

namespace $safeprojectname$
{
    /// <summary>
    /// Page object for the Automation page
    /// </summary>
    public class $safeitemname$ : BasePageModel
    {
        /// <summary>
        /// The page url
        /// </summary>
        private static string PageUrl = Config.GetValue("WebSiteBase") + "Static/Training3/loginpage.html";

        /// <summary>
        /// Initializes a new instance of the <see cref="$safeitemname$" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public $safeitemname$(SeleniumTestObject testObject) : base(testObject)
        {
        }

        /// <summary>
        /// Gets user name box
        /// </summary>
        private LazyElement UserNameInput
        {
            get { return new LazyElement(this.testObject, By.CssSelector("#name"), "User name input"); }
        }

        /// <summary>
        /// Gets password box
        /// </summary>
        private LazyElement PasswordInput
        {
            get { return new LazyElement(this.testObject, By.CssSelector("#pw"), "Password input"); }
        }

        /// <summary>
        /// Gets login button
        /// </summary>
        private LazyElement LoginButton
        {
            get { return new LazyElement(this.testObject, By.CssSelector("#Login"), "Login button"); }
        }

        /// <summary>
        /// Gets error message
        /// </summary>
        private LazyElement ErrorMessage
        {
            get { return new LazyElement(this.testObject, By.CssSelector("#LoginError"), "Error message"); }
        }

        /// <summary>
        /// Open the login page
        /// </summary>
        public void OpenLoginPage()
        {
            this.testObject.WebDriver.Navigate().GoToUrl(PageUrl);
            this.AssertPageLoaded();
        }

        /// <summary>
        /// Enter the use credentials
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <param name="password">The user password</param>
        public void EnterCredentials(string userName, string password)
        {
            this.UserNameInput.SendKeys(userName);
            this.PasswordInput.SendKeys(password);
        }

        /// <summary>
        /// Enter the use credentials and log in - Navigation sample
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <param name="password">The user password</param>
        /// <returns>The home page</returns>
        public HomePageModel LoginWithValidCredentials(string userName, string password)
        {
            this.EnterCredentials(userName, password);
            this.LoginButton.Click();

            return new HomePageModel(this.testObject);
        }

        /// <summary>
        /// Enter the use credentials and try to log in - Verify login failed
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <param name="password">The user password</param>
        /// <returns>True if the error message is displayed</returns>
        public bool LoginWithInvalidCredentials(string userName, string password)
        {
            this.EnterCredentials(userName, password);
            this.LoginButton.Click();
            return this.ErrorMessage.Displayed;
        }

        /// <summary>
        /// Assert the login page loaded
        /// </summary>
        public void AssertPageLoaded()
        {
            Assert.IsTrue(
                this.IsPageLoaded(),
                "The web page '{0}' is not loaded",
                PageUrl);
        }

        /// <summary>
        /// Check if the home page has been loaded
        /// </summary>
        /// <returns>True if the page was loaded</returns>
        public override bool IsPageLoaded()
        {
            return this.UserNameInput.Displayed && this.PasswordInput.Displayed && this.LoginButton.Displayed;
        }
    }
}

