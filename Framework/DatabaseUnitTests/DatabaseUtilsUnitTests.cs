//--------------------------------------------------
// <copyright file="DatabaseUnitTests.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Database base utility unit tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseDatabaseTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
        /// Check if column counts are equal
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyDataTableColumnsCount()
        {
            var states = this.DatabaseDriver.Query("SELECT * FROM States").ToList();
            var statesTable = DatabaseUtils.ToDataTable(states);

            // Validate Column Count
            Assert.AreEqual(3, statesTable.Columns.Count);
            Assert.AreEqual(((IDictionary<string, object>)states.First()).Keys.Count, statesTable.Columns.Count);
        }

        /// <summary>
        /// Check if the column names and datatype are equal
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

                // Check name
                Assert.AreEqual(statesTable.Columns[i].ColumnName, dynamicRowKey);

                // Check Data Type
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

        /// <summary>
        /// Check if datatypes are properly parsed
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyDataTableDataTypesMatch()
        {
            var datatypes = this.DatabaseDriver.Query("SELECT * FROM DataType").ToList();
            IDictionary<string, object> datatypesColumns = datatypes.First();
            var datatypesTable = DatabaseUtils.ToDataTable(datatypes);
            var dynamicRowKey = datatypesColumns.Keys.ToList();

            Assert.AreEqual(datatypesTable.Rows[0].ItemArray[0].GetType(), typeof(Int64));
            Assert.AreEqual(datatypesTable.Rows[0].ItemArray[1].GetType(), typeof(bool));
            Assert.AreEqual(datatypesTable.Rows[0].ItemArray[2].GetType(), typeof(string));
            Assert.AreEqual(datatypesTable.Rows[0].ItemArray[3].GetType(), typeof(DateTime));
            Assert.AreEqual(datatypesTable.Rows[0].ItemArray[4].GetType(), typeof(DateTime));
            Assert.AreEqual(datatypesTable.Rows[0].ItemArray[5].GetType(), typeof(double));
            Assert.AreEqual(datatypesTable.Rows[0].ItemArray[6].GetType(), typeof(int));
            Assert.AreEqual(datatypesTable.Rows[0].ItemArray[7].GetType(), typeof(string));
            Assert.AreEqual(datatypesTable.Rows[0].ItemArray[8].GetType(), typeof(string));
            Assert.AreEqual(datatypesTable.Rows[0].ItemArray[9].GetType(), typeof(string));
            Assert.AreEqual(datatypesTable.Rows[0].ItemArray[10].GetType(), typeof(decimal));
            Assert.AreEqual(datatypesTable.Rows[0].ItemArray[11].GetType(), typeof(DBNull));
        }

        /// <summary>
        /// Check that ToDataTable properly parses Null and String values
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyDataTableColumnsNullStringValue()
        {
            var states = this.DatabaseDriver.Query("select City.CityName from States left Join Cities as City on City.CityName like 'Minneaplois' and City.CityId = States.StateId").ToList();
            var statesTable = DatabaseUtils.ToDataTable(states);
            var found = false;

            // Loop through rows and validate
            for (int i = 0; i < statesTable.Rows.Count; i++)
            {
                // Check Data Type
                if (statesTable.Rows[i].ItemArray[0] != null && statesTable.Rows[i].ItemArray[0].GetType() != typeof(DBNull))
                {
                    Type dataType = statesTable.Rows[i].ItemArray[0].GetType();
                    Assert.AreEqual(dataType, typeof(string));
                    found = true;
                }
            }

            Assert.IsTrue(found, "Did not find any non null values. Verify the query is correct.");
        }

        /// <summary>
        /// Check that ToDataTable properly parses Null and int values
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyDataTableColumnsNullIntValue()
        {
            var states = this.DatabaseDriver.Query("select City.CityId from States left Join Cities as City on City.CityName like 'Minneaplois' and City.CityId = States.StateId").ToList();
            var statesTable = DatabaseUtils.ToDataTable(states);
            var found = false;

            // Loop through rows and validate
            for (int i = 0; i < statesTable.Rows.Count; i++)
            {
                // Check Data Type
                if (statesTable.Rows[i].ItemArray[0] != null && statesTable.Rows[i].ItemArray[0].GetType() != typeof(DBNull))
                {
                    Type dataType = statesTable.Rows[i].ItemArray[0].GetType();
                    Assert.AreEqual(dataType, typeof(int));
                    found = true;
                }
            }

            Assert.IsTrue(found, "Did not find any non null values. Verify the query is correct.");
        }

        /// <summary>
        /// Check that ToDataTable properly parses Null and Decimal values
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyDataTableColumnsNullDecimalValue()
        {
            var states = this.DatabaseDriver.Query("select City.CityPopulation from States left Join Cities as City on City.CityName like 'St. Paul' and City.CityId = States.StateId").ToList();
            var statesTable = DatabaseUtils.ToDataTable(states);
            var found = false;

            // Loop through rows and validate
            for (int i = 0; i < statesTable.Rows.Count; i++)
            {
                // Check Data Type
                if (statesTable.Rows[i].ItemArray[0] != null && statesTable.Rows[i].ItemArray[0].GetType() != typeof(DBNull))
                {
                    Type dataType = statesTable.Rows[i].ItemArray[0].GetType();
                    Assert.AreEqual(dataType, typeof(decimal));
                    found = true;
                }
            }

            Assert.IsTrue(found, "Did not find any non null values. Verify the query is correct.");
        }
    }
}
