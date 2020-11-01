using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SeleniumUnitTests
{
    [ExcludeFromCodeCoverage]
    public class SauceLabsBaseSeleniumTest : BaseSeleniumTest
    {
        private static readonly string BuildDate = DateTime.Now.ToString("MMddyyyy hhmmss");

        protected override IWebDriver GetBrowser()
        {
            if(string.Equals(Config.GetValueForSection(ConfigSection.RemoteSeleniumCapsMaqs, "RunOnSauceLabs"), "YES", System.StringComparison.OrdinalIgnoreCase))
            {
                var name = this.TestContext.FullyQualifiedTestClassName + "." + this.TestContext.TestName;
                var sauceOptions = new Dictionary<string, object>
                {
                    ["screenResolution"] = "1280x1024",
                    ["build"] = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SAUCE_BUILD_NAME")) ? BuildDate : Environment.GetEnvironmentVariable("SAUCE_BUILD_NAME"),
                    ["name"] = name
                };

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

                var browserOptions = new ChromeOptions
                {
                    UseSpecCompliantProtocol = true,
                    PlatformName = "Windows 10",
                    BrowserVersion = "latest"
                };
                browserOptions.AddAdditionalCapability("sauce:options", sauceOptions, true);

                var remoteCapabilities = browserOptions.ToCapabilities();

                return new RemoteWebDriver(new Uri(Config.GetValueForSection(ConfigSection.SeleniumMaqs, "HubUrl")), remoteCapabilities, SeleniumConfig.GetCommandTimeout());
            }

            return base.GetBrowser();
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
