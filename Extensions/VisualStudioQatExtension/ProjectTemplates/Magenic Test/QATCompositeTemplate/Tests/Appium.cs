using Magenic.Maqs.BaseAppiumTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Appium;

namespace $safeprojectname$
{
    /// <summary>
    /// Sample test class
    /// </summary>
    [TestClass]
    public class $safeitemname$ : BaseAppiumTest
    {
        /// <summary>
        /// Valid login page test
        /// </summary>
        // [TestMethod] - Disabled because this step will fail as the template does not include access to a test mobile device/application
        public void OpenLoginPage()
        {
            ApplicationLogin page = new ApplicationLogin(this.TestObject);
        }
    }
}
