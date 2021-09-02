# <img src="resources/maqslogo.ico" height="32" width="32"> Overriding The Appium driver

## Overriding the Appium driver 
By default, BaseAppiumTest will create a driver for you based on your [configuration](MAQS_6/Appium/AppiumConfig.md). This typically works for most instances, but there are times when the default driver implementation provide by MAQS does not suit your needs. This is why we provide several different ways for you to provide your own driver implementation.

There are three primary ways to override the driver.

### Override the base Appium test get driver function
```csharp
[TestClass]
public class YOURTESTCLASS : BaseAppiumTest
{
    /// <summary>
    /// Get the Appium driver
    /// </summary>
    /// <returns>Appium Driver</returns>
    protected override AppiumDriver<IWebElement> GetMobileDevice()
    {
        AppiumOptions options = new AppiumOptions();

        options.AddAdditionalCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
        options.AddAdditionalCapability(MobileCapabilityType.Udid, "0C0E26E7-966B-4C89-A765-32C5C997A456");
        return new WindowsDriver<IWebElement>(new Uri("http://127.0.0.1:4723"), options);
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
