# <img src="resources/maqslogo.ico" height="32" width="32"> Appium Test Object

## Overview
Takes care of the base test context data.

[AppiumDriver](#AppiumDriver)
[AppiumManager](#AppiumManager)  
[OverrideWebServiceDriver](#OverrideWebServiceDriver)

## AppiumDriver
Gets the web service driver
```csharp
AppiumDriver<IWebElement> driver = this.AppiumManager.GetMobileDriver();
```

## WebServiceManager
Gets the web service driver manager
```csharp
MoblieDriverManager mobileDriver = this.AppiumManager.GetMobileDriver();
```

## OverrideWebServiceDriver
Override the http client
```csharp
public void OverrideWebServiceDriver(HttpClient httpClient)
{
	this.OverrideDriverManager(typeof(MobileDriverManager).FullName, new MobileDriverManager(() => appiumDriver, this));
}
```

Override the http client driver
```csharp
public void OverrideWebServiceDriver(WebServiceDriver webServiceDriver)
{
    this.OverrideDriverManager(typeof(MobileDriverManager).FullName, new MobileDriverManager(appiumDriver, this));
}
```