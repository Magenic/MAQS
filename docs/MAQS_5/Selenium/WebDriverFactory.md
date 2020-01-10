# <img src="resources/maqslogo.ico" height="32" width="32"> WebDriver Factory

## Overview
A Static web driver factory that deals with browser options and configurations

[GetDefaultBrowser](#GetDefaultBrowser) 
[GetBrowserWithDefaultConfiguration](#GetBrowserWithDefaultConfiguration) 
[GetDefaultChromeOptions](#GetDefaultChromeOptions) 
[GetDefaultHeadlessChromeOptions](#GetDefaultHeadlessChromeOptions) 
[GetDefaultIEOptions](#GetDefaultIEOptions) 
[GetDefaultFirefoxOptions](#GetDefaultFirefoxOptions) 
[GetDefaultEdgeOptions](#GetDefaultEdgeOptions) 
[GetChromeDriver](#GetChromeDriver) 
[GetHeadlessChromeDriver](#GetHeadlessChromeDriver) 
[GetFirefoxDriver](#GetFirefoxDriver) 
[GetEdgeDriver](#GetEdgeDriver) 
[GetIEDriver](#GetIEDriver) 
[GetDefaultRemoteOptions](#GetDefaultRemoteOptions) 
[GetRemoteOptions](#GetRemoteOptions) 
[SetDriverOptions](#SetDriverOptions) 
[SetProxySettings](#SetProxySettings) 
[CreateDriver](#CreateDriver) 
[SetBrowserSize](#SetBrowserSize) 
[GetHeadlessWindowSizeString](#GetHeadlessWindowSizeString) 
[ExtractSizeFromString](#ExtractSizeFromString) 
[GetDriverLocation](#GetDriverLocation) 
[GetProgramFilesFolder](#GetProgramFilesFolder) 

## GetDefaultBrowser
Get the default web driver based on the test run configuration 
```csharp
public static IWebDriver GetDefaultBrowser()
{
    return GetBrowserWithDefaultConfiguration(SeleniumConfig.GetBrowserType());
}
```

## GetBrowserWithDefaultConfiguration
Get the default web driver (for the specified browser type) based on the test run configuration 
```csharp
string browserConfig = GetBrowserWithDefaultConfiguration(SeleniumConfig.GetBrowserType());
```

## GetDefaultChromeOptions 
Get the default Chrome options
```csharp
ChromeOptions options = GetDefaultChromeOptions();
```

## GetDefaultHeadlessChromeOptions
Get the default headless Chrome options
```csharp
ChromeOptions options = GetDefaultHeadlessChromeOptions(SeleniumConfig.GetBrowserSize()));
```

## GetDefaultIEOptions
Get the default IE options
```csharp
InternetExplorerOptions = GetDefaultIEOptions();
```

## GetDefaultFirefoxOptions
Get the default Firefox options
```csharp
FirefoxOptions options = GetDefaultFirefoxOptions()
```

## GetDefaultEdgeOptions
Get the default Edge options
```csharp
EdgeOptions options = GetDefaultEdgeOptions();
```

## GetChromeDriver
Initialize a new Chrome driver
```csharp
IWebDriver webDriver = GetChromeDriver(timeout, GetDefaultChromeOptions(), SeleniumConfig.GetBrowserSize());
```

## GetHeadlessChromeDriver
Initialize a new headless Chrome driver
```csharp
IWebDriver webDriver = GetHeadlessChromeDriver(timeout, GetDefaultHeadlessChromeOptions(SeleniumConfig.GetBrowserSize()));
```

## GetFirefoxDriver
Initialize a new Firefox driver
```csharp
IWebDriver webDriver = GetFirefoxDriver(timeout, GetDefaultFirefoxOptions(), SeleniumConfig.GetBrowserSize());
``

## GetEdgeDriver
Initialize a new Edge driver
```csharp
IWebDriver webDriver = GetEdgeDriver(timeout, GetDefaultEdgeOptions(), SeleniumConfig.GetBrowserSize());
```

## GetIEDriver
Get a new IE driver
```csharp
IWebDriver webDriver = GetIEDriver(timeout, GetDefaultIEOptions(), SeleniumConfig.GetBrowserSize());
```

## GetDefaultRemoteOptions
Get the default remote driver options - Default values are pulled from the configuration
```csharp
IWebDriver webDriver = new RemoteWebDriver(SeleniumConfig.GetHubUri(), GetDefaultRemoteOptions().ToCapabilities(), SeleniumConfig.GetCommandTimeout());
```

## GetRemoteOptions
Get the remote driver options
```csharp
DriverOptions options = GetRemoteOptions(remoteBrowserType)
```

## SetDriverOptions
Add additional capabilities to the driver options
```csharp
DriverOptions options = options.SetDriverOptions(remoteCapabilities);
```

## SetProxySettings
Sets the proxy settings for the driver options (if configured)
```csharp
SetProxySettings(options, SeleniumConfig.GetProxyAddress());
```

## CreateDriver
Creates a web driver, but if the creation fails it tries to cleanup after itself
```csharp
return CreateDriver(() => new ChromeDriver(GetDriverLocation("chromedriver.exe"), headlessChromeOptions, commandTimeout));
```

## SetBrowserSize
Sets the browser size based on the provide string value
```csharp
SetBrowserSize(driver, size);
```

## GetHeadlessWindowSizeString
Get the browser/browser size as a string
```csharp
GetHeadlessWindowSizeString("MAXIMIZE");
```

## ExtractSizeFromString
Get the window size as a string
```csharp
ExtractSizeFromString(size, out int width, out int height);
```

## GetDriverLocation
Get the web driver location
```csharp
GetDriverLocation("MicrosoftWebDriver.exe", GetProgramFilesFolder("Microsoft Web Driver", "MicrosoftWebDriver.exe")
```

## GetProgramFilesFolder
Get the programs file folder which contains given file
```csharp
GetProgramFilesFolder("Microsoft Web Driver", "MicrosoftWebDriver.exe")
```