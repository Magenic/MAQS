# <img src="resources/maqslogo.ico" height="32" width="32"> Base Basics

## Overview
MAQS provides support for testing, base is the foundation for this support.  

## BaseTest
 Base for tests without a defined system under test
 ```csharp
  MaqsBase basetest = new MaqsBase();
 ```

## ExtenableTest
Base code that extends the base test objects for Selenium, Appium, WebServices, Database, Email, and MongoDB drivers or connections.

## TestObject
Base test context data
```csharp
BaseTestObject baseTestObject = new BaseTestObject(new ConsoleLogger(), string.Empty);
```

## DriverManager
Base driver manager object
```csharp
DriverManager managerToKeep = GetManager();
```

## ManagerDictionary
 Driver manager dictionary allows you to get, add, override, remove, clears, and disposes the driver. 
 ```csharp
 this.ManagerStore = new ManagerDictionary();
 ```

## SoftAssert
SoftAssert class allows tests to continue running after low priority steps fail.
```csharp
 this.SoftAssert = new SoftAssert(this.Log);
```

## SoftAssertException
Soft assert exceptions to catch any encountered exceptions while using Soft Asserts.
```csharp
 throw new SoftAssertException(StringProcessor.SafeFormatter("SoftAssert.IsFalse failed for: {0}", softAssertName));
```