# <img src="resources/maqslogo.ico" height="32" width="32"> BaseAppiumTest
The BaseAppiumTest class provides access to the AppiumTestObject and AppiumDriver.

## Overview
The BaseAppiumTest has methods that sets up the webdriver, gets the mobile driver, tears down the appium driver, and creates a new test object. 

# Available calls
[GetMobileDevice](#GetMobileDevice)  
[BeforeLoggingTeardown](#BeforeLoggingTeardown)  
[CreateNewTestObject](#CreateNewTestObject)  

## GetMobileDevice
This method gets the defult Appium/Mobile driver. 
```csharp
protected virtual AppiumDriver<IWebElement> GetMobileDevice()
{
    return AppiumDriverFactory.GetDefaultMobileDriver();
}
```

## BeforeLoggingTeardown
Takes a screen shot if needed and tear down the appium driver. It is called during teardown.
```csharp
this.BeforeLoggingTeardown(resultType);
```

## CreateNewTestObject
This method creates a new Appium test object based on the mobile device.
```csharp
 protected override void CreateNewTestObject()
{
    this.TestObject = new AppiumTestObject(() => this.GetMobileDevice(), this.CreateLogger(), this.GetFullyQualifiedTestClassName());
}

this.CreateNewTestObject();
```