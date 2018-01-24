using Magenic.MaqsFramework.BaseAppiumTest;
using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
using OpenQA.Selenium;
namespace $safeprojectname$
{
    /// <summary>
    /// Page object for the ALoginPageModel
    /// </summary>
    public abstract class ALoginPageModel
    {
        /// <summary>
        /// Appium test object
        /// </summary>
        protected AppiumTestObject TestObject;

        /// <summary>
        /// The user name input element 'By' finder
        /// </summary>
        protected abstract FluentMobileElement UserNameInput { get; }

        /// <summary>
        /// The password input element 'By' finder
        /// </summary>
        protected abstract FluentMobileElement PasswordInput { get; }

        /// <summary>
        /// The login button element 'By' finder
        /// </summary>
        protected abstract FluentMobileElement LoginButton { get; }

        /// <summary>
        /// The error message element 'By' finder
        /// </summary>
        protected abstract FluentMobileElement ErrorMessage { get; }

        /// <summary>
        /// Enter the user credentials
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <param name="password">The user password</param>
        public void EnterCredentials(string userName, string password)
        {
            UserNameInput.SendKeys(userName);
            PasswordInput.SendKeys(password);
        }

        /// <summary>
        /// Enter the use credentials and try to log in - Verify login failed
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <param name="password">The user password</param>
        public void LoginWithInvalidCredentials(string userName, string password)
        {
            EnterCredentials(userName, password);
            LoginButton.Click();
        }

        /// <summary>
        /// Enter the use credentials and log in - Navigation sample
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <param name="password">The user password</param>
        /// <returns>Home Page Object Model</returns>
        public AHomePageModel LoginWithValidCredentials(string userName, string password)
        {
            EnterCredentials(userName, password);
            LoginButton.Click();
            return GetHomePageModel();
        }

        /// <summary>
        /// Get text from error message label
        /// </summary>
        /// <returns>Error Message text string</returns>
        public string GetErrorMessage()
        {
            return ErrorMessage.Text;
        }

        /// <summary>
        /// Get the iOS or Android version of the HomePageModel
        /// </summary>
        /// <returns>The iOS or Android version of the HomePageModel</returns>
        protected abstract AHomePageModel GetHomePageModel();
    }
}