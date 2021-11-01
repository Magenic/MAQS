# <img src="resources/maqslogo.ico" height="32" width="32"> MongoDB Driver

## Overview
Class to wrap the IMongoCollection and related helper functions

## Client
Gets the client object
```csharp
 public IMongoClient Client
{
    get
    {
        return this.client;
    }
}
```

## Database
Gets the database object
```csharp
public IMongoDatabase Database
{
    get
    {
        return this.database;
    }
}
```

## Collection
Gets the collection object
```csharp
public IMongoCollection<T> Collection
{
    get
    {
        return this.collection;
    }
}
```

## ListAllCollectionItems
List all of the items in the collection
```csharp
public virtual List<T> ListAllCollectionItems()
{
    return this.collection.Find<T>(_ => true).ToList();
}
```

## IsCollectionEmpty
Checks if the collection contains any records
```csharp
public virtual bool IsCollectionEmpty()
{
    return !this.collection.Find<T>(_ => true).Any();
}
```

## CountAllItemsInCollection
Counts all of the items in the collection
```csharp
public virtual int CountAllItemsInCollection()
{
    return int.Parse(this.collection.CountDocuments(_ => true).ToString());
}
```