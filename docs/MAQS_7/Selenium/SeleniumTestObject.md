# <img src="resources/maqslogo.ico" height="32" width="32"> Selenium Test Object

## Overview
Selenium test context data

## WebManager
Gets the Selenium driver manager
```csharp
SeleniumDriverManager manager = this.WebManager.GetWebDriver();
```

## WebDriver
Gets the Selenium web driver
```csharp
public IWebDriver WebDriver
{
    get
    {
        return this.WebManager.GetWebDriver();
    }
}
```

## OverrideWebDriver
Override the Selenium web driver
```csharp
public void OverrideWebDriver(IWebDriver webDriver)
{
    this.OverrideDriverManager(typeof(SeleniumDriverManager).FullName, new SeleniumDriverManager(() => webDriver, this));
}
```

Override the function for creating a Selenium web driver
```csharp
public void OverrideWebDriver(Func<IWebDriver> getDriver)
{
    this.OverrideDriverManager(typeof(SeleniumDriverManager).FullName, new SeleniumDriverManager(getDriver, this));
}
```
