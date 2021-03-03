﻿//--------------------------------------------------
// <copyright file="WebServiceDriverNUnitConfig.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Configuration override tests for NUnit</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Configuration nunit override via properties tests
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class WebServiceDriverNUnitConfig : BaseWebServiceTest
    {
        /// <summary>
        /// Setup before running tests
        /// </summary>
        [OneTimeSetUp]
        public static void CheckBeforeClass()
        {
            // Set overrides
            MethodInfo dynMethod = NUnit.Framework.TestContext.Parameters.GetType().GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance);
            dynMethod.Invoke(NUnit.Framework.TestContext.Parameters, new object[] { "OverrideNUnitTest", "Value" });
            dynMethod.Invoke(NUnit.Framework.TestContext.Parameters, new object[] { "OverrideNUnitTestNew", "Value2" });

            Config.UpdateWithNUnitTestContext(NUnit.Framework.TestContext.Parameters);
        }

        /// <summary>
        /// If the property override was for an a value not in existing configuration file does the override work
        /// </summary>
        [Test]
        [Category(TestCategories.Utilities)]
        public void CheckIfOverrideNewWorks()
        {
            // Make sure the new key is not present
            Assert.AreEqual("Value2", Config.GetGeneralValue("OverrideNUnitTestNew"));
        }

        /// <summary>
        /// If the property override was for an existing configuration does the override work
        /// </summary>
        [Test]
        [Category(TestCategories.Utilities)]
        public void CheckIfOverrideExistingWorks()
        {
            // Make sure the new key is not present
            Assert.AreEqual("Value", Config.GetGeneralValue("OverrideNUnitTest"));
        }
    }
}
