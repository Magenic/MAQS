# <img src="resources/maqslogo.ico" height="32" width="32"> Selenium Utilities

## Overview
The SeleniumUtilities class is a utility class for working with Selenium

# Available methods
[CaptureScreenshot](#CaptureScreenshot)  
[SavePageSource](#SavePageSource)  
[KillDriver](#KillDriver)  
[SearchContextToJavaScriptExecutor](#SearchContextToJavaScriptExecutor)  
[SearchContextToWebDriver](#SearchContextToWebDriver)  
[WebElementToWebDriver](#WebElementToWebDriver)  


##  CaptureScreenshot
Capture a screenshot.  
*By default screenshots ends up in the log folder, but one of the 'CaptureScreenshot' functions allows you to choose a different directory.*
```csharp
string username = "NOT";
string password = "Valid";
LoginPageModel page = new LoginPageModel(this.TestObject);
page.OpenLoginPage();

SeleniumUtilities.CaptureScreenshot(this.WebDriver, this.TestObject, "LoginPage");
```
##  SavePageSource
Capture the page DOM.  
*By default page source ends up in the log folder, but one of the 'SavePageSource' functions allows you to choose a different directory.*
```csharp
string username = "NOT";
string password = "Valid";
LoginPageModel page = new LoginPageModel(this.TestObject);
page.OpenLoginPage();

SeleniumUtilities.SavePageSource(this.WebDriver, this.TestObject);
```

##  KillDriver
Make sure a web driver gets closed
```csharp
IWebDriver tempDriver = SeleniumConfig.Browser("HeadlessChrome");

try
{
    //Do something
tempDriver.GetWebDriver().Navigate().GoToUrl("http://magenicautomation.azurewebsites.net/");
}
finally
{
    tempDriver.KillDriver();
}
```

##  SearchContextToJavaScriptExecutor
Get the JavaScript executor from a search context objects like web driver or web element   

### From driver
```csharp
LoginPageModel page = new LoginPageModel(this.TestObject);
page.OpenLoginPage();

IJavaScriptExecutor js = SeleniumUtilities.SearchContextToJavaScriptExecutor(this.WebDriver);
string title = (string)js.ExecuteScript("return document.title");
```

### From element
```csharp
LoginPageModel page = new LoginPageModel(this.TestObject);
page.OpenLoginPage();

IWebElement element = this.WebDriver.FindElement(By.CssSelector("#SELECTOR"));

IJavaScriptExecutor js = SeleniumUtilities.SearchContextToJavaScriptExecutor(element);
string title = (string)js.ExecuteScript("return document.title");
```

##  SearchContextToWebDriver
Get the web driver from a search context objects like web driver or web element  
### From driver
```csharp
LoginPageModel page = new LoginPageModel(this.TestObject);
page.OpenLoginPage();

IWebDriver driver = SeleniumUtilities.SearchContextToWebDriver(this.WebDriver);
```
### From element
```csharp
LoginPageModel page = new LoginPageModel(this.TestObject);
page.OpenLoginPage();

IWebElement element = this.WebDriver.FindElement(By.CssSelector("#SELECTOR"));

IWebDriver driver = SeleniumUtilities.SearchContextToWebDriver(element);
```

##  WebElementToWebDriver
Get the web driver from a web element
```csharp
LoginPageModel page = new LoginPageModel(this.TestObject);
page.OpenLoginPage();

IWebElement element = this.WebDriver.FindElement(By.CssSelector("#SELECTOR"));

IWebDriver driver = SeleniumUtilities.WebElementToWebDriver(element);

```