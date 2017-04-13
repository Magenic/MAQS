using Magenic.MaqsFramework.BaseAppiumTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppiumUnitTests
{
    [TestClass()]
    public class AppiumTestObjectTests: BaseAppiumTest
    {
        [TestMethod()]
        [TestCategory(TestCategories.Appium)]
        public void AppiumTestObjectTest()
        {
            Assert.IsNotNull(this.TestObject.AppiumDriver);
            this.TestObject.AppiumDriver.CloseApp();
            this.TestObject.AppiumDriver.Quit();
            this.TestObject.AppiumDriver.Dispose();
        }
    }
}