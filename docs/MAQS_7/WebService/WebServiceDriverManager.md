# <img src="resources/maqslogo.ico" height="32" width="32"> Web Service Driver Manager

## Overview
The WebService Driver Manager has overreach of the Base Driver Manager.

[OverrideDriver](#OverrideDriver)
[GetWebSeriveDriver](#GetWebSeriveDriver)  
[Get](#Get)
[MapEvents](#MapEvents)
[DriverDispose](#DriverDispose)
[WebService_Event](#WebService_Event)
[WebService_Error](#WebService_Error)

## OverrideDriver
Override the http driver
```csharp
 this.driver = driver;
 ```

## GetWebSeriveDriver
Get the http driver
 ```csharp
WebServiceDriver = GetWebServiceDriver();
 ```

## Get
Get the service driver
```csharp
object driver = Get();
```

## MapEvents
Map web service events to log events
```csharp
public void MapEvents(EventFiringWebServiceDriver eventFiringDriver)
{
    eventFiringDriver.WebServiceEvent += this.WebService_Event;
    eventFiringDriver.WebServiceErrorEvent += this.WebService_Error;
}
```

## DriverDispose
Dispose of the driver
```csharp
this.DriverDispose();
```

## WebService_Event
Logs Web service events
```csharp
eventFiringDriver.WebServiceEvent += this.WebService_Event;
```

## WebService_Error
Logs Web service error events
```csharp
eventFiringDriver.WebServiceErrorEvent += this.WebService_Error;
```