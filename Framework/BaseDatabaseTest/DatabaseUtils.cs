//--------------------------------------------------
// <copyright file="DatabaseUtils.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Database utilities</summary>
//--------------------------------------------------
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Magenic.Maqs.BaseDatabaseTest
{
    /// <summary>
    /// Database utilities
    /// </summary>
    public static class DatabaseUtils
    {
        /// <summary>
        /// Transform dynamic list into a data table
        /// </summary>
        /// <param name="dynamicList">The dynamic list</param>
        /// <returns>Returns dynamicList as a data table</returns>
        public static DataTable ToDataTable(List<dynamic> dynamicList)
        {
            DataTable dataTable = new DataTable();

            // Return empty datatable if empty
            if (!dynamicList.Any())
            {
                return dataTable;
            }

            // Add column to DataTable. DapperRows store column names as keys in a dictionary.
            var dynamicColumns = (IDictionary<string, object>)dynamicList.First();

            // Maintain datatype
            foreach (var column in dynamicColumns)
            {
                DataColumn dataTableColumn = ToDataColumn(column);
                dataTable.Columns.Add(dataTableColumn);
            }

            // Store the values in the datatable
            foreach (IDictionary<string, object> dynamicRow in dynamicList.OfType<IDictionary<string, object>>())
            {
                DataRow newDataRow = dataTable.NewRow();
                foreach (var key in dynamicRow.Keys)
                {
                    // Add the value to the data row from the dapper row using the key                    
                    newDataRow[key] = dynamicRow[key];
                }

                dataTable.Rows.Add(newDataRow);
            }

            return dataTable;
        }

        /// <summary>
        /// Creates a Data Column from a key value pair
        /// </summary>
        /// <param name="column">The data column as a key value pair</param>
        /// <returns>The column data as a data column object</returns>
        public static DataColumn ToDataColumn(KeyValuePair<string, object> column)
        {
            DataColumn dataTableColumn = new DataColumn();
            dataTableColumn.DataType = column.Value == null ? typeof(object) : column.Value.GetType();
            dataTableColumn.ColumnName = column.Key;
            return dataTableColumn;
        }
    }
}
