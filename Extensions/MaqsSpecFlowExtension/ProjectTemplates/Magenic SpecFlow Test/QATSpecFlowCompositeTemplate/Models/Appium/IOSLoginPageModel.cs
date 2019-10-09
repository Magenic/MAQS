﻿using Magenic.Maqs.BaseAppiumTest;
using OpenQA.Selenium;
namespace Models.Appium
{
    /// <summary>
    /// Page object for the IOSLoginPageModel inheriting from the ALoginPageModel
    /// </summary>
    public class IOSLoginPageModel : ALoginPageModel
    {
        /// <summary>
        /// The user name input element 'By' finder
        /// </summary>
        protected override LazyMobileElement UserNameInput
        {
            get { return new LazyMobileElement(this.TestObject, By.Id("UserName"), "User Name Field"); }
        }

        /// <summary>
        /// The password input element 'By' finder
        /// </summary>
        protected override LazyMobileElement PasswordInput
        {
            get { return new LazyMobileElement(this.TestObject, By.Id("Password"), "Password Field"); }
        }

        /// <summary>
        /// The login button element 'By' finder
        /// </summary>
        protected override LazyMobileElement LoginButton
        {
            get { return new LazyMobileElement(this.TestObject, By.Id("Login"), "Login Button"); }
        }

        /// <summary>
        /// The error message element 'By' finder
        /// </summary>
        protected override LazyMobileElement ErrorMessage
        {
            get { return new LazyMobileElement(this.TestObject, By.Id("message"), "Error Message Label"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IOSLoginPageModel" /> class.
        /// </summary>
        /// <param name="testObject">The appium test object</param>
        public IOSLoginPageModel(AppiumTestObject testObject)
        {
            this.TestObject = testObject;
        }

        /// <summary>
        /// Get the iOS version of the HomePageModel
        /// </summary>
        /// <returns>The iOS version of the HomePageModel</returns>
        protected override AHomePageModel GetHomePageModel()
        {
            return new IOSHomePageModel(TestObject);
        }
    }
}