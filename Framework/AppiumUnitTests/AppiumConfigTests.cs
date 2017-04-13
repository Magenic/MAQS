using Magenic.MaqsFramework.BaseAppiumTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;

namespace AppiumUnitTests
{
    [TestClass()]
    public class AppiumConfigTests
    {
        [TestMethod()]
        [TestCategory(TestCategories.Appium)]
        public void GetMobileDeviceOSTest()
        {
            Assert.AreEqual(AppiumConfig.GetMobileDeviceOS(), "Android");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Appium)]
        public void GetMobileDeviceUDIDTest()
        {
            Assert.AreEqual(AppiumConfig.GetMobileDeviceUDID(), "0123456789abcdef0123456789abcdef01234567");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Appium)]
        public void GetBundleIdTest()
        {
            Assert.AreEqual(AppiumConfig.GetBundleId(), "org.tasks");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Appium)]
        public void GetOSVersionTest()
        {
            Assert.AreEqual(AppiumConfig.GetOSVersion(), "7.1.1");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Appium)]
        public void GetDeviceNameTest()
        {
            Assert.AreEqual(AppiumConfig.GetDeviceName(), "Nexus 6P");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Appium)]
        public void UsingMobileBrowserTest()
        {
            Assert.AreEqual(AppiumConfig.UsingMobileBrowser(), false);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Appium)]
        public void MobileDeviceTest()
        {
            #region MobileDevice
             AppiumDriver<AppiumWebElement> driver = AppiumConfig.MobileDevice();
            #endregion

            Assert.IsNotNull(driver);
            driver.CloseApp();
            driver.Quit();
            driver.Dispose();
        }

        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetMobileHubUrlTest()
        {
            Assert.AreEqual(AppiumConfig.GetMobileHubUrl(), "http://qat-win81-pc:4444/wd/hub");
        }

        //[TestMethod()]
        //public void MobileDeviceTest1()
        //{
        //    //Assert.Fail();
        //}

        [TestMethod()]
        [TestCategory(TestCategories.Appium)]
        public void GetWaitDriverTest()
        {
            AppiumDriver<AppiumWebElement> driver = AppiumConfig.MobileDevice();
            WebDriverWait wait = AppiumConfig.GetWaitDriver(driver);

            Assert.IsNotNull(wait);
            driver.CloseApp();
            driver.Quit();
            driver.Dispose();
        }

        //[TestMethod()]
        //[TestCategory(TestCategories.Appium)]
        //public void SetTimeoutsTest()
        //{    
        //    AppiumConfig.SetTimeouts(AppiumConfig.MobileDevice());
        //    WebDriverWait wait = AppiumConfig.GetWaitDriver(AppiumConfig.MobileDevice());
        //    Assert.AreEqual(wait.Timeout.TotalMilliseconds.ToString(), Config.GetValue("Timeout", "0"));
            
        //}

        [TestMethod()]
        [TestCategory(TestCategories.Appium)]
        public void GetAvdNameTest()
        {
            Assert.AreEqual(AppiumConfig.GetAvdName(), "Nexus-6P");
        }
    }
}