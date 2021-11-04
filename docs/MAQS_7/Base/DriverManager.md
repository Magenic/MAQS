# <img src="resources/maqslogo.ico" height="32" width="32"> Driver Manager

## Overview
The DriverManager has overreach of te BaseTestObject and the BaseDriver.

[Log](#Log)
[BaseDriver](#BaseDriver)  
[GetDriver](#GetDriver)
[IsDriverIntialized](#IsDriverIntialized)
[Get](#Get)
[Dispose](#Dispose)
[DriverDisposer](#DriverDispose)
[GetBase](#GetBase)

## Log
Gets the testing object
```csharp
Logger log = this.Log;
```

## BaseDriver
Gets or sets the underlying driver; like the web driver or database connection driver
 ```csharp
 this.BaseDriver = this.GetDriver();
 ```

## GetDriver
Gets or sets the function for getting the underlying driver
```csharp
this.BaseDriver = this.GetDriver();
```

## IsDriverIntialized
Check if the underlying driver has been initialized
```csharp
bool initialiized = this.IsDriverIntialized();
```

## Dispose
Cleanup the driver
```csharp
this.driver.Dispose();
```

## DriverDispose
Dispose driver specific objects
```csharp
this.DriverDispose();
```

## GetBase
Get the underlying driver
```csharp
object base = GetBase();
```