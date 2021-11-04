# <img src="resources/maqslogo.ico" height="32" width="32"> BaseDatabaseTest
The BaseDatabaseTest class provides access to the TestObject and DatabaseDriver.


## Overriding the Database 
By default, the BaseDatabaseTest will use one of the included [MAQS Providers](MAQS_7/Database/DatabaseProviders.md) and configuration connection string. 

There are three primary ways to override the database connection.

### Override the base database test get connection function
```csharp
/// <summary>
/// Test database functionality
/// </summary>
[TestClass]
public class YOURTESTCLASS : BaseDatabaseTest
{
    /// <summary>
    /// Get the database connection
    /// </summary>
    /// <returns>The database connection</returns>
    protected override IDbConnection GetDataBaseConnection()
    {
        return YourNewConnectionFunction();
    }
```

### Override database connection
```csharp
// Override with a function call
this.TestObject.OverrideDatabaseConnection(GetConnectionFunction);

// Override with a lambda expression
this.TestObject.OverrideDatabaseConnection(() => ConnectionFactory.GetOpenConnection("SQLITE", $"Data Source={GetDByPath()}"));
```
*_**The above examples do lazy instantiation of the database connection - AKA You only connect to the database if/when you use the DatabaseDriver**_  

### Override the database driver directly
```csharp
// Override with a database connection
IDbConnection connection = ConnectionFactory.GetOpenConnection("SQLITE", $"Data Source={GetDByPath()}");
this.TestObject.OverrideDatabaseDriver(connection);

// Override with a database driver
DatabaseDriver driver = new DatabaseDriver(connection);
this.TestObject.OverrideDatabaseDriver(driver);

// Override the database driver directly
IDbConnection conn = ConnectionFactory.GetOpenConnection("SQLITE", $"Data Source={GetDByPath()}");
this.DatabaseDriver = new DatabaseDriver(conn);
```
*_**Overriding the database driver is not advised because it doesn't lazy load the database connection and only provides limited logging capabilities**_  
