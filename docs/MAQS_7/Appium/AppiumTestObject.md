# <img src="resources/maqslogo.ico" height="32" width="32"> Appium Test Object

## Overview
Takes care of the base test context data.

[AppiumManager](#AppiumManager)  
[AppiumDriver](#AppiumDriver)  
[OverrideAppiumDriver](#OverrideAppiumDriver)  
[OverrideDriverManager](#OverrideDriverManager)

## AppiumManager
Gets the Appium driver manager
```csharp
AppiumDriverManager manager = this.TestObject.AppiumManager;
```

## AppiumDriver
Gets the Appium driver
```csharp
// Pull from the manager
AppiumDriver driverViaManager = this.TestObject.AppiumManager.GetAppiumDriver();

// Indirectly pull form the manager
AppiumDriver driverViaManagerIndirect = this.AppiumDriver;
```

## OverrideAppiumDriver
Override the entire driver manager
```csharp
// Override how we get the driver
this.TestObject.OverrideAppiumDriver(AppiumDriverFactory.GetDefaultMobileDriver);

// Override the driver with one that is already initialized 
AppiumDriver driver = AppiumDriverFactory.GetDefaultMobileDriver();
this.TestObject.OverrideAppiumDriver(driver);
```
*_**Overriding how to get a driver is preferable as it can be lazy loaded**_  

## OverrideDriverManager
Override the entire driver manager
```csharp
this.TestObject.OverrideDriverManager(new AppiumDriverManager(() => AppiumDriverFactory.GetDefaultMobileDriver(), this.TestObject));
```
