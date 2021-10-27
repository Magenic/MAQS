# <img src="resources/maqslogo.ico" height="32" width="32"> Manager Dictionary

## Overview
The Manager store contains methods to interact with the Driver manager or underlying Driver.

[GetDriver](#GetDriver)  
[GetManager](#GetManager)  
[Add](#Add)   
[AddOrOverride](#AddOrOverride)  
[Remove](#Remove)  
[Clear](#Clear)  
[Dispose](#Dispose)  

## GetDriver
Get the driver for the associated driver manager
```csharp
T driver = this.ManagerStore.GetDriver<WebServiceDriver>("test")

T driver = this.ManagerStore.GetDriver<WebServiceDriver, WebServiceDriverManager>();
```

## GetManager
Get the driver for the associated driver manager
```csharp
T driver = this.ManagerStore.GetManager<WebServiceDriverManager>("test")

T driver = this.ManagerStore.GetManager<WebServiceDriverManager>();
```

## Add
Add a manager
```csharp
ManagerStore dictionary = new ManagerStore();
dictionary.Add(GetManager());
```

## AddOrOverride
Add or replace a manager
```csharp
ManagerStore dictionary = new ManagerStore();
dictionary.Add(GetManager());
dictionary.AddOrOverride(GetManager());
```

## Remove
Remove a driver manager
```csharp
bool removed = this.Remove(key);
```

## Clear
Clears the dictionary
```csharp
ManagerStore dictionary = new ManagerStore();
dictionary.Add(GetManager());
dictionary.Add(string.Empty, GetManager());
dictionary.Clear();
```

## Dispose
Cleanup the driver
```csharp
this.driver.Dispose();
```