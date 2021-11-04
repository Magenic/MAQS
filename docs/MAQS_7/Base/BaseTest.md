# <img src="resources/maqslogo.ico" height="32" width="32"> Base Test
The BaseTest class provides access to the TestObject and DriverManager.

## Overview
The Base for tests without a defined system under test

# Available calls
[PerfTimerCollection](#PerfTimerCollection)  
[SoftAssert](#SoftAssert)  
[Log](#Log)  
[LoggedExceptionList](#LoggedExceptionList)  
[TestContext](#TestContext)  
[TestObject](#TestObject)  
[ManagerStore](#ManagerStore)  
[BaseTestObjects](#BaseTestObjects)  
[LoggingEnabledSetting](#LoggingEnabledSetting)  
[LoggedExceptions](#LoggedExceptions)  
[Setup](#Setup)  
[Teardown](#Teardown)  
[CreateLogger](#CreateLogger)  
[GetFullyQualifiedTestClassName](#GetFullyQualifiedTestClassName)  
[GetResultType](#GetResultType)  
[GetResultText](#GetResultText)  
[TryToLog](#TryToLog)  
[CreateNewTestObject](#CreateNewTestObject)  
[BeforeLoggingTeardown](#BeforeLoggingTeardown)  
[GetFullyQualifiedTestClassNameVS](#GetFullyQualifiedTestClassNameVS)  
[FirstChanceHandler](#FirstChanceHandler)  
[GetResultTypeVS](#GetResultTypeVS)  
[GetResultTextVS](#GetResultTextVS)  
[GetFullyQualifiedTestClassNameNunit](#GetFullyQualifiedTestClassNameNunit)  
[GetResultTypeNunit](#GetResultTypeNunit)  
[GetResultTextNunit](#GetResultTextNunit)  

## PerfTimerCollection
Gets or sets the performance timer collection for a test
```csharp
 PerfTimerCollection p = this.PerfTimerCollection;
```

## SoftAssert
Takes a screen shot if needed and tear down the appium driver. It is called during teardown.
```csharp
this.BeforeLoggingTeardown(resultType);
```

## SoftAssert
Gets or sets the SoftAssert objects.
```csharp
SoftAssert.AreEqual("Automation - Magenic Automation Test Site", WebDriver.Title, "Title Test", "Title is incorrect");
```

## Log
Gets or sets the testing object logger.
```csharp
this.Log = new FileLogger();
```

## LoggedExceptionList
Gets or sets the testing object logged exceptions
```csharp
var messages = this.LoggedExceptionList;
```

## TestContext
Gets or sets the Visual Studio Test Context
```csharp
BaseTest tester = this.GetBaseTest();
tester.TestContext = this.TestContext;
```

## TestObject
Gets or sets the test object
```csharp
Assert.IsNotNull(this.TestObject);
```

## ManagerStore
Gets the driver store of the Manager Dictionary.
```csharp
this.ManagerStore.Add("test", newDriver);
```

## BaseTestObjects
Gets or sets the BaseContext objects.
```csharp
this.BaseTestObjects = new ConcurrentDictionary<string, BaseTestObject>();
```

## LoggingEnabledSetting
Gets the logging enable flag
```csharp
this.LoggingEnabledSetting = LoggingConfig.GetLoggingEnabledSetting();
```

## LoggedExceptions
Gets or sets the logged exceptions.
```csharp
this.LoggedExceptions = new ConcurrentDictionary<string, List<string>>();
```

## Setup
Setup before a test.
```csharp
BaseTest tester = this.GetBaseTest();
tester.Setup();
```

## Teardown
Tear down after a test.
```csharp
BaseTest tester = this.GetBaseTest();
 tester.Teardown();
```

## CreateLogger
Create a logger
```csharp
Logger newLogger = this.CreateLogger();
```

## GetFullyQualifiedTestClassName
Get the fully qualified test name
```csharp
string className = this.GetFullyQualifiedTestClassName(),
```

## GetResultType
Get the type of test result
```csharp
TestResultType resultType = this.GetResultType();
```

## GetResultText
Get the test result type as text
```csharp
string resultText = this.GetResultText()
```

## TryToLog
Try to log a message - Do not fail if the message is not logged


## CreateNewTestObject
Create a Selenium test object
```csharp
this.CreateNewTestObject()
```

## BeforeLoggingTeardown
Steps to do before logging teardown results - If not override nothing is done before logging the results
```csharp
this.BeforeLoggingTeardown(resultType);
```

## GetFullyQualifiedTestClassNameVS
Get the fully qualified test name
```csharp
string className = this.GetFullyQualifiedTestClassNameVS();
```

## FirstChanceHandler
Listen for any thrown exceptions
```csharp
AppDomain.CurrentDomain.FirstChanceException += this.FirstChanceHandler;
```

## GetResultTypeVS
Get the type of test result
```csharp
TestResultType result = this.GetResultTypeVS();
```

## GetResultTextVS
Get the test result type as text
```csharp
string resultText = this.GetResultTextVS();
```

## TryToLog
Try to log a message - Do not fail if the message is not logged
```csharp
this.TryToLog(MessageType.SUCCESS, "Test passed");
```

## LogVerbose
Log a verbose message and include the automation specific call stack data
```csharp

```

## CreateNewTestObject
Create a Selenium test object
```csharp
this.CreateNewTestObject();
```

## BeforeLoggingTeardown
Steps to do before logging teardown results - If not override nothing is done before logging the results
```csharp
this.BeforeLoggingTeardown(resultType);
```

## GetFullyQualifiedTestClassNameVS
Get the fully qualified test name
```csharp
string className = this.GetFullyQualifiedTestClassNameVS();
```

## FirstChanceHandler
Listen for any thrown exceptions
```csharp
AppDomain.CurrentDomain.FirstChanceException += this.FirstChanceHandler;
```

## GetResultTypeVS
Get the type of test result
```csharp
TestResultType result = GetResultTypeVS();
```

## GetResultTextVS
Get the test result type as text
```csharp
string text = GetResultTextVS();
```
## GetFullyQualifiedTestClassNameNunit
Get the fully qualified test name
```csharp
string className = GetFullyQualifiedTestClassNameNunit();
```

## GetResultTypeNunit
Get the type of test result
```csharp
TestResultType type = GetResultTypeNunit();
```

## GetResultTextNunit
Get the test result type as text
```csharp
string text = GetResultTextNunit();
```

## AttachAssociatedFiles
For VS unit tests attach the all of the files in the associated files set if they exist, else write to log
```csharp
AttachAssociatedFiles();
```