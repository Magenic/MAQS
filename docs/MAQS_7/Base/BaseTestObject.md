# <img src="resources/maqslogo.ico" height="32" width="32"> Base Test Object

## Overview
Takes care of the base test context data.

[Log](#Log)
[PerftimerCollection](#PerftimerCollection)  
[SoftAssert](#SoftAssert)
[Values](#Values)  
[Objects](#Objects)
[ManagerStore](#ManagerStore)  
[AssociatedFiles](#AssociatedFiles)
[SetValue](#SetValue)  
[SetObject](#SetObject) 
[GetDriverManager](#GetDriverManager)  
[AddDriverManager](#AddDriverManager)
[AddAssociatedFile](AddAssociatedFile)  
[RemoveAssociatedFile](#RemoveAssociatedFile)
[GetArrayOfAssociatedFiles](#GetArrayOfAssociatedFiles)  
[ContainsAssociatedFile](#ContainsAssociatedFile)
[OverrideDriverManager](#OverrideDriverManager)  
[Dispose](#Dispose)

## Log
Gets or sets the logger
```csharp
this.TestObject.Log = this.CreateLogger();
```

## PerftimerCollection
Gets or sets the performance timer collection
```csharp
PerfTimerCollection collection = this.TestObject.PerfTimerCollection;
```

## SoftAssert
Gets or sets soft assert
```csharp
this.SoftAssert = new SeleniumSoftAssert(this);
```

## Values
Gets a dictionary of string key value pairs
```csharp
Dictionary<string, string>
```

## Objects
Gets a dictionary of string key and object value pairs
```csharp
Dictionary<string, object> values = this.Objects = baseTestObject.Objects;
```

## ManagerStore
Gets a dictionary of string key and driver value pairs
```csharp
 public ManagerDictionary ManagerStore
{
    get
    {
        return this.TestObject.ManagerStore;
    }
}
```

## AssociatedFiles
Gets a hash set of unique associated files to attach to the test context
```csharp
this.AssociatedFiles.Add(path);
```

## SetValue
Sets a string value, will replace if the key already exists
```csharp
public void SetValue(string key, string value)
{
    if (this.Values.ContainsKey(key))
    {
        this.Values[key] = value;
    }
    else
    {
        this.Values.Add(key, value);
    }
}

this.TestObject.SetValue("1", "one");
```

## SetObject
Sets an object value, will replace if the key already exists
```csharp
 public void SetObject(string key, object value)
{
    if (this.Objects.ContainsKey(key))
    {
        this.Objects[key] = value;
    }
    else
    {
        this.Objects.Add(key, value);
    }
}

 this.TestObject.SetObject("1", builder);
```

## GetDriverManager
Get a driver manager of the specific type
```csharp
this.TestObject.GetDriverManager<AppiumDriverManager>().Dispose();

public T GetDriverManager<T>() where T : DriverManager
{
    return this.ManagerStore[typeof(T).FullName] as T;
}
```

## AddDriverManager
Add a new driver
```csharp
public void AddDriverManager<T>(T driver, bool overrideIfExists = false) where T : DriverManager
{
    if (overrideIfExists)
    {
        this.OverrideDriverManager(typeof(T).FullName, driver);
    }
    else
    {
        this.AddDriverManager(typeof(T).FullName, driver);
    }
}

this.AddDriverManager(typeof(T).FullName, driver);
```

## AddAssociatedFile
Checks if the file exists and if so attempts to add it to the associated files set
```csharp
bool added = this.TestObject.AddAssociatedFile(@"TeardownTest/FakeFileToAttach1.txt");
```

## RemoveAssociatedFile
Removes the file path from the associated file set
```csharp
bool removed = this.TestObject.RemoveAssociatedFile(logPath);
```

## GetArrayOfAssociatedFiles
Returns an array of the file paths associated with the test object
```csharp
string[] assocFiles = this.TestObject.GetArrayOfAssociatedFiles();
```

## ContainsAssociatedFile
Returns an array of the file paths associated with the test object
```csharp
bool contains = this.TestObject.ContainsAssociatedFile(filePath);
```

## OverrideDriverManager
Override a specific driver
```csharp
this.OverrideDriverManager(typeof(T).FullName, driver);
```

## Dispose
Dispose the driver store
```csharp
this.driver.Dispose();
```