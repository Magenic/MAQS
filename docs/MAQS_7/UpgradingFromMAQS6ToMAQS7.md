# Updating from MAQS 6 to MAQS 7

## Deprecated MAQS 6 features have been removed
### SoftAssert
Soft asserts have dropped direct assert checks.  
This means the "SoftAssert.AreEqual", "SoftAssert.IsTrue", and "SoftAssert.IsFalse" functions no longer exist.  

Here is how to do these checks in MAQS 7:
``` csharp
this.SoftAssert.Assert(() => Assert.AreEqual("Expected", "Actual"));
this.SoftAssert.Assert(() => Assert.IsTrue(true, "Was not true"));
this.SoftAssert.Assert(() => Assert.IsFalse(false, "Was not false"));
```
*The benefit of this implementation is that allows end user to use SoftAsserts with whatever assertion library (or libraries) they want.*

### PerfTimerCollection
PerfTimerCollection have dropped EndTimer.  
The new name is StopTimer.
``` csharp
// Old
this.PerfTimerCollection.StartTimer("testTimer");
this.PerfTimerCollection.EndTimer("testTimer");

// New
this.PerfTimerCollection.StartTimer("testTimer");
this.PerfTimerCollection.StopTimer("testTimer");
```
## Let there be interfaces
MAQS 7 implements many more interface.  
This is meant to make MAQS far more extendable.

### What will I need to change
For the most part, interface related changes should not break existing MAQS 6 base code.   
*The big exceptions are with the BaseSeleniumPageModel and BaseAppiumPageModel implement.  
These classes will need to be updated to take an ISeleniumTestObject or IAppiumTestObject.*
``` csharp
// Old implementation
public LoginPageModel(SeleniumTestObject testObject) : base(testObject)
{
}

// New implementation
public LoginPageModel(ISeleniumTestObject testObject) : base(testObject)
{
}
```

## Use WebDriverManager  
WebDriverManager now handles fetching OS specific WebDrivers.  
This means you can/should remove the following references:
``` xml
<PackageReference Include="Selenium.WebDriver.ChromeDriver" Version="#.#.#.#" />
<PackageReference Include="Selenium.WebDriver.GeckoDriver.Win64" Version="#.#.#" />
<PackageReference Include="Selenium.WebDriver.IEDriver" Version="#.#.#.#" />
```

## Move to Selenium 4
Selenium 4 has numerous updates, full details can be found here:
https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/CHANGELOG

### Capabilities
Selenium 4 fundamentally changes how we driver capabilities work.  
For the most part this will not be an use for most users. The exceptions are those that provide their own driver initialization implementations and those using Sauce Labs or BrowsersStack.

Those provide their own driver initialization implementations will find the "AddAdditionalCapability" is being deprecated in favor of platform specific capabilities.  
For example:
``` csharp 
// This
chromeOptions.AddAdditionalCapability(keyValue.Key, keyValue.Value, true);

// Becomes this
chromeOptions.AddAdditionalChromeOption(keyValue.Key, keyValue.Value);
```
When it comes to thing like Sauce Labs and BrowserStack the format for how their options have changes.  This means we have to change the format in our configuration.   
Old format:
``` xml
  <RemoteSeleniumCapsMaqs>
    <add key="username" value="Sauce_Labs_Username" />
    <add key="accessKey" value="Sauce_Labs_Accesskey" />
  </RemoteSeleniumCapsMaqs>
```
New format:
``` xml
  <RemoteSeleniumCapsMaqs>
    <add key="sauce:options" value="{username: 'SAUCE_NAME', accessKey:  'SAUCE_KEY' }"  />
  </RemoteSeleniumCapsMaqs>
```

## Appium 5 Related Updates
Appium 5 has numerous updates to work with Selenium 4.  
For full details review Appium 5 changes in: https://github.com/appium/appium-dotnet-driver

### Appium Driver Signature
The Appium drivers signature no longer takes a generic type.
``` csharp
// Old
 var driver = new AndroidDriver<IWebElement>(mobileHub, options, timeout);

// New
var driver = new AndroidDriver(mobileHub, options, timeout);
```

### FindElement(s)
All FindElementBy***TYPE*** and FindElementsBy***TYPE*** functions have been removed, they have been replaced with MobileBy.
``` csharp
// Old
AppiumDriver.FindElementByName("One").Click();

// New
AppiumDriver.FindElement(MobileBy.Name("One")).Click();
```

### By
Appium used to override the Name, Id, ClassName and TagName 'By' selectors. 
This is no longer the case and user will need to transition to 'MobileBy' selectors.

``` csharp
// Old
AppiumDriver.FindElement(By.Name("NAME")).Click();
AppiumDriver.FindElement(By.Id("ID")).Click();
AppiumDriver.FindElement(By.ClassName("CLASS_NAME")).Click();
AppiumDriver.FindElement(By.TagName("TAG_NAME")).Click();

// New
AppiumDriver.FindElement(MobileBy.Name("NAME")).Click();
AppiumDriver.FindElement(MobileBy.Id("ID")).Click();
AppiumDriver.FindElement(MobileBy.ClassName("CLASS_NAME")).Click();
AppiumDriver.FindElement(MobileBy.TagName("TAG_NAME")).Click();
```
**Under the hood Selenium auto-generates CSS selectors for many of these selectors.  This means By.Id("ID") and By.CssSelector("#ID") send the exact same find request to a web driver. The Appium driver however does not support CSS selectors so using the Selenium 'By' is some contexts will results cause an error.*

### Capabilities
The Selenium 4 capability changes also affect Appium.
Here is how it would change use provided driver initialization implementations:

``` csharp 
// This
AppiumOptions options = new AppiumOptions();

options.AddAdditionalCapability("deviceName", "iPhone 8 Simulator");
options.AddAdditionalCapability("platformVersion", "12.2");
options.AddAdditionalCapability("platformName", "iOS");
options.AddAdditionalCapability("browserName", "Safari");
options.AddAdditionalCapability("username", Config.GetValueForSection(ConfigSection.AppiumCapsMaqs, "userName"));
options.AddAdditionalCapability("accessKey", Config.GetValueForSection(ConfigSection.AppiumCapsMaqs, "accessKey"));

// Becomes this
AppiumOptions options = new AppiumOptions
{
    DeviceName = "iPhone 8 Simulator",
    PlatformVersion = "12.2",
    PlatformName = "iOS",
    BrowserName = "Safari"
};

var sauceCreds = Config.GetValueForSection(ConfigSection.AppiumCapsMaqs, "sauce:options");
options.AddAdditionalAppiumOption("sauce:options", JsonConvert.DeserializeObject<Dictionary<string, string>>(sauceCreds));
```
For Sauce Labs and BrowserStack configuration changes would look like this:  
Old format:
``` xml 
  <AppiumMaqs>
    <!-- Device settings -->
    <add key="PlatformName" value="Android" />
    <add key="PlatformVersion" value="6.0" />

    <!-- All other MAQS Appium setting
    OTHER STUFF 
    -->
  <AppiumCapsMaqs>
    <add key="username" value="SAUCE_NAME" />
    <add key="accessKey" value="SAUCE_KEY" />
    <add key="deviceName" value="Android Emulator" />
    <add key="deviceOrientation" value="portrait" />
    <add key="browserName" value="Chrome" />
    <add key="appiumVersion" value="1.20.2" />
  </AppiumCapsMaqs>
```
New format:
``` xml 

  <AppiumMaqs>
    <!-- Device settings -->
    <add key="PlatformName" value="Android" />
    <add key="PlatformVersion" value="6.0" />
    <add key="DeviceName" value="Android Emulator" />
    <add key="BrowserName" value="Chrome" />

    <!-- All other MAQS Appium setting
    OTHER STUFF 
    -->
  </AppiumMaqs>
  <AppiumCapsMaqs>
    <add key="sauce:options" value="{username:'S_NAME', accessKey:'S_KEY', appiumVersion:'1.20.2', orientation:'portrait' }"  />
  </AppiumCapsMaqs>
```
