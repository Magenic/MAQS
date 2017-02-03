//--------------------------------------------------
// <copyright file="WebServiceWithWrapper.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Web service general unit tests</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseWebServiceTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test web service wrapper testing
    /// </summary>
    [TestClass]
    public class WebServiceWithWrapper : BaseWebServiceTest
    {
        /// <summary>
        /// Did the logging folder exist at the start of the test run
        /// </summary>
        private static bool loggingFolderExistsBeforeRun = false;

        /// <summary>
        /// Setup before we start running selenium tests
        /// </summary>
        /// <param name="context">The upcoming test context</param>
        [ClassInitialize]
        public static void CheckBeforeClass(TestContext context)
        {
            loggingFolderExistsBeforeRun = TestHelper.DoesFolderExist();
        }

        /// <summary>
        /// Cleanup after we are done running selenium tests
        /// </summary>
        [ClassCleanup]
        public static void CleanupAfterClass()
        {
            TestHelper.Cleanup(loggingFolderExistsBeforeRun);
        }

        /// <summary>
        /// Make sure the test objects map properly
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [TestCategory(TestCategories.Utilities)]
        public void WebServiceTestObjectMapCorrectly()
        {
            Assert.AreEqual(this.TestObject.Log, this.Log, "Logs don't match");
            Assert.AreEqual(this.TestObject.SoftAssert, this.SoftAssert, "Soft asserts don't match");
            Assert.AreEqual(this.TestObject.PerfTimerCollection, this.PerfTimerCollection, "Soft asserts don't match");
            Assert.AreEqual(this.TestObject.WebServiceWrapper, this.WebServiceWrapper, "Web service wrapper don't match");
        }

        /// <summary>
        /// Make sure test object values are saved as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [TestCategory(TestCategories.Utilities)]
        public void WebServiceTestObjectValuesCanBeUsed()
        {
            this.TestObject.SetValue("1", "one");

            Assert.AreEqual(this.TestObject.Values["1"], "one");
            string outValue;
            Assert.IsFalse(this.TestObject.Values.TryGetValue("2", out outValue), "Didn't expect to get value for key '2', but got " + outValue);
        }

        /// <summary>
        /// Make sure the test object objects are saved as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [TestCategory(TestCategories.Utilities)]
        public void WebServiceTestObjectObjectssCanBeUsed()
        {
            StringBuilder builder = new StringBuilder();
            this.TestObject.SetObject("1", builder);

            Assert.AreEqual(this.TestObject.Objects["1"], builder);

            object outObject;
            Assert.IsFalse(this.TestObject.Objects.TryGetValue("2", out outObject), "Didn't expect to get value for key '2'");

            builder.Append("123");

            Assert.AreEqual(((StringBuilder)this.TestObject.Objects["1"]).ToString(), builder.ToString());
        }
    }
}
