# <img src="resources/maqslogo.ico" height="32" width="32"> MongoDB Features

## Overview
MAQS provides support for testing MongoDB databases. 

## Base Test
BaseMongoTest is an abstract test class you can extend.  Extending the class allows you to automatically use MAQS's web service testing capabilities.
```csharp
[TestClass]
public class MyMongoDBTests : BaseMongoTest
```

## TestObject
The TestObject can be thought of as your test context, it holds all the MAQS test execution replated data.  
This includes the MongoDB driver, logger, soft asserts, performance timers, plus more.

*Notes:*  
* *Most of the test object objects are already accessible on the test level. For example **this.Log** and **this.TestObject.Log** both access the same logger.*
* *You seldom use the test object directly. It is usually only used when you want to share your test MAQS context with another piece of code*

## Driver
The MongoDBDriver is an object that allows you to interact with MongoDB services.  
This driver wraps common MongoDB interactions, making testing relatively easy.  
The driver is also thread safe, which means you can run multiple tests in parallel.  
*Information, such as the OS version is pulled from the MAQS configuration.
```csharp
List<BsonDocument> collectionItems = this.MongoDBDriver.ListAllCollectionItems();
```

## Driver Manager
Manages the MongoDB driver.

## Config
Config class for MongoDB properties.

## Event Firing Driver
A driver to Wrap basic firing database interactionss

## Log
There is also logger (also thread safe) the can be used to add log message to your log.
```csharp
this.Log.LogMessage("I am testing with MAQS");
```

## Sample code
```csharp
using Magenic.Maqs.BaseAppiumTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Tests
{
    /// <summary>
    /// Test for creating Mobile Device driver
    /// </summary>
    [TestMethod]
    [TestCategory(TestCategories.Database)]
    public void GetDatabaseConnectionStringTest()
    {
        string connection = MongoDBConfig.GetConnectionString();
        Assert.AreEqual("mongodb://localhost:27017", connection);
    }
}
```