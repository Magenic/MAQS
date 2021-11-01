# <img src="resources/maqslogo.ico" height="32" width="32"> Email Driver Manager

## Overview
The Email Driver Manager has overreach of the Base Driver Manager.

[OverrideDriver](#OverrideDriver)  
[GetEmailDriver](#GetWebSeriveDriver)  
[Get](#Get)  
[MapEvents](#MapEvents)  
[DriverDispose](#DriverDispose)  
[Email_Event](#Email_Event)  
[Email_Error](#Email_Error)  

## OverrideDriver
Override the http driver
```csharp
 this.driver = driver;
 ```

## GetEmailDriver
Get the http driver
 ```csharp
EmailDriver = GetEmailDriver();
 ```

## Get
Get the service driver
```csharp
object driver = Get();
```

## DriverDispose
Dispose of the driver
```csharp
this.DriverDispose();
```

## MapEvents
Map email events to log events
```csharp
private void MapEvents(EventFiringEmailDriver eventFiringConnectionDriver)
{
    eventFiringConnectionDriver.EmailEvent += this.Email_Event;
    eventFiringConnectionDriver.EmailErrorEvent += this.Email_Error;
}
```

## Email_Event
Email event
```csharp
eventFiringConnectionDriver.EmailEvent += this.Email_Event;
```

## Email_Error
Email error event
```csharp
eventFiringConnectionDriver.EmailErrorEvent += this.Email_Error;
```