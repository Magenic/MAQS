using Microsoft.VisualStudio.TestTools.UnitTesting;
using Magenic.MaqsFramework.BaseAppiumTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magenic.MaqsFramework.Utilities.Helper;

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