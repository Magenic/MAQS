# <img src="resources/maqslogo.ico" height="32" width="32"> Selenium test basics

## Overview
MAQS provides support for testing web application.  


## BaseSeleniumTest
BaseSeleniumTest is an abstract test class you can extend.  Extending the class allows you to automatically use MAQS's web application testing capabilities.
```csharp
[TestClass]
public class MySeleniumTest : BaseSeleniumTest
```

## WebDriver
The WebDriver is an object that allows you to interact with web pages. MAQS extends the Selenium WebDriver, but does wrap it. This means all native Selenium functionality is available via the WebDriver. Also note that the driver is thread safe, which means you can run multiple web tests in parallel.   
*Notes:*
* Some browsers, such as IE and Edge, cannot be run in parallel on the same machine.  
* When logging is enabled MAQS automatically creates EventFiringWebDrivers instead of a standard WebDrivers. So be warned that your WebDriver may actually be an EventFiringWebDrivers.
* For more info on the Selenium driver you can visit the Selenium GitHub page: https://github.com/SeleniumHQ/selenium/tree/master/dotnet.  *

## Configuration 
Information, such as the type of browser and website base url are pulled from the SeleniumMaqs section your configuration.
```csharp
 this.WebDriver.Navigate().GoToUrl(SeleniumConfig.GetWebSiteBase());
```
## Log
There is also logger (also thread safe) the can be used to add log message to your log.
```csharp
this.Log.LogMessage("I am testing with MAQS");
```
## TestObject
The TestObject can be thought of as your test context.  It holds all the MAQS test execution replated data.  This includes the Selenium driver, logger, soft asserts, performance timers, plus more.
```csharp
this.TestObject.WebDriver.Navigate().GoToUrl("http://magenicautomation.azurewebsites.net/");
this.TestObject.Log.LogMessage("I am testing with MAQS");
```
*Notes:*  
* *Most of the test object objects are already accessible on the test lever. For example **this.Log** and **this.TestObject.Log** both access the same logger.*
* *You seldom what you use the test object directly.  It is usually only used when you want to share your test MAQS context with another piece of code*

## Sample code
```csharp
using Magenic.Maqs.BaseSeleniumTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    /// <summary>
    /// SeleniumTest test class
    /// </summary>
    [TestClass]
    public class SeleniumTest : BaseSeleniumTest
    {
        [TestMethod]
        public void HomePageTitle()
        {
            this.WebDriver.Navigate().GoToUrl(SeleniumConfig.GetWebSiteBase());
            this.Log.LogMessage("I am testing with MAQS");
            Assert.AreEqual("HOME", this.WebDriver.Title);
        }
    }
}
```