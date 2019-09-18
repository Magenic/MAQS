using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Test the WebDriverFactory class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebDriverFactoryUnitTests
    {
        /// <summary>
        /// Verify SetProxySettings sets the proxy in the config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [DoNotParallelize]
        public void SetProxySettings()
        {
            try
            {
                Dictionary<string, string> overrides = new Dictionary<string, string> { { "UseProxy", "Yes" } };
                Config.AddTestSettingValues(overrides, ConfigSection.SeleniumMaqs, true);

                ChromeOptions options = new ChromeOptions();
                options.SetProxySettings();

                Assert.IsNotNull(options.Proxy);
                Assert.AreEqual(options.Proxy.HttpProxy, "127.0.0.1:8080");
                Assert.AreEqual(options.Proxy.SslProxy, "127.0.0.1:8080");
            }
            finally
            {
                // Revert the config
                Dictionary<string, string> overrides = new Dictionary<string, string> { { "UseProxy", "No" } };
                Config.AddTestSettingValues(overrides, ConfigSection.SeleniumMaqs, true);
            }
        }
    }
}
