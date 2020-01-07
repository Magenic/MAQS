# <img src="resources/maqslogo.ico" height="32" width="32"> BaseDatabaseTest
The BaseDatabaseTest class provides access to the DatabaseTestObject and DatabaseDriver.

# Available calls
[GetDatabaseConnection](#GetDatabaseConnection)  
[CreateNewTestObject](#CreateNewTestObject)  

## DatabaseConnection
This method gets the database connection. 
```csharp
protected virtual IDbConnection GetDataBaseConnection()
{
    return DatabaseConfig.GetOpenConnection();
}
```

## CreateNewTestObject
This method creates a database test object.
```csharp
 protected override void CreateNewTestObject()
{
    Logger newLogger = this.CreateLogger();
    this.TestObject = new DatabaseTestObject(() => this.GetDataBaseConnection(), newLogger, new SoftAssert(newLogger), this.GetFullyQualifiedTestClassName());
}
```

## Overriding the GetDataBaseConnection method
By default, the BaseDatabaseTest will use one of the included [MAQS Providers](MAQS_5/DatabaseProviders.md) and configuration connection string. 

There are two primary ways to override the database connection.
  
The first way is to simply replace the DatebaseDriver.  
*This is often done in a test initialize, but it can also be done inside your test.*
```csharp
IDbConnection connection = ConnectionFactory.GetOpenConnection("SQLITE", $"Data Source={GetDByPath()}");
this.DatabaseDriver = new DatabaseDriver(connection);
```

The other way is to override the GetDataBaseConnection method in your test class.

```csharp
/// <summary>
/// Get the database connection
/// </summary>
/// <returns>The database connection</returns>
protected override IDbConnection GetDataBaseConnection()
{
   return new YourNewConnectionFunction();
}
```