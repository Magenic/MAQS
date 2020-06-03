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
[CheckAccessibility](#CheckAccessibility)  
[GetReadableAxeResults](#GetReadableAxeResults) 


[CreateAccessibilityHtmlReport](#CreateAccessibilityHtmlReport) 


##  CaptureScreenshot
Capture a screenshot.  
*By default screenshots ends up in the log folder, but one of the 'CaptureScreenshot' functions allows you to choose a different directory.*
```csharp
string username = "NOT";
string password = "Valid";
LoginPageModel page = new LoginPageModel(this.TestObject);
page.OpenLoginPage();

SeleniumUtilities.CaptureScreenshot(this.WebDriver, this.TestObject, "LoginPage");
// or
this.WebDriver.CaptureScreenshot(this.TestObject, "LoginPage");
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
// or
this.WebDriver.SavePageSource(this.TestObject);
```

##  KillDriver
Make sure a web driver gets closed
```csharp
IWebDriver tempDriver = WebDriverFactory.GetBrowserWithDefaultConfiguration(BrowserType.HeadlessChrome);

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

##  CheckAccessibility
Run an accessibility check and log results to the logger
```csharp
SeleniumUtilities.CheckAccessibility(this.TestObject);
```

Run an accessibility check, log results to the logger and throw an exception if there are any accessibility violations
```csharp
SeleniumUtilities.CheckAccessibility(this.TestObject, true);
```
Run an accessibility check for a given web driver/logger combination, log results to the logger and throw an exception if there are any accessibility violations
```csharp
SeleniumUtilities.CheckAccessibility(webDriver, fileLogger, true);
```

##  GetReadableAxeResults
Get a readable accessibility analysis as a string
```csharp
SeleniumUtilities.GetReadableAxeResults("Violations", WebDriver, WebDriver.Analyze().Violations, out string messages);
Console.WriteLine(messages);
```
## CreateAccessibilityHtmlReport
Run an accessibility check and create a standalone HTML report. The standard MAQS log will provide a link to the produced report.
*The report ends up in the log folder.*
```csharp
// Report on entire page
SeleniumUtilities.CreateAccessibilityHtmlReport(WebDriver, this.TestObject);
```
```csharp
// Report on entire page and throw exception if any violations are found
SeleniumUtilities.CreateAccessibilityHtmlReport(WebDriver, this.TestObject, true);
```
```csharp
// Report on specific lazy element and it's children
LazyElement foodTable = new LazyElement(this.TestObject, By.Id("FoodTable"));
SeleniumUtilities.CreateAccessibilityHtmlReport(WebDriver, this.TestObject, foodTable);
```
```csharp
// Report on specific element and it's children
SeleniumUtilities.CreateAccessibilityHtmlReport(WebDriver, this.TestObject, WebDriver.FindElement(By.Id("FoodTable")));
```
```csharp
// Report with user defined rules
var builder = new AxeBuilder(WebDriver).DisableRules("color-contrast");           
SeleniumUtilities.CreateAccessibilityHtmlReport(WebDriver, this.TestObject, () => builder.Analyze());
```