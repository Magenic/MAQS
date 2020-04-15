# <img src="resources/maqslogo.ico" height="32" width="32"> Base Extendable Test

## Overview
The Base Extendable Test is the base code for classes that work with test objects (web drivers, Database connections).

[TestObject](#TestObject)  
[Setup](#Setup)

## TestObject
This method gets or sets the test object.
```csharp
protected new T TestObject
{
    get
    {
        return (T)base.TestObject;
    }

    set
    {
        this.BaseTestObjects.AddOrUpdate(this.GetFullyQualifiedTestClassName(), value, (oldkey, oldvalue) => value);
    }
}
```

## Setup
The setup before a test
```csharp
public new void Setup()
{
    // Do base generic setup
    base.Setup();
}

public void Setup()
{
    // Only create a test object if one doesn't exist
    if (!this.BaseTestObjects.ContainsKey(this.GetFullyQualifiedTestClassName()))
    {
        // Create the test object
        this.CreateNewTestObject();
    }
}
```