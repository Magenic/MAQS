using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;

namespace SeleniumUnitTests
{
    public class SauceLabsBaseSeleniumTest : BaseSeleniumTest
    {
        private static readonly string BuildDate = DateTime.Now.ToString("MMddyyyy hhmmss");

        protected override IWebDriver GetBrowser()
        {
            if(string.Equals(Config.GetValueForSection(ConfigSection.RemoteSeleniumCapsMaqs, "RunOnSauceLabs"), "YES", System.StringComparison.OrdinalIgnoreCase))
            {
                var sauceOptions = new Dictionary<string, object>();
                sauceOptions.Add("screenResolution", "1280x1024");

                var browserOptions = new ChromeOptions();
                browserOptions.UseSpecCompliantProtocol = true;
                browserOptions.PlatformName = "Windows 10";
                browserOptions.BrowserVersion = "latest";
                browserOptions.AddAdditionalCapability("sauce:options", sauceOptions, true);

                var remoteCapabilitySection = Config.GetSection(ConfigSection.RemoteSeleniumCapsMaqs);

                if (remoteCapabilitySection != null)
                {
                    foreach (var keyValue in remoteCapabilitySection)
                    {
                        if (remoteCapabilitySection[keyValue.Key].Length > 0)
                        {
                            sauceOptions.Add(keyValue.Key, keyValue.Value);
                        }
                    }
                }

                var remoteCapabilities = browserOptions.ToCapabilities();

                var driver = new RemoteWebDriver(new Uri(Config.GetValueForSection(ConfigSection.SeleniumMaqs, "HubUrl")), remoteCapabilities, SeleniumConfig.GetCommandTimeout());

                //Potential to move this back into the test initialize.  The test SeleniumSoftAssertIsFalseTrueConditionPageSourceNoBrowser does not work if it is in the TestInitialize
                try
                {
                    var name = this.TestContext.FullyQualifiedTestClassName + "." + this.TestContext.TestName;
                    ((IJavaScriptExecutor)driver).ExecuteScript("sauce:job-build=" + BuildDate);
                    ((IJavaScriptExecutor)driver).ExecuteScript("sauce:job-name=" + name);

                }
                catch (Exception e)
                {
                    this.Log.LogMessage(Magenic.Maqs.Utilities.Logging.MessageType.WARNING, "Failed to set Sauce Test or build name because: " + e.Message);
                }

                return driver;
            }

            return base.GetBrowser();
        }

        [TestInitialize]
        public void TestInit()
        {
            if (SeleniumConfig.GetBrowserName().Equals("REMOTE", System.StringComparison.OrdinalIgnoreCase))
            {
                // The test SeleniumSoftAssertIsFalseTrueConditionPageSourceNoBrowser does not work if it is in the TestInitialize
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            var passed = this.GetResultType() == Magenic.Maqs.Utilities.Logging.TestResultType.PASS;

            if (string.Equals(Config.GetValueForSection(ConfigSection.RemoteSeleniumCapsMaqs, "RunOnSauceLabs"), "YES", System.StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    ((IJavaScriptExecutor)this.WebDriver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
                }
                catch(Exception e)
                {
                    this.Log.LogMessage(Magenic.Maqs.Utilities.Logging.MessageType.WARNING, "Failed to set Sauce Result because: " + e.Message);
                }
            }
            base.Teardown();
        }
    }
}
