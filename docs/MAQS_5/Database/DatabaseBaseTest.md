# <img src="resources/maqslogo.ico" height="32" width="32"> BaseDatabaseTest

## Overview
MAQS provides support for testing databases.  

## BaseDatabaseTest
BaseDatabaseTest is an abstract test class you can extend.  Extending the class allows you to automatically use MAQS's database testing capabilities.
```csharp
[TestClass]
public class DatabaseTest : BaseDatabaseTest
```

## DatabaseDriver
The DatabaseDriver is an object that allows you to interact with databases.  
This driver wraps common database interactions, making database testing relatively easy.  
The driver is also thread safe, which means you can run multiple database tests in parallel.  
*Information, such as connection strings are pulled from the MAQS configuration.
```csharp
var table = this.DatabaseDriver.Query("SELECT * FROM information_schema.tables").ToList();
```
## Log
There is also logger (also thread safe) the can be used to add log message to your log.
```csharp
this.Log.LogMessage("I am testing with MAQS");
```
## TestObject
The TestObject can be thought of as your test context.  It holds all the MAQS test execution replated data.  This includes the database driver, logger, soft asserts, performance timers, plus more.
```csharp
var table = this.DatabaseDriver.Query("SELECT * FROM information_schema.tables").ToList();
this.TestObject.Log.LogMessage("I am testing with MAQS");
```
*Notes:*  
* *Most of the test object objects are already accessible on the test lever. For example **this.Log** and **this.TestObject.Log** both access the same logger.*
* *You seldom what you use the test object directly.  It is usually only used when you want to share your test MAQS context with another piece of code*

## Sample code
```csharp
using System.Linq;
using Magenic.Maqs.BaseDatabaseTest;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DatabaseUnitTests
{
    /// <summary>
    /// DatabaseTest test class
    /// </summary>
    [TestClass]
    public class DatabaseTest : BaseDatabaseTest
    {
        /// <summary>
        /// Check that we get back the state table
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyStateTableExists()
        {
            this.TestObject.Log.LogMessage(MessageType.INFORMATION, "Before query");
            var table = this.DatabaseDriver.Query("SELECT * FROM information_schema.tables").ToList();
            this.Log.LogMessage(MessageType.INFORMATION, "After query");

            Assert.IsTrue(table.Any(n => n.TABLE_NAME.Equals("States")));
        }
    }
}
```