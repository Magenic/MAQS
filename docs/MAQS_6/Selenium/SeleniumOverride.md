# <img src="resources/maqslogo.ico" height="32" width="32"> Overriding The Selenium WebDriver

## Overriding the WebDriver 
By default, BaseSeleniumTest will create a WebDriver for you based on your [configuration](MAQS_6/Selenium/SeleniumConfig.md). This typically works for most instances, but there are times when the default WebDriver implementation provide by MAQS does not suit your needs. This is why we provide several different ways for you to provide your own WebDriver implementation.

There are three primary ways to override the WebDriver.

### Override the base Selenium test get webdriver function
```csharp
[TestClass]
public class YOURTESTCLASS : BaseSeleniumTest
{
    /// <summary>
    /// Get the WebDriver
    /// </summary>
    /// <returns>The WebDriver</returns>
    protected override IWebDriver GetBrowser()
    {
        return YourNewWebDriverFunction();
    }
```
### Override how to get the driver
```csharp
// Override with a function call
this.TestObject.OverrideWebDriver(YourNewWebDriverFunction);

// Override with a lambda expression
this.TestObject.OverrideWebDriver(() => WebDriverFactory.GetBrowserWithDefaultConfiguration(BrowserType.HeadlessChrome));
```
*_**The above examples do lazy instantiation of the WebDriver - AKA You only create a driver if/when you use it**_  

### Override the driver directly
```csharp
// Override with a driver
IWebDriver overrideDriver = YourNewWebDriverFunction();
this.TestObject.OverrideWebDriver(overrideDriver);

// Override the driver directly 
IWebDriver driver = YourNewWebDriverFunction();
this.WebDriver = driver;
```
*_**Overriding the driver is not advised because it doesn't lazy load the WebDriver and only provides limited logging capabilities**_  
