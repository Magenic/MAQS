# <img src="resources/maqslogo.ico" height="32" width="32"> Email Basics

## Overview
MAQS provides support for testing Email.  

## BaseEmailTest
BaseEmailTest is an abstract test class you can extend.  Extending the class allows you to automatically use MAQS's email testing capabilities.
```csharp
[TestClass]
public class MyBaseEmailTest : BaseEmailTest
```

## EmailDriver
The EmailDriver is an object that allows you to interact with an email account using and IMAP connection.  
This driver wraps common email interactions.  
The driver is also thread safe, which means you can run multiple email tests in parallel.  
*Information, such as the email host and port is pulled from the [MAQS configuration](MAQS_7/Email/EmailConfig.md). 
```csharp
EmailDriver driver = new EmailDriver(() => ClientFactory.GetDefaultEmailClient());
```

## Log
There is also logger (also thread safe) the can be used to add log message to your log.
```csharp
this.Log.LogMessage("I am testing with MAQS");
```

## EmailTestObject
The TestObject can be thought of as your test context.  It holds all the MAQS test execution replated data.  This includes the Email driver, logger, soft asserts, performance timers, plus more.
```csharp
bool connected = this.EmailDriver.CanAccessEmailAccount();
this.TestObject.Log.LogMessage("I am testing with MAQS");
```
*Notes:*  
* *Most of the test object objects are already accessible on the test level. For example **this.Log** and **this.TestObject.Log** both access the same logger.*
* *You seldom use the test object directly. It is usually only used when you want to share your test MAQS context with another piece of code*

## Sample code
```csharp
using Magenic.Maqs.BaseEmailTest;
using Magenic.Maqs.Utilities.Helper;
using MailKit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit;
using System;

namespace EmailUnitTests
{
    /// <summary>
    /// Test basic email testing with the base email test
    /// </summary>
    [TestClass]
    public class EmailUnitWithDriver : BaseEmailTest
    {
        /// <summary>
        /// Verify the user can access account
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void CanAccessEmail()
        {
            Assert.IsTrue(this.EmailDriver.CanAccessEmailAccount(), "Email account was not accessible");
        }
    }
}
```