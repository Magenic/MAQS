# <img src="resources/maqslogo.ico" height="32" width="32"> Overriding The Appium driver

## Overriding the Appium driver 
By default, BaseAppiumTest will create a driver for you based on your [configuration](MAQS_7/Appium/AppiumConfig.md). This typically works for most instances, but there are times when the default driver implementation provide by MAQS does not suit your needs. This is why we provide several different ways for you to provide your own driver implementation.

There are three primary ways to override the driver.

### Override the base Appium test get driver function
```csharp
[TestClass]
public class YOURTESTCLASS : BaseAppiumTest
{
        /// <summary>
        /// Sets capabilities for testing the Windows application driver creation
        /// </summary>
        /// <returns>Windows application driver instance of the Appium Driver</returns>
        protected override AppiumDriver GetMobileDevice()
        {
            AppiumOptions options = new AppiumOptions();
            options.App = $"{Environment.SystemDirectory}\\notepad.exe";
            return AppiumDriverFactory.GetWindowsDriver(new Uri("http://127.0.0.1:4723/wd/hub"), options, TimeSpan.FromSeconds(30));
        }
```
*_**The above example does lazy instantiation of the driver - AKA You only create a driver if/when you use it**_  

### Override how to get the driver
```csharp
// Override with a function call
this.TestObject.OverrideAppiumDriver(YourNewdriverFunction);

// Override with a lambda expression
this.TestObject.OverrideAppiumDriver(() => YourNewdriverFunction());
```
*_**The above examples do lazy instantiation of the driver - AKA You only create a driver if/when you use it**_  

### Override the driver directly
```csharp
// Override with a driver
var driver = YourNewdriverFunction();
this.TestObject.OverrideAppiumDriver(driver);

// Override the driver directly 
var anotherDriver = YourNewdriverFunction();
this.driver = anotherDriver;
```
*_**Overriding the driver is not advised because it doesn't lazy load the driver and only provides limited logging capabilities**_  
