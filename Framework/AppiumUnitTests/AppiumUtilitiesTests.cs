using Magenic.MaqsFramework.BaseAppiumTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppiumUnitTests
{
    [TestClass()]
    public class AppiumUtilitiesTests : BaseAppiumTest
    {
        [TestMethod()]
        [TestCategory(TestCategories.Appium)]
        public void CaptureScreenshotTest()
        {
            Assert.IsTrue(AppiumUtilities.CaptureScreenshot(this.TestObject.AppiumDriver, this.TestObject.Log));
        }
    }
}