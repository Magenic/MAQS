//--------------------------------------------------
// <copyright file="DatabaseUnitTestsWithWrapper.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Database base test unit tests</summary>
//--------------------------------------------------

using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using DatabaseUnitTests.Models;
using Magenic.Maqs.BaseDatabaseTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DatabaseUnitTests
{
    /// <summary>
    /// Test basic database base test functionality
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DatabaseUnitTestsWithWrapper : BaseDatabaseTest
    {
        /// <summary>
        /// Check that we get back the state table
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyStateTableExists()
        {
            var table = this.DatabaseWrapper.Query("SELECT * FROM information_schema.tables").ToList();

            Assert.IsTrue(table.Any(n => n.TABLE_NAME.Equals("States")));
        }

        /// <summary>
        /// Check if we get the expect number of results
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyStateTableHasCorrectNumberOfRecords()
        {
            var states = this.DatabaseWrapper.Query("SELECT * FROM States").ToList();

            // Our database only has 49 states
            Assert.AreEqual(49, states.Count, "Expected 49 states.");
        }

        /// <summary>
        /// Check if we get the expect number of results, mapping to an object
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyStateTableHasCorrectNumberOfRecordsWithModels()
        {
            var states = this.DatabaseWrapper.Query<States>("SELECT * FROM States").ToList();

            // Our database only has 49 states
            Assert.AreEqual(49, states.Count, "Expected 49 states.");
        }

        /// <summary>
        /// Check if we get the expected mapped data when mapping to an object
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyStateTableDataIsCorrectWithModels()
        {
            var states = this.DatabaseWrapper.Query<States>("SELECT * FROM States").ToList();

            Assert.AreNotEqual(string.Empty, states.First().StateAbbreviation, "Expected nonempty state abbreviation.");
        }

        /// <summary>
        /// Check if Procedures actions can update an item
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresActionWithAnUpdate()
        {
            var result = this.DatabaseWrapper.Execute("setStateAbbrevToSelf", new { StateAbbreviation = "MN" }, commandType: CommandType.StoredProcedure);
            
            Assert.AreEqual(1, result, "Expected 1 state abbreviation to be updated.");
        }
        
        /// <summary>
        /// Verify that a non query SQL call works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyNonQuerySqlCallWorks()
        {
            string query = @"UPDATE States SET StateAbbreviation = 'WI' WHERE StateAbbreviation = 'WI'";
            var result = this.DatabaseWrapper.Execute(query);
            Assert.AreEqual(1, result, "Expected 1 state abbreviation to be updated.");
        }

        /// <summary>
        /// Check if Procedures actions work when no items are affected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresActionWithNoUpdates()
        {
            var result = this.DatabaseWrapper.Execute("setStateAbbrevToSelf", new { StateAbbreviation = "ZZ" }, commandType: CommandType.StoredProcedure);

            Assert.AreEqual(0, result, "Expected 0 state abbreviation to be updated.");
        }

        /// <summary>
        /// Check if Procedures queries work when an item is returned
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresQueryWithResult()
        {
            var result = this.DatabaseWrapper.Execute("setStateAbbrevToSelf", new { StateAbbreviation = "MN" }, commandType: CommandType.StoredProcedure);
            
            Assert.AreEqual(1, result, "Expected 1 state abbreviation to be returned.");
        }

        /// <summary>
        /// Check if Procedures queries work when no results are found
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresQueryWithoutResult()
        {
            var result = this.DatabaseWrapper.Execute("setStateAbbrevToSelf", new { StateAbbreviation = "ZZ" }, commandType: CommandType.StoredProcedure);
            
            Assert.AreEqual(0, result, "Expected 0 state abbreviation to be returned.");
        }

        /// <summary>
        /// Make sure the test objects map properly
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        [TestCategory(TestCategories.Utilities)]
        public void DatabaseTestObjectMapCorrectly()
        {
            Assert.AreEqual(this.TestObject.Log, this.Log, "Logs don't match");
            Assert.AreEqual(this.TestObject.SoftAssert, this.SoftAssert, "Soft asserts don't match");
            Assert.AreEqual(this.TestObject.PerfTimerCollection, this.PerfTimerCollection, "Soft asserts don't match");
            Assert.AreEqual(this.TestObject.DatabaseWrapper, this.DatabaseWrapper, "Web service wrapper don't match");
        }

        /// <summary>
        /// Make sure test object values are saved as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        [TestCategory(TestCategories.Utilities)]
        public void DatabaseTestObjectValuesCanBeUsed()
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
        [TestCategory(TestCategories.Database)]
        [TestCategory(TestCategories.Utilities)]
        public void DatabaseTestObjectObjectsCanBeUsed()
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
