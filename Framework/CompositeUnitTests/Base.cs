﻿//--------------------------------------------------
// <copyright file="Base.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Core Base unit tests</summary>
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
    [TestCategory(TestCategories.Utilities)]
    public class Base : BaseTest
    {
        /// <summary>
        /// Make sure section get old, new and override values
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void ConfigSections()
        {
            var keysAndValues = Config.GetSectionDictionary("MagenicMaqS");
            SoftAssert.Assert(() => Assert.AreEqual("TXT", keysAndValues["LogType"], "Base configuration not respected"), "2");
            SoftAssert.Assert(() => Assert.AreEqual("SAMPLEGen", keysAndValues["SectionOverride"], "Override not respected"), "3");
            SoftAssert.Assert(() => Assert.AreEqual("SAMPLEGenz", keysAndValues["SectionAdd"], "Run settings addition not respected"), "4");
        }

        /// <summary>
        /// Make we handle missing sections gracefully
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void EmptyConfigSections()
        {
            var keysAndValues = Config.GetSectionDictionary("MagenicMaqSZZZ");
            SoftAssert.Assert(() => Assert.AreEqual(0, keysAndValues.Count, "Expected no matching configuration key value pairs."), "1");
        }

        /// <summary>
        /// Can a basic test run
        /// </summary>
        [TestMethod]
        public void CanRunTest()
        {
            Assert.IsNotNull(this.TestObject);
        }

        /// <summary>
        /// Can we add to a section
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.UtilitiesCore)]
        public void TestRunSettingAddition()
        {
            Assert.AreEqual("SAMPLE", Config.GetValueForSection("SeleniumMaqs", "Adding"));
        }

        /// <summary>
        /// Can we override general
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.UtilitiesCore)]
        public void TestRunSettingOverrideGeneral()
        {
            Assert.AreEqual("YetAnother", Config.GetGeneralValue("Grog"));
            Config.AddGeneralTestSettingValues("Grog", "ZZZ");
            Assert.AreEqual("ZZZ", Config.GetGeneralValue("Grog"));
        }

        /// <summary>
        /// Can we override in a section
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.UtilitiesCore)]
        public void TestRunSettingOverrideSection()
        {
            Assert.AreEqual("SAMPLEGen", Config.GetValueForSection("Magenicmaqs", "SectionOverride"));
        }
    }
}
