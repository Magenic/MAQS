# <img src="resources/maqslogo.ico" height="32" width="32"> Database Connection Factory

## Overview
Config class for database connections

[GetOpenConnection](#GetOpenConnection)  
[GetProvider](#GetProvider)  
[GetSQLServerProvider](#GetSQLServerProvider)  
[GetSqliteProvider](#GetSqliteProvider)  
[GetPostgreSqlProvider](#GetPostgreSqlProvider)  
[GetCustomProviderType](#GetCustomProviderType)  

## GetOpenConnection
Gets a database connection based on configuration values
```csharp
public static IDbConnection GetOpenConnection()
{
    return GetOpenConnection(DatabaseConfig.GetProviderTypeString(), DatabaseConfig.GetConnectionString());
}

```

## GetProvider
Gets the provider based on the provider type.
```csharp
IDbConnection connection =  GetProvider(providerType).SetupDataBaseConnection(connectionString);
```

## GetSQLServerProvider
Get a SQL server provider
```csharp
 IProvider<IDbConnection> provider = GetSQLServerProvider();
```

## GetSqliteProvider
Get a SQL lite provider
```csharp
IProvider<IDbConnection> provider = GetSqliteProvider();
```

## GetPostgreSqlProvider
Get a PostgreSQL provider
```csharp
IProvider<IDbConnection> provider = GetPostgreSqlProvider();
```

## GetCustomProviderType
Checks if the provider type key value is supported and try to create the type.
```csharp
IProvider<IDbConnection> provider = GetCustomProviderType(providerType);
```