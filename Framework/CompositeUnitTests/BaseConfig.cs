//--------------------------------------------------
// <copyright file="BaseConfig.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Base configuration unit tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompositeUnitTests
{
    /// <summary>
    /// Simple base test
    /// </summary>
    [TestClass]
    public class BaseConfig : BaseTest
    {
        /// <summary>
        /// Can a basic test run
        /// </summary>
        [TestMethod]
        public void CanRunTest()
        {
            Assert.IsNotNull(this.TestObject);
        }

        /// <summary>
        /// Are run settings overrides respected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SectionOverrides()
        {
            Assert.AreEqual("SAMPLEGen", Config.GetValueForSection("MagenicMaqs", "SectionOverride"));
            Assert.AreEqual("SAMPLEApp", Config.GetValueForSection("AppiumMaqs", "SectionOverride"));
            Assert.AreEqual("SAMPLEAppCap", Config.GetValueForSection("AppiumCapsMaqs", "SectionOverride"));
            Assert.AreEqual("SAMPLEDatabase", Config.GetValueForSection("DatabaseMaqs", "SectionOverride"));
            Assert.AreEqual("SAMPLEEmail", Config.GetValueForSection("EmailMaqs", "SectionOverride"));
            Assert.AreEqual("SAMPLESel", Config.GetValueForSection("SeleniumMaqs", "SectionOverride"));
            Assert.AreEqual("SAMPLESelCap", Config.GetValueForSection("RemoteSeleniumCapsMaqs", "SectionOverride"));
            Assert.AreEqual("SAMPLEWeb", Config.GetValueForSection("WebServiceMaqs", "SectionOverride"));
        }

        /// <summary>
        /// Are run settings adds respected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SectionAdds()
        {
            Assert.AreEqual("SAMPLEGenz", Config.GetValueForSection(ConfigSection.MagenicMaqs, "SectionAdd"));
            Assert.AreEqual("SAMPLEGen", Config.GetGeneralValue("SectionOverride"));
            Assert.AreEqual("SAMPLEAppz", Config.GetValueForSection(ConfigSection.AppiumMaqs, "SectionAdd"));
            Assert.AreEqual("SAMPLEAppCapz", Config.GetValueForSection(ConfigSection.AppiumCapsMaqs, "SectionAdd"));
            Assert.AreEqual("SAMPLEDatabasez", Config.GetValueForSection(ConfigSection.DatabaseMaqs, "SectionAdd"));
            Assert.AreEqual("SAMPLEEmailz", Config.GetValueForSection(ConfigSection.EmailMaqs, "SectionAdd"));
            Assert.AreEqual("SAMPLESelz", Config.GetValueForSection(ConfigSection.SeleniumMaqs, "SectionAdd"));
            Assert.AreEqual("SAMPLESelCapz", Config.GetValueForSection(ConfigSection.RemoteSeleniumCapsMaqs, "SectionAdd"));
            Assert.AreEqual("SAMPLEWebz", Config.GetValueForSection(ConfigSection.WebServiceMaqs, "SectionAdd"));
        }

        /// <summary>
        /// Can we override
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void GeneralSectionOverride()
        {
            Assert.AreEqual("SAMPLEGen", Config.GetGeneralValue("SectionOverride"));
        }

        /// <summary>
        /// Can we add 
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void GeneralSectionAdd()
        {
            Assert.AreEqual("SAMPLEGenz", Config.GetGeneralValue("SectionAdd"));
        }
    }
}
