using System;
using Magenic.Maqs.BaseAppiumTest;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Android;
using PageModel;

namespace Tests
{
    /// <summary>
    /// AppiumTestsNUnit test class with NUnit
    /// </summary>
    [TestFixture]
    public class AppiumTestsNUnit : BaseAppiumTest
    {
         /// <summary>
        /// The starting page
        /// </summary>
        private ALoginPageModel startingPage;

        /// <summary>
        /// Sets up the starting page to be iOS or Android
        /// </summary>
        [SetUp]
        public void SetupStartingPage()
        {
            Type driverType = this.TestObject.AppiumDriver.GetType();
            if (driverType == typeof(IOSDriver<AppiumWebElement>))
            {
                startingPage = new IOSLoginPageModel(this.TestObject);
            }
            else if (driverType == typeof(AndroidDriver<AppiumWebElement>))
            {
                startingPage = new AndroidLoginPageModel(this.TestObject);
            }
            else
            {
                throw new NotSupportedException($"This OS type: {driverType.ToString()} is not supported.");
            }
        }

        /// <summary>
        /// Verifies the error message is as expected when a user logs in with invalid creds
        /// </summary>
        [Test]
        public void InvalidLoginTest()
        {
            string expectedError = "Use the following credentials: \r\n(User Name: Ted Password: 123)";
            startingPage.LoginWithInvalidCredentials("Not", "Valid");
            string actualError = startingPage.GetErrorMessage();
            Assert.AreEqual(expectedError, startingPage.GetErrorMessage());
        }

		/// <summary>
        /// Verifies a user can login with valid creds
        /// </summary>
        [Test]
        public void ValidLoginTest()
        {
            string username = "Ted";
            string password = "123";
            string expectedGreeting = $"Welcome {username}!";
            string expectedTimeDescription = "The current time is:";
            AHomePageModel homePage = startingPage.LoginWithValidCredentials(username, password);
            SoftAssert.AreEqual(expectedGreeting, homePage.GetGreetingMessage());
            SoftAssert.AreEqual(expectedTimeDescription, homePage.GetTimeDiscription());
            DateTime time;
            SoftAssert.IsTrue(DateTime.TryParse(homePage.GetTime(), out time), "Time Parsing");
            SoftAssert.FailTestIfAssertFailed();
        }
    }
}
