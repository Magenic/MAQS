//--------------------------------------------------
// <copyright file="DatabaseUnitTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test database wrapper without base database test</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseDatabaseTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

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
            DatabaseConnectionWrapper wrapper = new DatabaseConnectionWrapper(DatabaseConfig.GetConnectionString());

            DataTable table = wrapper.QueryAndGetDataTable("SELECT * FROM information_schema.tables");

            // Get the list of table names
            List<string> tablesNames = new List<string>(table.Rows.Count);
            foreach (DataRow row in table.Rows)
            {
                tablesNames.Add((string)row["TABLE_NAME"]);
            }

            Assert.IsTrue(tablesNames.Contains("States"));
        }

        /// <summary>
        /// Check if we get the expect number of results
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyStateTableHasCorrectNumberOfRecordsNoWrapper()
        {
            DatabaseConnectionWrapper wrapper = new DatabaseConnectionWrapper(DatabaseConfig.GetConnectionString());

            DataTable table = wrapper.QueryAndGetDataTable("SELECT * FROM States");

            // Our database only has 49 states
            Assert.AreEqual(49, table.Rows.Count, "Expected 49 states.");
        }

        /// <summary>
        /// Check if Procedures actions can update an item
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresActionWithAnUpdateNoWrapper()
        {
            DatabaseConnectionWrapper wrapper = new DatabaseConnectionWrapper(DatabaseConfig.GetConnectionString());

            SqlParameter state = new SqlParameter("StateAbbreviation", "MN");
            int result = wrapper.RunActionProcedure("setStateAbbrevToSelf", state);
            Assert.AreEqual(1, result, "Expected 1 state abbreviation to be updated.");
        }

        /// <summary>
        /// Check if Procedures actions work when no items are affected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresActionWithNoUpdatesNoWrapper()
        {
            DatabaseConnectionWrapper wrapper = new DatabaseConnectionWrapper(DatabaseConfig.GetConnectionString());

            SqlParameter state = new SqlParameter("StateAbbreviation", "ZZ");
            int result = wrapper.RunActionProcedure("setStateAbbrevToSelf", state);
            Assert.AreEqual(0, result, "Expected 0 state abbreviation to be updated.");
        }

        /// <summary>
        /// Check if Procedures queries work when an item is returned
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresQueryWithResultNoWrapper()
        {
            DatabaseConnectionWrapper wrapper = new DatabaseConnectionWrapper(DatabaseConfig.GetConnectionString());

            SqlParameter state = new SqlParameter("StateAbbreviation", "MN");
            DataTable table = wrapper.RunQueryProcedure("getStateAbbrevMatch", state);
            Assert.AreEqual(1, table.Rows.Count, "Expected 1 state abbreviation to be returned.");
        }

        /// <summary>
        /// Check if Procedures queries work when no results are found
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresQueryWithoutResultNoWrapper()
        {
            DatabaseConnectionWrapper wrapper = new DatabaseConnectionWrapper(DatabaseConfig.GetConnectionString());

            SqlParameter state = new SqlParameter("StateAbbreviation", "ZZ");
            DataTable table = wrapper.RunQueryProcedure("getStateAbbrevMatch", state);
            Assert.AreEqual(0, table.Rows.Count, "Expected 0 state abbreviation to be returned.");
        }
    }
}
