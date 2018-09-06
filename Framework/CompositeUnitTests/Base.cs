//--------------------------------------------------
// <copyright file="Base.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
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
    public class Base : BaseTest
    {
        /// <summary>
        /// Make sure section get old, new and override values
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void ConfigSections()
        {
            var keysAndValues = Config.GetSection("MagenicMaqS");
            SoftAssert.Assert(() => Assert.AreEqual(9, keysAndValues.Count, "Expect 9 values, 6 from app.config plus 3 from run settings file"));
            SoftAssert.Assert(() => Assert.AreEqual("TXT", keysAndValues["LogType"], "Base configuration not repected"));
            SoftAssert.Assert(() => Assert.AreEqual("SAMPLEGen", keysAndValues["SectionOverride"], "Override not respected"));
            SoftAssert.Assert(() => Assert.AreEqual("SAMPLEGenz", keysAndValues["SectionAdd"], "Run settings addition not repected"));
        }

        /// <summary>
        /// Make we handle missing sections gracefully
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void EmptyConfigSections()
        {
            var keysAndValues = Config.GetSection("MagenicMaqSZZZ");
            SoftAssert.Assert(() => Assert.AreEqual(0, keysAndValues.Count, "Expected no matching configuration key value pairs."));
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
