# <img src="resources/maqslogo.ico" height="32" width="32"> MAQS Dapper

## Overview
MAQS provides support for testing mobile applictions.  	

## BaseTest
BaseAppiumTest is an abstract test class you can extend.  Extending the class allows you to automatically use MAQS's web service testing capabilities.
```csharp
[TestClass]
public class MyDatabaseTests : BaseDatabaseTest
```

## Driver
The DatabaseDriver is an object that allows you to interact with database services.  
This driver wraps common database interactions, making testing relatively easy.  
The driver is also thread safe, which means you can run multiple appium tests in parallel.  
*Information, such as the OS version is pulled from the MAQS configuration.

## DriverManager
Driver that manages the driver manager.

## Log
There is also logger (also thread safe) the can be used to add log message to your log.
```csharp
this.Log.LogMessage("I am testing with MAQS");
```

## TestObject
The TestObject can be thought of as your test context, It holds all the MAQS test execution replated data.  
This includes the database driver, logger, soft asserts, performance timers, plus more.

*Notes:*  
* *Most of the test object objects are already accessible on the test level. For example **this.Log** and **this.TestObject.Log** both access the same logger.*
* *You seldom use the test object directly. It is usually only used when you want to share your test MAQS context with another piece of code*

## Utilities
Stores functions for Capturing screenshots, saving page sources, waiting with the webdriver, and killing the driver.

## Config
Stores methods for interacting with the App.config

## LazyMobileElement
Driver for dynamically finding and interacting with elements

## EventFiringDriver
Wrap basic firing database interactions

## SoftAssert
Enables methods for soft asserts.

## Providers
Manages the provider connections.

## ConnectionFactory
Gets a database connection based on configuration values

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
    /// Check that we get back the state table
    /// </summary>
    [TestMethod]
    [TestCategory(TestCategories.Database)]
    public void VerifyStateTableExistsNoDriver()
    {
        DatabaseDriver driver = new DatabaseDriver();
        var table = driver.Query("SELECT * FROM information_schema.tables");
        Assert.IsTrue(table.Any(n => n.TABLE_NAME.Equals("States")));
    }
}
```