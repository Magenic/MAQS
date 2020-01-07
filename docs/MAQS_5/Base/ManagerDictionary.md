# <img src="resources/maqslogo.ico" height="32" width="32"> Manager Dictionary

## Overview


[GetDriver](#GetDriver)
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

## Add
Add a manager
```csharp
ManagerDictionary dictionary = GetDictionary();
dictionary.Add(GetManager());
```

## AddOrOverride
Add or replace a manager
```csharp
ManagerDictionary dictionary = GetDictionary();
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
 ManagerDictionary dictionary = GetDictionary();
dictionary.Add(GetManager());
dictionary.Add(string.Empty, GetManager());
dictionary.Clear();
```

## Dispose
Cleanup the driver
```csharp
this.driver.Dispose();
```