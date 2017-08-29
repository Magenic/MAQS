using Magenic.MaqsFramework.BaseAppiumTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PageModel;

namespace $safeprojectname$
{
    /// <summary>
    /// $safeitemname$ test class
    /// </summary>
    [TestClass]
    public class $safeitemname$ : BaseAppiumTest
    {
        /// <summary>
        /// Application installed test
        /// </summary>
        [TestMethod]
        public void ApplicationInstalledTest()
        {
            bool isInstalled = this.AppiumDriver.IsAppInstalled(AppiumConfig.GetBundleId());
        }

        /// <summary>
        /// Enter credentials test
        /// </summary>
        [TestMethod]
        public void InvalidLoginTest()
        {
            LoginPageModel page = new LoginPageModel(this.TestObject);
            page.LoginWithInvalidCredentials("Not", "Valid");
            string errorMessage = page.GetErrorMessage();
        }

		/// <summary>
        /// Valid login test
        /// </summary>
        [TestMethod]
        public void ValidLoginTest()
        {
            string username = "Magenic";
            string password = "MAQS";
            LoginPageModel page = new LoginPageModel(this.TestObject);
            HomePageModel homePage = page.LoginWithValidCredentials(username, password);
            string loggedInPassword = homePage.GetLoggedInPassword();
            string loggedInUsername = homePage.GetLoggedInUsername();
        }
    }
}
