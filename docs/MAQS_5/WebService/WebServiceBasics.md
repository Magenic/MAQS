# <img src="resources/maqslogo.ico" height="32" width="32"> Web service test basics

## Overview
MAQS provides support for testing web services.  


## BaseWebServiceTest
BaseWebServiceTest is an abstract test class you can extend.  Extending the class allows you to automatically use MAQS's web service testing capabilities.
```csharp
[TestClass]
public class MyWebServiceTests : BaseWebServiceTest
```

## WebServiceDriver
The WebServiceDriver is an object that allows you to interact with web services.  
This driver wraps common web service interactions, making web service testing relatively easy.  
The driver is also thread safe, which means you can run multiple web service tests in parallel.  
*Information, such as web service base URL is pulled from the MAQS configuration.
```csharp
ProductJson result = this.WebServiceDriver.Get<ProductJson>
```
## Log
There is also logger (also thread safe) the can be used to add log message to your log.
```csharp
this.Log.LogMessage("I am testing with MAQS");
```
## TestObject
The TestObject can be thought of as your test context.  It holds all the MAQS test execution replated data.  This includes the web service driver, logger, soft asserts, performance timers, plus more.
```csharp
ProductXml result = this.TestObject.WebServiceDriver.Get<ProductXml>("/api/XML_JSON/GetProduct/1", "application/xml", false);
this.TestObject.Log.LogMessage("I am testing with MAQS");
```
*Notes:*  
* *Most of the test object objects are already accessible on the test lever. For example **this.Log** and **this.TestObject.Log** both access the same logger.*
* *You seldom what you use the test object directly.  It is usually only used when you want to share your test MAQS context with another piece of code*

## Sample code
```csharp
using Magenic.Maqs.BaseWebServiceTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.WebService;

namespace Tests
{
    /// <summary>
    /// Sample test class
    /// </summary>
    [TestClass]
    public class MyWebServiceTests : BaseWebServiceTest
    {
        /// <summary>
        /// Get single product as Json
        /// </summary>
        [TestMethod]
        public void GetJsonDeserialized()
        {
            ProductJson result = this.WebServiceDriver.Get<ProductJson>("/api/XML_JSON/GetProduct/1", "application/json", false);
            this.Log.LogMessage("I am testing with MAQS");

            Assert.AreEqual(1, result.Id, "Expected to get product 1");
        }
    }
}
```