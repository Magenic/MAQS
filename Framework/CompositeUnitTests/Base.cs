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
