//--------------------------------------------------
// <copyright file="DatabaseUnitTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test database wrapper without base database test</summary>
//--------------------------------------------------

using System;
using System.Data;
using Magenic.MaqsFramework.BaseDatabaseTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DatabaseUnitTests.Models;

namespace DatabaseUnitTests
{
    /// <summary>
    /// Test basic database base test functionality
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DatabaseUnitTests
    {
        /// <summary>
        /// Check that we get back the state table
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyStateTableExistsNoWrapper()
        {
            DatabaseDriver wrapper = new DatabaseDriver();

            var table = wrapper.Query("SELECT * FROM information_schema.tables");

            Assert.IsTrue(table.Any(n => n.TABLE_NAME.Equals("States")));
        }

        /// <summary>
        /// Check if we get the expect number of results
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyStateTableHasCorrectNumberOfRecordsNoWrapper()
        {
            DatabaseDriver wrapper = new DatabaseDriver();

            var table = wrapper.Query("SELECT * FROM States").ToList();

            // Our database only has 49 states
            Assert.AreEqual(49, table.Count, "Expected 49 states.");
        }

        /// <summary>
        /// Check if we get the expect number of results
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyStateTableHasCorrectNumberOfRecordsNoWrapperWithModels()
        {
            DatabaseDriver wrapper = new DatabaseDriver();

            var states = wrapper.Query<States>("SELECT * FROM States").ToList();

            // Our database only has 49 states
            Assert.AreEqual(49, states.Count, "Expected 49 states.");
            Assert.AreNotEqual(string.Empty, states.First().StateAbbreviation, "Expected nonempty state abbreviation.");
        }

        /// <summary>
        /// Check if Procedures actions can update an item
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresActionWithAnUpdateNoWrapper()
        {
            DatabaseDriver wrapper = new DatabaseDriver(DatabaseConfig.GetProviderTypeString(), DatabaseConfig.GetConnectionString());

            var result = wrapper.Execute("setStateAbbrevToSelf", new { StateAbbreviation = "MN" }, commandType: CommandType.StoredProcedure);

            Assert.AreEqual(1, result, "Expected 1 state abbreviation to be updated.");
        }

        /// <summary>
        /// Check if Procedures actions work when no items are affected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresActionWithNoUpdatesNoWrapper()
        {
            DatabaseDriver wrapper = new DatabaseDriver(DatabaseConfig.GetProviderTypeString(), DatabaseConfig.GetConnectionString());

            var result = wrapper.Execute("setStateAbbrevToSelf", new { StateAbbreviation = "ZZ" }, commandType: CommandType.StoredProcedure);
            
            Assert.AreEqual(0, result, "Expected 0 state abbreviation to be updated.");
        }

        /// <summary>
        /// Check if Procedures queries work when an item is returned
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresQueryWithResultNoWrapper()
        {
            DatabaseDriver wrapper = new DatabaseDriver(DatabaseConfig.GetProviderTypeString(), DatabaseConfig.GetConnectionString());

            var result = wrapper.Query("getStateAbbrevMatch", new { StateAbbreviation = "MN" }, commandType: CommandType.StoredProcedure);

            Assert.AreEqual(1, result.Count(), "Expected 1 state abbreviation to be returned.");
        }

        /// <summary>
        /// Check if Procedures queries work when no results are found
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresQueryWithoutResultNoWrapper()
        {
            DatabaseDriver wrapper = new DatabaseDriver(DatabaseConfig.GetProviderTypeString(), DatabaseConfig.GetConnectionString());

            var result = wrapper.Query("getStateAbbrevMatch", new { StateAbbreviation = "ZZ" }, commandType: CommandType.StoredProcedure);
            
            Assert.AreEqual(0, result.Count(), "Expected 0 state abbreviation to be returned.");
        }
    }
}
