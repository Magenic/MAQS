# <img src="resources/maqslogo.ico" height="32" width="32"> Database Test Object

## Overview
Takes care of the Database test context data

[DatabaseManager](#DatabaseManager)  
[DatabaseDriver](#DatabaseDriver)  
[OverrideDatabaseConnection](#OverrideDatabaseConnection)  
[OverrideDatabaseDriver](#OverrideDatabaseDriver)  

## DatabaseManager
Gets the database driver manager
```csharp
public DatabaseDriverManager DatabaseManager
{
    get
    {
        return this.ManagerStore[typeof(DatabaseDriverManager).FullName] as DatabaseDriverManager;
    }
}
```

## DatabaseDriver
Gets the database driver
```csharp
 public DatabaseDriver DatabaseDriver
{
    get
    {
        return this.DatabaseManager.GetDatabaseDriver();
    }
}
```

## OverrideDatabaseConnection
Override the function for getting a database connection
```csharp
public void OverrideDatabaseConnection(Func<IDbConnection> databaseConnection)
{
    this.OverrideDriverManager(typeof(DatabaseDriverManager).FullName, new DatabaseDriverManager(databaseConnection, this));
}
```

## OverrideDatabaseDriver
Override the database connection and the driver.
```csharp
public void OverrideDatabaseDriver(IDbConnection databaseConnection)
{
    this.OverrideDriverManager(typeof(DatabaseDriverManager).FullName, new DatabaseDriverManager(() => databaseConnection, this));
}

public void OverrideDatabaseDriver(DatabaseDriver driver)
{
    this.DatabaseManager.OverrideDriver(driver);
}
```