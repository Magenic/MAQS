# <img src="resources/maqslogo.ico" height="32" width="32"> Selenium Configuration

## Overview
The SeleniumConfig class is used to get values from the SeleniumMaqs section of your test run properties.
<br>These values come from your App.config, appsettings.json and/or test run parameters.

## SeleniumMaqs
The SeleniumMaqs configuration section contains the following Keys:  
**Items are only respected when using the REMOTE browser*
* ***WebSiteBase*** : The base website url
* ***Browser*** : Which browser to use
* ***BrowserSize*** : The browser resolution
* ***WebDriverHintPath*** : First place to look for the web drive EXE
* ***RemoteBrowser**** : The type of browser to use when executing remotely which something like Grid or SauceLabs
* ***HubUrl**** : The grid URL
* ***RemotePlatform**** : The remote OS
* ***RemoteBrowserVersion**** : The remote browser version
* ***SeleniumCommandTimeout*** : How long wait before saying the connection to Selenium has died
* ***BrowserWaitTime*** : How long to wait before rechecking for something - Used heavily with the MAQS waits
* ***BrowserTimeout*** : How long to wait for something before timing out - Used heavily with the MAQS waits
* ***SoftAssertScreenshot*** : If a screenshot should be taken when a soft assert fails
* ***ImageFormat*** :  What format screenshot should be saved as
* ***SavePagesourceOnFail*** : If page source is saved when a test fails


## RemoteSeleniumCapsMaqs

Remote Selenium capabilities are used when only when you use a REMOTE browser.  
These are key value pairs that get added to the remote web driver's desired capabilities.

## Available methods
Get the browser name:
```csharp
string driverName = SeleniumConfig.GetBrowserName();
```

Get a web driver based on your configuration:
```csharp
IWebDriver driver = SeleniumConfig.Browser();
```
Get a web driver for the specified browser:
```csharp
IWebDriver driver = SeleniumConfig.Browser("Chrome");
```
Get the command timeout:
```csharp
TimeSpan initTimeout = SeleniumConfig.GetCommandTimeout();
```
Get the web driver EXE hint path:
```csharp
string path = SeleniumConfig.GetDriverHintPath();
```
Get the remote browser name:
```csharp
string browser = SeleniumConfig.GetRemoteBrowserName();
```
Get the remote browser version:
```csharp
string version = SeleniumConfig.GetRemoteBrowserVersion();
```
Get the remote browser platform:
```csharp
string platform = SeleniumConfig.GetRemotePlatform();
```
Get the a wait driver for the provided web driver:  
*The wait times are pulled from your configuration*
```csharp
WebDriverWait defaultWaitDriver = SeleniumConfig.GetWaitDriver(this.WebDriver);
```
Get the base web site url:
```csharp
string siteUrl = SeleniumConfig.GetWebSiteBase();
```
Get the if page source should be captured test failure:
```csharp
bool savePageSourceOnFail = SeleniumConfig.GetSavePagesourceOnFail();
```
Get the if screenshots should be taken when there is a soft assert failure:
```csharp
bool saveSoftFailScreenshot = SeleniumConfig.GetSoftAssertScreenshot();
```
Get the screenshot image format:
```csharp
string imageFormat = SeleniumConfig.GetImageFormat();
```
Sets the time our for a provided web driver:   
*The wait times are pulled from your configuration*
```csharp
SeleniumConfig.SetTimeouts(driver);
```

# Sample config files
## App.config
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="SeleniumMaqs" type="System.Configuration.NameValueSectionHandler"/>
    <section name="RemoteSeleniumCapsMaqs" type="System.Configuration.NameValueSectionHandler"/>
    <section name="MagenicMaqs" type="System.Configuration.NameValueSectionHandler" />
  </configSections>
  <SeleniumMaqs>
    <!-- Root to website -->
    <add key="WebSiteBase" value="http://magenicautomation.azurewebsites.net/" />

    <!--Local browser settings
    <add key="Browser" value="Chrome"/>
    <add key="Browser" value="HeadlessChrome"/>
    <add key="Browser" value="Internet Explorer"/>
    <add key="Browser" value="Firefox"/>
    <add key="Browser" value="PhantomJS"/>
    <add key="Browser" value="Edge"/> -->
    <add key="Browser" value="Chrome" />

    <!--Browser Resize settings
    <add key="BrowserSize" value ="MAXIMIZE"/>
    <add key="BrowserSize" value="DEFAULT"/>
    <add key="BrowserSize" value="600x1600"/>-->
    <add key="BrowserSize" value="MAXIMIZE"/>

    <!-- Web driver hint path override - This is the first place Maqs will try to find your web drive 
    <add key="WebDriverHintPath" value="C:\Frameworks"/>-->

    <!-- Remote browser settings - RemoteBrowser can be any standard browse; such as IE, Firefox, Chrome, Edge or Safari
    <add key="Browser" value="REMOTE"/> -->
    <add key="RemoteBrowser" value="Chrome"/>
    <add key="HubUrl" value="http://localhost:4444/wd/hub"/>

    <!-- Extended remote browser settings - OS (xp, win7, win8, win8.1, win10, os x, os x 10.6, os x 10.8, os x 10.9, os x 10.10, os x 10.11, solaris, linux, android, +more)-->
    <!-- <add key="RemotePlatform" value="win7"/>-->

    <!-- Extended remote browser settings - Browser version-->
    <!-- <add key="RemoteBrowserVersion" value="44"/>-->

    <!-- Command Time-out in milliseconds -->
    <add key="SeleniumCommandTimeout" value="60000"/>

    <!-- Wait time in milliseconds - AKA how long do you wait for rechecking something -->
    <add key="BrowserWaitTime" value="1000" />

    <!-- Time-out in milliseconds -->
    <add key="BrowserTimeout" value="10000" />

    <!-- Do you want to take screenshots upon Soft Assert Failures
    <add key="SoftAssertScreenshot" value="YES"/>
    <add key="SoftAssertScreenshot" value="NO"/>-->
    <add key="SoftAssertScreenshot" value="NO"/>

    <!-- Screenshot Image Formats
    <add key="ImageFormat" value="Bmp"/>
    <add key="ImageFormat" value="Gif"/>
    <add key="ImageFormat" value="Jpeg"/>
    <add key="ImageFormat" value="Png"/>
    <add key="ImageFormat" value="Tiff"/>-->
    <add key="ImageFormat" value="Png"/>

    <!-- Do you want to save page source when a Soft Assert fails
    <add key="SavePagesourceOnFail" value="YES"/>
    <add key="SavePagesourceOnFail" value="NO"/> -->
    <add key="SavePagesourceOnFail" value="NO"/>
  </SeleniumMaqs>
  <RemoteSeleniumCapsMaqs>
    <!-- Cloud based Grid settings
    <add key="username" value="Sauce_Labs_Username"/>
    <add key="accessKey" value="Sauce_Labs_Accesskey"/>
    <add key="browserName" value="Chrome"/>
    <add key="platform" value="OS X 10.11"/>
    <add key="version" value="54.0"/> -->
  </RemoteSeleniumCapsMaqs>
  <MagenicMaqs>
    <!-- Generic wait time in milliseconds - AKA how long do you wait for rechecking something -->
    <add key="WaitTime" value="1000" />

    <!-- Generic time-out in milliseconds -->
    <add key="Timeout" value="10000" />

    <!-- Do you want to create logs for your tests
    <add key="Log" value="YES"/>
    <add key="Log" value="NO"/>
    <add key="Log" value="OnFail"/>-->
    <add key="Log" value="OnFail" />

    <!--Logging Levels
    <add key="LogLevel" value="VERBOSE"/>
    <add key="LogLevel" value="INFORMATION"/>
    <add key="LogLevel" value="GENERIC"/>
    <add key="LogLevel" value="SUCCESS"/>
    <add key="LogLevel" value="WARNING"/>
    <add key="LogLevel" value="ERROR"/>-->
    <add key="LogLevel" value="INFORMATION" />

    <!-- Logging Types
    <add key="LogType" value="CONSOLE"/>
    <add key="LogType" value="TXT"/>
    <add key="LogType" value="HTML"/>-->
    <add key="LogType" value="TXT" />

    <!-- Log file path - Defaults to build location if no value is defined
    <add key="FileLoggerPath" value="C:\Frameworks\"/>-->
  </MagenicMaqs>
</configuration>
```
## appsettings.json
```json
{
  "SeleniumMaqs": {
    "WebSiteBase": "http://magenicautomation.azurewebsites.net/",
    "Browser": "Chrome",
    "HubUrl": "http://localhost:4444/wd/hub",
    "RemoteBrowser": "Chrome",
    "SeleniumCommandTimeout": "60000",
    "BrowserWaitTime": "100",
    "BrowserTimeout": "10000",
    "BrowserSize": "MAXIMIZE",
    "SoftAssertScreenshot": "NO",
    "ImageFormat": "Png",
    "SavePagesourceOnFail": "NO"
  },
  "RemoteSeleniumCapsMaqs": {
    "Username": "Sauce_Labs_Username",
    "AccessKey": "Sauce_Labs_Accesskey",
    "BrowserName": "Chrome",
    "Platform": "OS X 10.11",
    "Version": "54.0"
  },
  "MagenicMaqs": {
    "WaitTime": "100",
    "Timeout": "10000",
    "Log": "OnFail",
    "LogLevel": "INFORMATION",
    "LogType": "TXT"
  }
}
```