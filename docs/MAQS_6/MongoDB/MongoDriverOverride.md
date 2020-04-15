# <img src="resources/maqslogo.ico" height="32" width="32"> BaseMongoTest
The BaseMongoTest class provides access to the TestObject and MongoDBDriver.


## Overriding the Database 
By default, the BaseMongoTest will use the configuration settings from [MAQS config](MAQS_6/MongoDB/MongoDBConfig.md) to create the Mongo connection.

There are three primary ways to override the Mongo connection.

### Override the base database test get connection function
```csharp
/// <summary>
/// Test database functionality
/// </summary>
[TestClass]
public class YOURTESTCLASS : BaseMongoTest<BsonDocument>
{
    /// <summary>
    /// Get the Mongo collection string
    /// </summary>
    /// <returns>The collection string</returns>
    protected override string GetBaseCollectionString()
    {
        return base.GetBaseCollectionString();
    }

    /// <summary>
    /// Get the Mongo database connection string
    /// </summary>
    /// <returns>The database connection string</returns>
    protected override string GetBaseConnectionString()
    {
        return base.GetBaseConnectionString();
    }

    /// <summary>
    /// Get the Mongo database string
    /// </summary>
    /// <returns>The database string</returns>
    protected override string GetBaseDatabaseString()
    {
        return base.GetBaseDatabaseString();
    }
```

### Override Mongo connection
```csharp
// Override with a function call
this.TestObject.OverrideDatabaseConnection(GetMongoColletionFunction);
var collection = MongoFactory.GetDefaultCollection<BsonDocument>();
this.TestObject.OverrideMongoDBDriver(() => collection);

// Override with a lambda expression
this.TestObject.OverrideMongoDBDriver(() => MongoFactory.GetDefaultCollection<BsonDocument>());
```
*_**The above examples do lazy instantiation of the database connection - AKA You only connect to the database if/when you use the MongoDBDriver**_  

### Override the Mongo driver directly
```csharp
// Override with a Mongo driver
MongoDBDriver<BsonDocument> newDriver = new MongoDBDriver<BsonDocument>(MongoFactory.GetDefaultCollection<BsonDocument>());
this.OverrideConnectionDriver(newDriver);

// Override the Mongo driver directly
this.MongoDBDriver = new MongoDBDriver<BsonDocument>(MongoFactory.GetDefaultCollection<BsonDocument>());
```
*_**Overriding the Mongo driver is not advised because it doesn't lazy load the database connection and only provides limited logging capabilities**_  
