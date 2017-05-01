using Magenic.MaqsFramework.BaseDatabaseTest;
using Magenic.MaqsFramework.BaseSeleniumTest;
using Magenic.MaqsFramework.BaseWebServiceTest;
using NUnit.Framework;
using Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace $safeprojectname$
{
    /// <summary>
    /// Composite Selenium test class
    /// </summary>
    [TestFixture]
    public class $safeitemname$ : BaseSeleniumTest
    {
        /// <summary>
        /// Do database setup for test run
        /// </summary>
        // [Setup] - Disabled because this step will fail as the template does not include access to a test database
        public static void TestSetup(TestContext context)
        {
            // Do database setup
            using (DatabaseConnectionWrapper wrapper = new DatabaseConnectionWrapper(DatabaseConfig.GetConnectionString()))
            {
                SqlParameter state = new SqlParameter("StateAbbreviation", "MN");
                DataTable table = wrapper.RunQueryProcedure("getStateAbbrevMatch", state);
                Assert.AreEqual(1, table.Rows.Count, "Expected 1 state abbreviation to be returned.");
            }
        }

        /// <summary>
        /// Do post test run web service cleanup
        /// </summary>
        [TearDown]
        public static void TestCleanup()
        {
            // Do web service post run cleanup
            HttpClientWrapper client = new HttpClientWrapper(new Uri(WebServiceConfig.GetWebServiceUri()));
            string result = client.Delete("/api/String/Delete/1", "text/plain", true);
            Assert.AreEqual(string.Empty, result);
        }

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
