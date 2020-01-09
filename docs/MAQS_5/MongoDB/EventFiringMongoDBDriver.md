# <img src="resources/maqslogo.ico" height="32" width="32"> MongoDB Event Firing Driver

## Overview
Wraps the basic firing database interactions

## ListAllCollectionItems
Check if the account is accessible
```csharp
public override List<T> ListAllCollectionItems()
{
    try
    {
        this.RaiseEvent("list all collection items");
        return base.ListAllCollectionItems();
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## IsCollectionEmpty
Checks if the collection contains any records
```csharp
public override bool IsCollectionEmpty()
{
    try
    {
        this.RaiseEvent("Is collection empty");
        return base.IsCollectionEmpty();
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## CountAllItemsInCollection
Counts all of the items in the collection
```csharp
public override int CountAllItemsInCollection()
{
    try
    {
        this.RaiseEvent("Count all items in collection");
        return base.CountAllItemsInCollection();
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## OnEvent
Database event logging
```csharp
this.OnEvent(StringProcessor.SafeFormatter("Performing {0}.", actionType));
```

## OnErrorEvent
Database error event logging
```csharp
this.OnErrorEvent(StringProcessor.SafeFormatter("Failed to log event because: {0}", e.ToString()));
```

## RaiseEvent
Raise an event message
```csharp
this.RaiseEvent("list all collection items");
```

## RaiseErrorMessage
Raise an exception message
```csharp
this.RaiseErrorMessage(ex);
```