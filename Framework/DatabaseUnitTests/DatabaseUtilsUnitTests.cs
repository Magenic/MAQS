//--------------------------------------------------
// <copyright file="DatabaseUnitTests.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Database base utility unit tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseDatabaseTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DatabaseUnitTests
{
    /// <summary>
    /// Database untilies unit tests
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DatabaseUtilsUnitTests : BaseDatabaseTest
    {
        /// <summary>
        /// Check if we get the expect number of results
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyDataTableCountMatch()
        {
            var states = this.DatabaseDriver.Query("SELECT * FROM States").ToList();
            var statesTable = DatabaseUtils.ToDataTable(states);
            // Our database only has 49 states
            Assert.AreEqual(states.Count, statesTable.Rows.Count, "Expected 49 states.");
            Assert.AreEqual(49, statesTable.Rows.Count, "Expected 49 states.");
        }

        /// <summary>
        /// Check if the values are equal
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyDataTableColumnsCount()
        {
            var states = this.DatabaseDriver.Query("SELECT * FROM States").ToList();
            var statesTable = DatabaseUtils.ToDataTable(states);

            //Validate Column Count
            Assert.AreEqual(3, statesTable.Columns.Count);
            Assert.AreEqual(((IDictionary<string, object>)states.First()).Keys.Count, statesTable.Columns.Count);
        }

        /// <summary>
        /// Check if the values are equal
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyDataTableColumnsMatch()
        {
            var states = this.DatabaseDriver.Query("SELECT * FROM States").ToList();
            IDictionary<string, object> statesColumns = states.First();
            var statesTable = DatabaseUtils.ToDataTable(states);
            
            // Loop through rows and validate
            for (int i = 0; i < statesTable.Columns.Count; i++)
            {
                var dynamicRowKey = statesColumns.Keys.ToList()[i];
                var dynamicRowType = statesColumns[dynamicRowKey].GetType();

                //Check name
                Assert.AreEqual(statesTable.Columns[i].ColumnName, dynamicRowKey);

                //Check Data Type
                Assert.AreEqual(statesTable.Columns[i].DataType, dynamicRowType);
            }
        }

        /// <summary>
        /// Check if the values are equal
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyDataTableValuesMatch()
        {
            var states = this.DatabaseDriver.Query("SELECT * FROM States").ToList();
            var statesTable = DatabaseUtils.ToDataTable(states);

            // Loop through rows and validate
            for(int i = 0; i < states.Count; i++)
            {
                DataRow dataTableRow = statesTable.Rows[i];
                IDictionary<string, object> dynamicRow = states[i];

                foreach (var key in dynamicRow.Keys)
                {
                    var dynamicValue = dynamicRow[key];
                    var dataTablevalue = dataTableRow[key];
                    Assert.AreEqual(dynamicValue, dataTablevalue);
                }               
            }
        }
    }
}
