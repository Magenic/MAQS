//--------------------------------------------------
// <copyright file="EventFiringDatabaseWrapperTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Database base Eventfiring wrapper test unit tests</summary>
//--------------------------------------------------

using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using Magenic.MaqsFramework.BaseDatabaseTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DatabaseUnitTests
{
    using System.Linq;

    using global::DatabaseUnitTests.Models;

    using Magenic.MaqsFramework.Utilities.Data;

    /// <summary>
    /// Unit tests for the event firing database wrapper.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EventFiringDatabaseWrapperTests : EventFiringDatabaseDriver
    {
        /// <summary>
        /// Database Event will modify this
        /// </summary>
        private string eventString = string.Empty;
        
        /// <summary>
        /// Test the execute code path
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void EventFiringExecuteTest()
        {
            var result = this.Execute("setStateAbbrevToSelf", new { StateAbbreviation = "MN" }, commandType: CommandType.StoredProcedure);
            Assert.AreEqual(1, result, "Expected 1 state abbreviation to be updated.");
        }

        /// <summary>
        /// Test the execute raise event by subscribing to the Database event
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void EventFiringExecuteEventTest()
        {
            // Subscribe to the event
            this.DatabaseEvent += this.DatabaseUnitTestEvent;

            this.Execute("setStateAbbrevToSelf", new { StateAbbreviation = "MN" }, commandType: CommandType.StoredProcedure);
            
            Assert.AreEqual("Performing execute with:\r\nsetStateAbbrevToSelf", this.eventString);
        }

        /// <summary>
        /// Test the execute throw exception code path
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExecuteThrowException()
        {
            this.Execute(null);
        }

        /// <summary>
        /// Test the execute raise event by subscribing to the Database event
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void EventFiringExecuteThrowExceptionEventTest()
        {
            try
            {
                // Subscribe to the event
                this.DatabaseErrorEvent += this.DatabaseUnitTestEvent;

                this.Execute(null);
            }
            catch (InvalidOperationException e)
            {
                var message = StringProcessor.SafeFormatter("Failed because: {0}", e.Message);
                Assert.IsTrue(this.eventString.Contains(message));
            }
        }

        /// <summary>
        /// Test the query list code path
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void EventFiringQueryListTest()
        {
            var states = this.Query("SELECT * FROM States").ToList();

            // Our database only has 49 states
            Assert.AreEqual(49, states.Count, "Expected 49 states.");
        }

        /// <summary>
        /// Test the query list raise event by subscribing to the Database event
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void EventFiringQueryListEventTest()
        {
            // Subscribe to the event
            this.DatabaseEvent += this.DatabaseUnitTestEvent;

            this.Query("SELECT * FROM States");
            
            Assert.AreEqual("Performing query with:\r\nSELECT * FROM States", this.eventString);
        }

        /// <summary>
        /// Test the query throw exception code path
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void QueryListThrowException()
        {
            this.Query(null);
        }

        /// <summary>
        /// Test the execute raise event by subscribing to the Database event
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void EventFiringQueryListThrowExceptionEventTest()
        {
            try
            {
                // Subscribe to the event
                this.DatabaseErrorEvent += this.DatabaseUnitTestEvent;

                this.Query(null);
            }
            catch (InvalidOperationException e)
            {
                var message = StringProcessor.SafeFormatter("Failed because: {0}", e.Message);
                Assert.IsTrue(this.eventString.Contains(message));
            }
        }

        /// <summary>
        /// Test the query list code path
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void EventFiringQueryTest()
        {
            var states = this.Query<States>("SELECT * FROM States").ToList();

            // Our database only has 49 states
            Assert.AreEqual(49, states.Count, "Expected 49 states.");
        }

        /// <summary>
        /// Test the query list raise event by subscribing to the Database event
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void EventFiringQueryEventTest()
        {
            // Subscribe to the event
            this.DatabaseEvent += this.DatabaseUnitTestEvent;

            this.Query<States>("SELECT * FROM States");
            
            Assert.AreEqual("Performing query with:\r\nSELECT * FROM States", this.eventString);
        }

        /// <summary>
        /// Test the query map throw exception code path
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void QueryThrowException()
        {
            this.Query<string>(null);
        }

        /// <summary>
        /// Test the execute raise event by subscribing to the Database event
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void EventFiringQueryThrowExceptionEventTest()
        {
            try
            {
                // Subscribe to the event
                this.DatabaseErrorEvent += this.DatabaseUnitTestEvent;

                this.Query<string>(null);
            }
            catch (InvalidOperationException e)
            {
                var message = StringProcessor.SafeFormatter("Failed because: {0}", e.Message);
                Assert.IsTrue(this.eventString.Contains(message));
            }
        }

        /// <summary>
        /// Test the insert throw exception code path
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        [ExpectedException(typeof(SqlException))]
        public void InsertThrowException()
        {
            this.Insert<string>(null);
        }

        /// <summary>
        /// Test the insert raise event by subscribing to the Database event
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void EventFiringInsertThrowExceptionEventTest()
        {
            try
            {
                // Subscribe to the event
                this.DatabaseErrorEvent += this.DatabaseUnitTestEvent;

                this.Insert<string>(null);
            }
            catch (SqlException e)
            {
                var message = StringProcessor.SafeFormatter("Failed because: {0}", e.Message);
                Assert.IsTrue(this.eventString.Contains(message));
            }
        }

        /// <summary>
        /// Test the delete throw exception code path
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteThrowException()
        {
            this.Delete<string>(null);
        }

        /// <summary>
        /// Test the delete raise event by subscribing to the Database event
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void EventFiringDeleteThrowExceptionEventTest()
        {
            try
            {
                // Subscribe to the event
                this.DatabaseErrorEvent += this.DatabaseUnitTestEvent;

                this.Delete<string>(null);
            }
            catch (ArgumentException e)
            {
                var message = StringProcessor.SafeFormatter("Failed because: {0}", e.Message);
                Assert.IsTrue(this.eventString.Contains(message));
            }
        }

        /// <summary>
        /// Test the update throw exception code path
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateThrowException()
        {
            this.Update<string>(null);
        }

        /// <summary>
        /// Test the update raise event by subscribing to the Database event
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void EventFiringUpdateThrowExceptionEventTest()
        {
            try
            {
                // Subscribe to the event
                this.DatabaseErrorEvent += this.DatabaseUnitTestEvent;

                this.Update<string>(null);
            }
            catch (ArgumentException e)
            {
                var message = StringProcessor.SafeFormatter("Failed because: {0}", e.Message);
                Assert.IsTrue(this.eventString.Contains(message));
            }
        }

        /// <summary>
        /// Method to subscribe to the Database Event Handler
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="message">The string message</param>
        public void DatabaseUnitTestEvent(object sender, string message)
        {
            this.eventString = message;
        }
    }
}
