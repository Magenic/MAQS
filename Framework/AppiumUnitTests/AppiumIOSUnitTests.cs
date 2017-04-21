using Magenic.MaqsFramework.BaseAppiumTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium.iOS;
using Magenic.MaqsFramework.Utilities.Helper;
using OpenQA.Selenium.Appium.Enums;

namespace AppiumUnitTests
{
    [TestClass()]
    public class AppiumIOSUnitTests: BaseAppiumTest
    {
        protected override AppiumDriver<AppiumWebElement> GetMobileDevice()
        {
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability(CapabilityType.Platform, "MAC");
            capabilities.SetCapability(CapabilityType.Version, "10.3");
            capabilities.SetCapability("deviceName", "iPhone 7");
            capabilities.SetCapability("bundleId", "com.teamtreehouse.Diary");
            capabilities.SetCapability("automationName", "XCUITest");
            capabilities.SetCapability(MobileCapabilityType.Udid, "0C0E26E7-966B-4C89-A765-32C5C997A456");
            return new IOSDriver<AppiumWebElement>(AppiumConfig.GetMobileHubUrl(), capabilities);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Appium)]
        public void AppiumIOSDriverTest()
        {
            Assert.IsNotNull(this.TestObject.AppiumDriver);
            this.TestObject.AppiumDriver.CloseApp();
            this.TestObject.AppiumDriver.Quit();
            this.TestObject.AppiumDriver.Dispose();
        }

    }
}
