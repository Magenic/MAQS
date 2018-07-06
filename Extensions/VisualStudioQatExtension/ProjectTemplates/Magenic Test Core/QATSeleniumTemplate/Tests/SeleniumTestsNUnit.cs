using Magenic.Maqs.BaseSeleniumTest;
using NUnit.Framework;
using PageModel;

namespace $safeprojectname$
{
    /// <summary>
    /// SeleniumTest test class with NUnit
    /// </summary>
    [TestFixture]
    public class $safeitemname$ : BaseSeleniumTest
    {
        /// <summary>
        /// Open page test
        /// </summary>
        [Test]
        public void OpenLoginPageTest()
        {
            LoginPageModel page = new LoginPageModel(this.TestObject);
            page.OpenLoginPage();
        }

        /// <summary>
        /// Enter credentials test
        /// </summary>
        [Test]
        public void EnterValidCredentialsTest()
        {
            string username = "Ted";
            string password = "123";
            LoginPageModel page = new LoginPageModel(this.TestObject);
            page.OpenLoginPage();
            HomePageModel homepage = page.LoginWithValidCredentials(username, password);
            Assert.IsTrue(homepage.IsPageLoaded());
        }

        /// <summary>
        /// Enter credentials test
        /// </summary>
        [Test]
        public void EnterInvalidCredentials()
        {
            string username = "NOT";
            string password = "Valid";
            LoginPageModel page = new LoginPageModel(this.TestObject);
            page.OpenLoginPage();
            Assert.IsTrue(page.LoginWithInvalidCredentials(username, password));
        }
    }
}
