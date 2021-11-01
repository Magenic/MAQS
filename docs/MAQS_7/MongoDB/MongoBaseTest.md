# <img src="resources/maqslogo.ico" height="32" width="32"> MongoDB Base Test

## Overview
Generic base MongoDB test class

## MongoDBDriver
Gets or sets the web service driver
```csharp
public MongoDBDriver<T> MongoDBDriver
{
    get
    {
        return this.TestObject.MongoDBDriver;
    }

    set
    {
        this.TestObject.OverrideMongoDBDriver(value);
    }
}
```

## OverrideConnectionDriver
Override the Mongo driver
```csharp
public void OverrideConnectionDriver(MongoDBDriver<T> driver)
{
    this.TestObject.OverrideMongoDBDriver(driver);
}

public void OverrideConnectionDriver(string connectionString, string databaseString, string collectionString)
{
    this.TestObject.OverrideMongoDBDriver(connectionString, databaseString, collectionString);
}
```

## GetBaseConnectionString
Get the base web service url
```csharp
string connection = this.GetBaseConnectionString();
```

## GetBaseDatabaseString
Get the base web service url
```csharp
string database = this.GetBaseDatabaseString();
```

## GetBaseCollectionString
Get the base web service url
```csharp
string collection = this.GetBaseCollectionString()
```

## CreateNewTestObject
Create a MongoDB test object
```csharp
protected override void CreateNewTestObject()
{
    Logger newLogger = this.CreateLogger();
    this.TestObject = new MongoTestObject<T>(this.GetBaseConnectionString(), this.GetBaseDatabaseString(), this.GetBaseCollectionString(), newLogger, new SoftAssert(newLogger), this.GetFullyQualifiedTestClassName());
}
```
