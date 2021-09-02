# <img src="resources/maqslogo.ico" height="32" width="32"> MongoDB Driver Manager

## Overview
Manages the MongoDB driver, it gets, disposes, maps events, and logs database events. 

## OverrideDriver
Override the Mongo driver
```csharp
public void OverrideDriver(MongoDBDriver<T> overrideDriver)
{
    this.driver = overrideDriver;
}
```

## GetMongoDriver
Get the Mongo driver
```csharp
MongoDBDriver<T> driver = this.GetMongoDriver();
```

## Get
Get the Mongo driver
```csharp
public override object Get()
{
    return this.GetMongoDriver();
}
```

## DriverDispose
Dispose of the driver
```csharp
protected override void DriverDispose()
{
    this.BaseDriver = null;
}
```

## MapEvents
Map database events to log events
```csharp
this.MapEvents((EventFiringMongoDBDriver<T>)this.driver);
```

## Database_Event
Database event logging.
```csharp
eventFiringConnectionDriver.DatabaseEvent += this.Database_Event;
```

## Database_Error
Database error event logging.
```csharp
eventFiringConnectionDriver.DatabaseErrorEvent += this.Database_Error;
```