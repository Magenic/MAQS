# <img src="resources/maqslogo.ico" height="32" width="32"> Manager Store

## Overview
The manager store is a collection of test drivers (such as Selenium WebDriver, Database driver, Web service driver, etc.) that can be used in any MAQS test.  The store allows you to add use multiple drivers in the same test.  This means you can have one test with any number of drive, you can also have multiple drivers of the same time.
Not only does the driver store allow you to multiple drivers but it also automatically cleans up all of the managed drivers when your test is done. 
# Available methods
[Add](#Add)  
[AddOrOverride](#AddOrOverride)  
[Get](#Get) 

# See it in practice
[Example](#Example)  

##  Add
Add a new webdriver to the store.  
*Will throw an exception if a default you try to add a duplicate default or named driver.*

### Add default
```csharp
this.ManagerStore.Add(new SeleniumDriverManager(() => SeleniumConfig.Browser(), TestObject));
```

### Add named
```csharp
this.ManagerStore.Add("NAMESEL", new SeleniumDriverManager(() => SeleniumConfig.Browser("HeadlessChrome"), TestObject));
```

##  AddOrOverride
Add or override a new webdriver in the store.  
*Will cleanup and dispose of any drivers that are being replaced.*

### Add or override default
```csharp
this.ManagerStore.Add(new DatabaseDriverManager(() => DatabaseConfig.GetOpenConnection(), TestObject));
```
### Add or override named
```csharp
this.ManagerStore.Add("NAMEDB", new DatabaseDriverManager(() => DatabaseConfig.GetOpenConnection(), TestObject));
```

##  Get
Get the driver from the store
### Default driver
```csharp
// All different ways of getting the same WebDriver
IWebDriver defaultDriver = this.ManagerStore.GetDriver<SeleniumDriverManager, SeleniumDriverManager>().GetWebDriver();
IWebDriver alsoDefaultDriver = this.TestObject.GetDriverManager<SeleniumDriverManager>().GetWebDriver();
IWebDriver alsoAlsoDefaultDriver = this.WebDriver;
``` 
*The this.WebDriver is only available when if you are creating base Selenium tests.*
### Named driver
```csharp
IWebDriver selenNamed = ((SeleniumDriverManager)this.ManagerStore["NAMESEL"]).GetWebDriver();
IWebDriver alsoSelenNamed = this.ManagerStore.GetDriver<SeleniumDriverManager>("NAMESEL").GetWebDriver();

DatabaseDriver dbNamed = ((DatabaseDriverManager)this.ManagerStore["NAMEDB"]).GetDatabaseDriver();
DatabaseDriver alsoDBNamed = this.ManagerStore.GetDriver<DatabaseDriverManager>("NAMEDB").GetDatabaseDriver();
```

# Example
```csharp
using Magenic.Maqs.BaseDatabaseTest;
using Magenic.Maqs.BaseSeleniumTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Tests
{
    /// <summary>
    /// SeleniumTest test class
    /// </summary>
    [TestClass]
    public class SeleniumTest : BaseSeleniumTest
    {
        [TestInitialize]
        public void SetupStore()
        {
            // Add database driver
            this.ManagerStore.Add(new DatabaseDriverManager(() => DatabaseConfig.GetOpenConnection(), TestObject));

            // Add named WebService driver
            this.ManagerStore.Add("NAMESEL", new SeleniumDriverManager(() => SeleniumConfig.Browser("HeadlessChrome"), TestObject));
        }

        [TestMethod]
        public void CompoundTest()
        {
            this.WebDriver.Navigate().GoToUrl(SeleniumConfig.GetWebSiteBase());

            // Get the other named driver from the store
            IWebDriver namedWebDriver = this.ManagerStore.GetDriver<SeleniumDriverManager>("NAMESEL").GetWebDriver();
            namedWebDriver.Navigate().GoToUrl(SeleniumConfig.GetWebSiteBase());

            Assert.AreEqual(this.WebDriver.Title, namedWebDriver.Title, "Expect page to have the same title");

            // Get the default database driver from the store
            DatabaseDriver databaseDriver = this.TestObject.GetDriverManager<DatabaseDriverManager>().GetDatabaseDriver();

            string query = @"UPDATE States SET WasRun = 'Yes' WHERE WasRun = 'No'";
            int result = databaseDriver.Execute(query);

            Assert.AreEqual(1, result, "Expected 1 update.");
        }
    }
}

```