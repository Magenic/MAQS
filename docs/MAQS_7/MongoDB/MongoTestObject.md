# <img src="resources/maqslogo.ico" height="32" width="32"> MongoDB Test Object

## Overview
Manages the MongoDB test objects.

## MongoDBManager
Gets the Mongo driver manager
```csharp
MongoDBDriver<T> driver = this.MongoDBManager.GetMongoDriver();
```

## MongoDBDriver
Gets the Mongo driver
```csharp
public MongoDBDriver<T> MongoDBDriver
{
    get
    {
        return this.MongoDBManager.GetMongoDriver();
    }
}
```

## OverrideMongoDBDriver
Override the Mongo driver settings
```csharp
public void OverrideMongoDBDriver(string connectionString, string databaseString, string collectionString)
{
    this.ManagerStore.Remove(typeof(MongoDriverManager<T>).FullName);
    this.ManagerStore.Add(typeof(MongoDriverManager<T>).FullName, new MongoDriverManager<T>(connectionString, databaseString, collectionString, this));
}

public void OverrideMongoDBDriver(MongoDBDriver<T> driver)
{
    this.MongoDBManager.OverrideDriver(driver);
}
```