using Magenic.MaqsFramework.BaseAppiumTest;
using NUnit.Framework;
using PageModel;

namespace $safeprojectname$
{
    /// <summary>
    /// Simple appium test class
    /// </summary>
    [TestFixture]
public class $safeitemname$ : BaseAppiumTest
{
    /// <summary>
    /// Application installed test
    /// </summary>
    [Test]
    public void ApplicationInstalledTest()
    {
        bool isInstalled = this.AppiumDriver.IsAppInstalled(AppiumConfig.GetBundleId());
    }

    /// <summary>
    /// Enter credentials test
    /// </summary>
    [Test]
    public void InvalidLoginTest()
    {
        LoginPageModel page = new LoginPageModel(this.TestObject);
        page.LoginWithInvalidCredentials("Not", "Valid");
        string errorMessage = page.GetErrorMessage();
    }

	/// <summary>
    /// Valid login test
    /// </summary>
    [Test]
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
