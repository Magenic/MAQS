using System;
using Magenic.MaqsFramework.BaseAppiumTest;
using OpenQA.Selenium;
namespace $safeprojectname$
{
     /// <summary>
    /// Page object for the AndroidLoginPageModel inheriting from the ALoginPageModel
    /// </summary>
    public class AndroidLoginPageModel : ALoginPageModel
    {
        /// <summary>
        /// The user name input element 'By' finder
        /// </summary>
        protected override FluentMobileElement UserNameInput
        {
            get { return new FluentMobileElement(this.TestObject, By.Id("UserName"), "User Name Field"); }
        }

        /// <summary>
        /// The password input element 'By' finder
        /// </summary>
        protected override FluentMobileElement PasswordInput
        {
            get { return new FluentMobileElement(this.TestObject, By.Id("Password"), "Password Field"); }
        }

        /// <summary>
        /// The login button element 'By' finder
        /// </summary>
        protected override FluentMobileElement LoginButton
        {
            get { return new FluentMobileElement(this.TestObject, By.Id("Login"), "Login Button"); }
        }

        /// <summary>
        /// The error message element 'By' finder
        /// </summary>
        protected override FluentMobileElement ErrorMessage
        {
            get { return new FluentMobileElement(this.TestObject, By.Id("message"), "Error Message Label"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidLoginPageModel" /> class.
        /// </summary>
        /// <param name="testObject">The appium test object</param>
        public AndroidLoginPageModel(AppiumTestObject testObject)
        {
            this.TestObject = testObject;
        }

        /// <summary>
        /// Get the iOS version of the HomePageModel
        /// </summary>
        /// <returns>The iOS version of the HomePageModel</returns>
        protected override AHomePageModel GetHomePageModel()
        {
            return new AndroidHomePageModel(TestObject);
        }
    }
}