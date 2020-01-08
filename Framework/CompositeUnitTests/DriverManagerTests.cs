//--------------------------------------------------
// <copyright file="DriverManagerTests.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Low level framework tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace CompositeUnitTests
{
    /// <summary>
    /// Framework unit test class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DriverManagerTests
    {
        /// <summary>
        ///  Can we add a manager by type
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void AddManagerByType()
        {
            ManagerDictionary dictionary = GetDictionary();
            dictionary.Add(GetManager());

            Assert.IsTrue(dictionary.ContainsKey(typeof(WebServiceDriverManager).FullName));
        }

        /// <summary>
        /// Does adding item increment count
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void AddIncrementCount()
        {
            ManagerDictionary dictionary = GetDictionary();
            dictionary.Add(GetManager());

            Assert.AreEqual(1, dictionary.Count);
        }

        /// <summary>
        ///  Is empty count zero
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void EmptyCountZero()
        {
            ManagerDictionary dictionary = GetDictionary();

            Assert.AreEqual(0, dictionary.Count);
        }

        /// <summary>
        ///  Does clear remove all item
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void ClearRemovesAll()
        {
            ManagerDictionary dictionary = GetDictionary();
            dictionary.Add(GetManager());
            dictionary.Add(string.Empty, GetManager());
            dictionary.Clear();

            Assert.AreEqual(0, dictionary.Count);
        }

        /// <summary>
        ///  Throw exception if we try add on top of an existing manager
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        [ExpectedException(typeof(ArgumentException))]

        public void ThrowDriverAlreadyExist()
        {
            ManagerDictionary dictionary = GetDictionary();
            dictionary.Add(GetManager());
            dictionary.Add(GetManager());

            Assert.Fail("Previous line should have failed the test.");
        }

        /// <summary>
        ///  Throw exception if we try add a named manager on top of an existing manager
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        [ExpectedException(typeof(ArgumentException))]

        public void ThrowNamedDriverAlreadyExist()
        {
            ManagerDictionary dictionary = GetDictionary();
            dictionary.Add(string.Empty, GetManager());
            dictionary.Add(string.Empty, GetManager());

            Assert.Fail("Previous line should have failed the test.");
        }

        /// <summary>
        ///  Can override existing
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void CanOverrideExisting()
        {
            ManagerDictionary dictionary = GetDictionary();
            dictionary.Add(GetManager());
            dictionary.AddOrOverride(GetManager());

            Assert.AreEqual(1, dictionary.Count);
        }

        /// <summary>
        ///  Can use override for new manager
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void CanOverrideNonExisting()
        {
            ManagerDictionary dictionary = GetDictionary();
            dictionary.AddOrOverride(GetManager());

            Assert.AreEqual(1, dictionary.Count);
        }

        /// <summary>
        ///  Can add named and unnamed
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void AddNamedAndUnnamed()
        {
            ManagerDictionary dictionary = GetDictionary();
            dictionary.Add(string.Empty, GetManager());
            dictionary.Add(GetManager());

            Assert.AreEqual(2, dictionary.Count);
        }

        /// <summary>
        ///  Remove by type
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void RemoveByType()
        {
            ManagerDictionary dictionary = GetDictionary();
            DriverManager managerToKeep = GetManager();

            dictionary.Add(GetManager());
            dictionary.Add(string.Empty, managerToKeep);
            dictionary.Remove(typeof(WebServiceDriverManager));

            Assert.AreEqual(managerToKeep, dictionary[string.Empty]);
        }

        /// <summary>
        ///  Remove by name
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void RemoveByName()
        {
            ManagerDictionary dictionary = GetDictionary();
            DriverManager managerToKeep = GetManager();

            dictionary.Add(managerToKeep);
            dictionary.Add(string.Empty, GetManager());
            dictionary.Remove(string.Empty);

            Assert.AreEqual(((WebServiceDriverManager)managerToKeep).Get(), dictionary.GetDriver<EventFiringWebServiceDriver, WebServiceDriverManager>());
        }

        /// <summary>
        ///  Managers map correctly
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void ManagersMap()
        {
            ManagerDictionary dictionary = GetDictionary();
            DriverManager managerToKeep = GetManager();
            DriverManager managerToKeep2 = GetManager();

            dictionary.Add(managerToKeep);
            dictionary.Add(string.Empty, managerToKeep2);

            Assert.AreEqual(((WebServiceDriverManager)managerToKeep).Get(), dictionary.GetDriver<EventFiringWebServiceDriver, WebServiceDriverManager>());
            Assert.AreEqual(managerToKeep2, dictionary[string.Empty]);
        }

        /// <summary>
        ///  Manager dispose
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void ManagerDispose()
        {
            ManagerDictionary manager = new ManagerDictionary();
            manager.Dispose();
            Assert.IsNotNull(manager);
        }

        /// <summary>
        /// Get a manager dictionary 
        /// </summary>
        /// <returns>The manager dictionary</returns>
        private static ManagerDictionary GetDictionary()
        {
            BaseTestObject baseTestObject = new BaseTestObject(new ConsoleLogger(), string.Empty);
            return baseTestObject.ManagerStore;
        }

        /// <summary>
        /// Get a driver manager
        /// </summary>
        /// <returns>The driver manager</returns>
        private static DriverManager GetManager()
        {
            BaseTestObject baseTestObject = new BaseTestObject(new ConsoleLogger(), string.Empty);
            return new WebServiceDriverManager(() => HttpClientFactory.GetDefaultClient(), baseTestObject);
        }
    }
}
