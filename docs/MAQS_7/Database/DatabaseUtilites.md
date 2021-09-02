# <img src="resources/maqslogo.ico" height="32" width="32"> Database Utilities

## Overview
Database utilities that helps with data tables and data column.

## ToDataTable
Transform a dynamic list into a data table
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

## ToDataColumn
Creates a Data Column from a key value pair
```csharp
public static DataColumn ToDataColumn(KeyValuePair<string, object> column)
{
    DataColumn dataTableColumn = new DataColumn();
    dataTableColumn.DataType = column.Value == null ? typeof(object) : column.Value.GetType();
    dataTableColumn.ColumnName = column.Key;
    return dataTableColumn;
}
```