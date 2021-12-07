using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using WebServiceTesterUnitTesting.Model;

namespace WebServiceUnitTests
{
    [TestClass]
    [DoNotParallelize]
    public class WebServiceDriverVersionTests : BaseWebServiceTest
    {
        [TestMethod]
        public void Version20()
        {
            string httpClientVersion = Config.GetValueForSection(ConfigSection.WebServiceMaqs, "HttpClientVersion");

            Config.AddTestSettingValues(
                    new Dictionary<string, string>
                    {
                        { "HttpClientVersion", "2.0" }
                    },
                   "WebServiceMaqs");

            try
            {
                this.WebServiceDriver.Get<ArrayOfProduct>("/api/XML_JSON/GetAllProducts", "application/xml");


            }
            finally
            {
                Config.AddTestSettingValues(
                    new Dictionary<string, string>
                    {

                         { "HttpClientVersion", httpClientVersion }
                    },
                   "WebServiceMaqs");
            }
        }

        [TestMethod]
        public void NoVersion()
        {
            string httpClientVersion = Config.GetValueForSection(ConfigSection.WebServiceMaqs, "HttpClientVersion");

            Config.AddTestSettingValues(
                    new Dictionary<string, string>
                    {
                        { "HttpClientVersion", string.Empty }
                    },
                   "WebServiceMaqs");

            try
            {
                this.WebServiceDriver.Get<ArrayOfProduct>("/api/XML_JSON/GetAllProducts", "application/xml");


            }
            finally
            {
                Config.AddTestSettingValues(
                    new Dictionary<string, string>
                    {

                         { "HttpClientVersion", httpClientVersion }
                    },
                   "WebServiceMaqs");
            }
        }
    }
}
