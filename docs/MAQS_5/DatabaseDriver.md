# <img src="resources/maqslogo.ico" height="32" width="32"> DatabaseDriver

## The DatabaseDriver
The DatabaseDrive object is included in the DatabaseTestObject. The driver sets and opens a connection to the database on instansitaiton. The driver wraps Dapper functionality. 

### EventFiringDatabaseDriver
Similar to the DatabaseDriver, except raises an event before a query. 

### BaseDatabaseTest and DatabaseDriver
Using the DatabaseDriver within a BaseDatabaseTest is easy, simply call the driver: 

```csharp
 var orders = this.DatabaseDriver.Query<Orders>("select * from orders").ToList();

```

### DatabaseDriver without BaseDatabaseTest
To use the DatabaseDriver without the BaseDatabaseTest, simply create the driver object. 

#### Using the provider type and connection string configuration keys:
```csharp
    DatabaseDriver driver = new DatabaseDriver();
    var table = driver.Query("SELECT * FROM States").ToList();
```

#### Using a string provider type and connection string
```csharp
    DatabaseDriver driver = new DatabaseDriver("SQLSERVER", "some_connection_string");
```

#### Using a DBConnection

```csharp
    DatabaseDriver tempDriver = new DatabaseDriver(DatabaseConfig.GetOpenConnection());
```
