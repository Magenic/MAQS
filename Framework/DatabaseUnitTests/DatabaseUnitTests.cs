//--------------------------------------------------
// <copyright file="DatabaseUnitTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test database driver without base database test</summary>
//--------------------------------------------------

using System;
using System.Data;
using Magenic.Maqs.BaseDatabaseTest;
using Magenic.Maqs.Utilities.Helper;
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
        public void VerifyStateTableExistsNoDriver()
        {
            DatabaseDriver driver = new DatabaseDriver();

            var table = driver.Query("SELECT * FROM information_schema.tables");

            Assert.IsTrue(table.Any(n => n.TABLE_NAME.Equals("States")));
        }

        /// <summary>
        /// Check if we get the expect number of results
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyStateTableHasCorrectNumberOfRecordsNoDriver()
        {
            DatabaseDriver driver = new DatabaseDriver();

            var table = driver.Query("SELECT * FROM States").ToList();

            // Our database only has 49 states
            Assert.AreEqual(49, table.Count, "Expected 49 states.");
        }

        /// <summary>
        /// Check if we get the expect number of results
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyStateTableHasCorrectNumberOfRecordsNoDriverWithModels()
        {
            DatabaseDriver driver = new DatabaseDriver();

            var states = driver.Query<States>("SELECT * FROM States").ToList();

            // Our database only has 49 states
            Assert.AreEqual(49, states.Count, "Expected 49 states.");
            Assert.AreNotEqual(string.Empty, states.First().StateAbbreviation, "Expected nonempty state abbreviation.");
        }

        /// <summary>
        /// Check if Procedures actions can update an item
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresActionWithAnUpdateNoDriver()
        {
            DatabaseDriver driver = new DatabaseDriver(DatabaseConfig.GetProviderTypeString(), DatabaseConfig.GetConnectionString());

            var result = driver.Execute("setStateAbbrevToSelf", new { StateAbbreviation = "MN" }, commandType: CommandType.StoredProcedure);

            Assert.AreEqual(1, result, "Expected 1 state abbreviation to be updated.");
        }

        /// <summary>
        /// Check if Procedures actions work when no items are affected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresActionWithNoUpdatesNoDriver()
        {
            DatabaseDriver driver = new DatabaseDriver(DatabaseConfig.GetProviderTypeString(), DatabaseConfig.GetConnectionString());

            var result = driver.Execute("setStateAbbrevToSelf", new { StateAbbreviation = "ZZ" }, commandType: CommandType.StoredProcedure);
            
            Assert.AreEqual(0, result, "Expected 0 state abbreviation to be updated.");
        }

        /// <summary>
        /// Check if Procedures queries work when an item is returned
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresQueryWithResultNoDriver()
        {
            DatabaseDriver driver = new DatabaseDriver(DatabaseConfig.GetProviderTypeString(), DatabaseConfig.GetConnectionString());

            var result = driver.Query("getStateAbbrevMatch", new { StateAbbreviation = "MN" }, commandType: CommandType.StoredProcedure);

            Assert.AreEqual(1, result.Count(), "Expected 1 state abbreviation to be returned.");
        }

        /// <summary>
        /// Check if Procedures queries work when no results are found
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresQueryWithoutResultNoDriver()
        {
            DatabaseDriver driver = new DatabaseDriver(DatabaseConfig.GetProviderTypeString(), DatabaseConfig.GetConnectionString());

            var result = driver.Query("getStateAbbrevMatch", new { StateAbbreviation = "ZZ" }, commandType: CommandType.StoredProcedure);
            
            Assert.AreEqual(0, result.Count(), "Expected 0 state abbreviation to be returned.");
        }
    }
}
