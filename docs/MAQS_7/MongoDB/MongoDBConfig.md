# <img src="resources/maqslogo.ico" height="32" width="32"> MongoDB Config

## Overview
Config class for mongoDB database

## GetConnectionString
Get the client connection string
```csharp
public static string GetConnectionString()
{
    return Config.GetValueForSection(MONGOSECTION, "MongoConnectionString");
}
```

## GetDatabaseString
Get the database connection string
```csharp
public static string GetDatabaseString()
{
    return Config.GetValueForSection(MONGOSECTION, "MongoDatabase");
}
```

## GetCollectionString
Get the mongo collection string
```csharp
public static string GetCollectionString()
{
    return Config.GetValueForSection(MONGOSECTION, "MongoCollection");
}
```

## GetQueryTimeout
Get the database timeout in seconds
```csharp
public static int GetQueryTimeout()
{
    return int.Parse(Config.GetValueForSection(MONGOSECTION, "MongoTimeout", "30"));
}
```
