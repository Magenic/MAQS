# <img src="resources/maqslogo.ico" height="32" width="32"> Appium Utilities

## Overview
The AppiumUtilities class is a utility class for working with Appium

# Available methods
[CaptureScreenshot](#CaptureScreenshot)  
[SavePageSource](#SavePageSource)  

##  CaptureScreenshot
Capture a screenshot.  
*By default screenshots ends up in the log folder, but one of the 'CaptureScreenshot' functions allows you to choose a different directory.*
```csharp
string username = "NOT";
string password = "Valid";
LoginPageModel page = new LoginPageModel(this.TestObject);
page.OpenLoginPage();

bool captured = AppiumUtilities.CaptureScreenshot(this.AppiumDriver, this.TestObject, "LoginPage");
```
##  SavePageSource
Capture the page DOM.  
*By default page source ends up in the log folder, but one of the 'SavePageSource' functions allows you to choose a different directory.*
```csharp
string username = "NOT";
string password = "Valid";
LoginPageModel page = new LoginPageModel(this.TestObject);
page.OpenLoginPage();

bool savedSource = AppiumUtilities.SavePageSource(this.AppiumDriver, this.TestObject);

string pageSourcePath = AppiumUtilities.SavePageSource(this.AppiumDriver, this.TestObject, "TempTestDirectory", "TestObjAssoc");
```

##  KillDriver
Make sure an Appium driver gets closed
```csharp
AppiumDriver driver = this.GetAppiumDriver();

try
{
    // Interact with driver
}
finally
{
    driver?.KillDriver();
}
```