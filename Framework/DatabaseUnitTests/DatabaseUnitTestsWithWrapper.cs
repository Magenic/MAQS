//--------------------------------------------------
// <copyright file="DatabaseUnitTestsWithWrapper.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Database base test unit tests</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseDatabaseTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Text;

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
        /// Did the logging folder exist at the start of the test run
        /// </summary>
        private static bool loggingFolderExistsBeforeRun = false;

        /// <summary>
        /// Setup before running tests
        /// </summary>
        /// <param name="context">The upcoming test context</param>
        [ClassInitialize]
        public static void CheckBeforeClass(TestContext context)
        {
            loggingFolderExistsBeforeRun = TestHelper.DoesFolderExist();
        }

        /// <summary>
        /// Cleanup after we are done running tests
        /// </summary>
        [ClassCleanup]
        public static void CleanupAfterClass()
        {
            TestHelper.Cleanup(loggingFolderExistsBeforeRun);
        }

        /// <summary>
        /// Check that we get back the state table
        /// </summary>
        #region QueryAndGetTableData
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyStateTableExists()
        {
            DataTable table = this.DatabaseWrapper.QueryAndGetDataTable("SELECT * FROM information_schema.tables");

            // Get the list of table names
            List<string> tablesNames = new List<string>(table.Rows.Count);
            foreach (DataRow row in table.Rows)
            {
                tablesNames.Add((string)row["TABLE_NAME"]);
            }

            Assert.IsTrue(tablesNames.Contains("States"));
        }
        #endregion

        /// <summary>
        /// Check if we get the expect number of results
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyStateTableHasCorrectNumberOfRecords()
        {
            DataTable table = this.DatabaseWrapper.QueryAndGetDataTable("SELECT * FROM States");

            // Our database only has 49 states
            Assert.AreEqual(49, table.Rows.Count, "Expected 49 states.");
        }

        /// <summary>
        /// Check if Procedures actions can update an item
        /// </summary>
        #region RunActionProcedure
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresActionWithAnUpdate()
        {
            SqlParameter state = new SqlParameter("StateAbbreviation", "MN");
            int result = this.DatabaseWrapper.RunActionProcedure("setStateAbbrevToSelf", state);
            Assert.AreEqual(1, result, "Expected 1 state abbreviation to be updated.");
        }
        #endregion

        #region NonQuerySqlCall
        /// <summary>
        /// Verify that a non query SQL call works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyNonQuerySqlCallWorks()
        {
            string query = @"UPDATE States SET StateAbbreviation = 'WI' WHERE StateAbbreviation = 'WI'";
            int result = this.DatabaseWrapper.NonQueryAndGetRowsAffected(query);
            Assert.AreEqual(1, result, "Expected 1 state abbreviation to be updated.");
        }
        #endregion

        /// <summary>
        /// Check if Procedures actions work when no items are affected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresActionWithNoUpdates()
        {
            SqlParameter state = new SqlParameter("StateAbbreviation", "ZZ");
            int result = this.DatabaseWrapper.RunActionProcedure("setStateAbbrevToSelf", state);
            Assert.AreEqual(0, result, "Expected 0 state abbreviation to be updated.");
        }

        /// <summary>
        /// Check if Procedures queries work when an item is returned
        /// </summary>
        #region RunQueryProcedure
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresQueryWithResult()
        {
            SqlParameter state = new SqlParameter("StateAbbreviation", "MN");
            DataTable table = this.DatabaseWrapper.RunQueryProcedure("getStateAbbrevMatch", state);
            Assert.AreEqual(1, table.Rows.Count, "Expected 1 state abbreviation to be returned.");
        }
        #endregion

        /// <summary>
        /// Check if Procedures queries work when no results are found
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyProceduresQueryWithoutResult()
        {
            SqlParameter state = new SqlParameter("StateAbbreviation", "ZZ");
            DataTable table = this.DatabaseWrapper.RunQueryProcedure("getStateAbbrevMatch", state);
            Assert.AreEqual(0, table.Rows.Count, "Expected 0 state abbreviation to be returned.");
        }

        /// <summary>
        /// Test to verify a stored procedure exists
        /// </summary>
        #region ProcedureExists
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyStoredProcedureExists()
        {
            Assert.IsTrue(this.DatabaseWrapper.CheckForProcedure("getStateAbbrevMatch"));
        }
        #endregion

        /// <summary>
        /// Test to verify a stored procedure doesn't exist
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyStoredProcedureDoesntExists()
        {
            Assert.IsFalse(this.DatabaseWrapper.CheckForProcedure("getStateAbbrevMatch1"));
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
        public void DatabaseTestObjectObjectssCanBeUsed()
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
