# <img src="resources/maqslogo.ico" height="32" width="32"> Appium Soft Asserts

## Overview
MAQS provides Soft Assert functionality in Appium testing.

## AreEqual
Soft assert method to check if the files are equal
```csharp
SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.SoftAssertValidTest"));
softAssert.AreEqual("Yes", "Yes", "Utilities Soft Assert", "Message is not equal");
```

## TextToAppend
Method to determine the text to be appended to the screenshot file names
```csharp
AppiumUtilities.CaptureScreenshot(this.appiumTestObject.AppiumDriver, this.appiumTestObject, this.TextToAppend(softAssertName));
```