//--------------------------------------------------
// <copyright file="DriverManagerTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
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
            IManagerStore managerStore = GetManagerStore();
            managerStore.Add(GetManager());

            Assert.IsTrue(managerStore.Contains<WebServiceDriverManager>());
        }

        /// <summary>
        /// Does adding item increment count
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void AddIncrementCount()
        {
            IManagerStore dictionary = GetManagerStore();
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
            IManagerStore dictionary = GetManagerStore();

            Assert.AreEqual(0, dictionary.Count);
        }

        /// <summary>
        ///  Does clear remove all item
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void ClearRemovesAll()
        {
            IManagerStore dictionary = GetManagerStore();
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
            IManagerStore dictionary = GetManagerStore();
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
            IManagerStore dictionary = GetManagerStore();
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
            IManagerStore dictionary = GetManagerStore();
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
            IManagerStore dictionary = GetManagerStore();
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
            IManagerStore dictionary = GetManagerStore();
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
            IManagerStore managerStore = GetManagerStore();
            DriverManager managerToKeep = GetManager();

            managerStore.Add(GetManager());
            managerStore.Add(string.Empty, managerToKeep);
            managerStore.Remove<WebServiceDriverManager>();

            Assert.AreEqual(managerToKeep, managerStore.GetManager(string.Empty));
        }

        /// <summary>
        ///  Remove by name
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void RemoveByName()
        {
            IManagerStore dictionary = GetManagerStore();
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
            IManagerStore managerStore = GetManagerStore();
            DriverManager managerToKeep = GetManager();
            DriverManager managerToKeep2 = GetManager();

            managerStore.Add(managerToKeep);
            managerStore.Add(string.Empty, managerToKeep2);

            Assert.AreEqual(((WebServiceDriverManager)managerToKeep).Get(), managerStore.GetDriver<EventFiringWebServiceDriver, WebServiceDriverManager>());
            Assert.AreEqual(managerToKeep2, managerStore.GetManager(string.Empty));
        }

        /// <summary>
        ///  Manager dispose
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void ManagerDispose()
        {
            IManagerStore manager = new ManagerStore();
            manager.Dispose();
            Assert.IsNotNull(manager);
        }

        /// <summary>
        /// Tries to get a specific driver based on name
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void TryGetDriver()
        {
            IManagerStore managerStore = new ManagerStore();
            DriverManager managerToKeep = GetManager();
            managerStore.Add("TestDriver", managerToKeep);
            var result = managerStore.TryGetDriver<WebServiceDriver>("TestDriver", out var driver);
            Assert.IsTrue(result, "Could not find driver");
            Assert.IsNotNull(driver, "Driver was null");
        }

        /// <summary>
        /// Tries to get a driver that is expected not to exist
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void TryGetDriverNotFound()
        {
            IManagerStore managerStore = new ManagerStore();
            DriverManager managerToKeep = GetManager();
            managerStore.Add("TestDriver", managerToKeep);
            var result = managerStore.TryGetDriver<WebServiceDriver>("NotFound", out var driver);
            Assert.IsFalse(result, "Should not find driver");
            Assert.IsNull(driver, "Driver was not null");
        }

        /// <summary>
        /// Tries to get a specific driver manager based on name
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void TryGetManager()
        {
            IManagerStore managerStore = new ManagerStore();
            DriverManager managerToKeep = GetManager();
            managerStore.Add("TestDriver", managerToKeep);
            var result = managerStore.TryGetManager<WebServiceDriverManager>("TestDriver", out var driverManager);
            Assert.IsTrue(result, "Could not find driver");
            Assert.IsNotNull(driverManager, "Driver was null");
        }

        /// <summary>
        /// Tries to get a driver manager that is expected not to exist
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void TryGetManagerNotFound()
        {
            IManagerStore managerStore = new ManagerStore();
            DriverManager managerToKeep = GetManager();
            managerStore.Add("TestDriver", managerToKeep);
            var result = managerStore.TryGetManager<WebServiceDriverManager>("NotFound", out var driverManager);
            Assert.IsFalse(result, "Should not find driver");
            Assert.IsNull(driverManager, "Driver was not null");
        }

        /// <summary>
        /// Get a manager store
        /// </summary>
        /// <returns>The manager dictionary</returns>
        private static IManagerStore GetManagerStore()
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
