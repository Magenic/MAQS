using Magenic.MaqsFramework.BaseAppiumTest;
using NUnit.Framework;
using Models.Appium;

namespace $safeprojectname$
{
    /// <summary>
    /// Sample Appium test class
    /// </summary>
    [TestFixture]
    public class $safeitemname$ : BaseAppiumTest
    {
        /// <summary>
        /// Valid login page test
        /// </summary>
        // [Test] - Disabled because this step will fail as the template does not include access to a test mobile device/application
        public void OpenLoginPage()
        {
            ApplicationLogin page = new ApplicationLogin(this.TestObject);
        }
    }
}
