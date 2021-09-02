# <img src="resources/maqslogo.ico" height="32" width="32"> Appium Driver

## Overview
The AppiumDriver object allows you to interact with the mobile driver.

# Available calls
[GetWaitDriver](#GetWaitDriver)  
[RemoveWaitDriver](#RemoveWaitDriver)  
[SetWaitDriver](#SetWaitDriver)  
[WaitForAbsentElement](#WaitForAbsentElement)  
[WaitForPageLoad](#WaitForPageLoad)
[WaitUntilAbsentElement](#WaitUntilAbsentElement)
[WaitUntilPageLoad](#WaitUntilPageLoad)

## GetWaitDriver
Get the WebDriverWait
```csharp
WebDriverWait webDriverWait = this.TestObject.AppiumDriver.GetWaitDriver();
```

## RemoveWaitDriver
Remove the stored wait driver
```csharp
bool remove = this.TestObject.AppiumDriver.RemoveWaitDriver();
```

## SetWaitDriver
Set wait driver
```csharp
this.TestObject.AppiumDriver.SetWaitDriver(driver, webDriverWait);
```

## WaitForAbsentElement
Wait for an element to not appear on the page - It can be gone or just not displayed
```csharp
this.TestObject.AppiumDriver.WaitForAbsentElement(driver, by);
```

## WaitForPageLoad
Wait for the page to load
```csharp
this.TestObject.AppiumDriver.WaitForPageLoad(driver);
```

## WaitUntilAbsentElement
Wait for an element to not appear on the page - It can be gone or just not displayed
```csharp
bool isAbsent = this.TestObject.AppiumDriver.WaitUntilAbsentElement(driver, by);
```

## WaitUntilPageLoad
Wait for the page to load
```csharp
bool isLoaded = this.TestObject.AppiumDriver.WaitUntilPageLoad(driver);
```
