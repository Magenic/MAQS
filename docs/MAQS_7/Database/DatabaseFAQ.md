# <img src="resources/maqslogo.ico" height="32" width="32"> Database FAQ

## What about DataTables
***MAQS and Dapper*** shifts away with DataTables and instead leveages a strategy around strongly typed and dynamic object lists. Migrating from MAQS 4 to 5 requires either a rewrite of queries to support the mapping strategy, or a conversion of the returned objects into a DataTable.

## How do use a provider besides SQL, SQLite and PostgreSql
There are multiple ways to use a custom provider. 

* [Override the GetDataBaseConnection method](MAQS_6/Database/DatabaseBaseTest.md)
* [Create your own DatabaseDriver](MAQS_6/Database/DatabaseDriver.md)
* [Implement the IProvider Class](MAQS_6/Database/DatabaseProviders.md)

## Why doesn't MAQS directly support Oracle SQL anymore
The Oracle SQL library has a non-standard license and we are not confident it adheres to most user licensing standards.

## How Can to Connect to an Oracle SQL Database
 
### Adding the Oracle Database Connector
Adding the Oracle database connector can be added by adding the "Oracle.ManagedDataAccess.Core"  NuGet package.
### Setting Up the Connection Inline
```csharp
/// <summary>
/// Check that we get back the state table
/// </summary>
[TestMethod]
public void AddInTest()
{
    // Adding the connection inline
    this.TestObject.OverrideDatabaseConnection(() => new OracleConnection(DatabaseConfig.GetConnectionString()));

    var states = this.DatabaseDriver.Query("SELECT * FROM States").ToList();
    Assert.AreEqual(50, states.Count, "Expected 50 states.");
}
```

### Or Setting Up the Connection for All Tests In A Class
```csharp
[TestClass]
public class DatabaseCustomUnitTests : BaseDatabaseTest
{
    /// <summary>
    /// Overridde the database connection
    /// </summary>
    /// <returns>The database connection</returns>
    protected override IDbConnection GetDataBaseConnection()
    {
        return new OracleConnection(DatabaseConfig.GetConnectionString());
    }
```