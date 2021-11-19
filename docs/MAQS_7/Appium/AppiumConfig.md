# <img src="resources/maqslogo.ico" height="32" width="32"> Appium Configuration

## Overview
The AppiumConfig class is used to get values from the AppiumMaqs section of your test run properties.
<br>These values come from your App.config, appsettings.json and/or test run parameters.

## AppiumMaqs
The AppiumMaqs configuration section contains the following Keys:
* ***PlatformName***: The mobile device's platform name
* ***PlatformVersion***: The platform version
* ***DeviceName***: The mobile device's name 
* ***MobileHubUrl***: the mobile hub URL
* ***MobileCommandTimeout***: How long to wait before the mobile command should time out in milliseconds
* ***MobileWaitTime***: The wait time (how long do we wait before rechecking) for the mobile device in milliseconds
* ***MobileTimeout***: The timeout (how long we wait for an expected state) for the mobile device in milliseconds
* ***SavePagesourceOnFail***: If MAQS should save the page source when a test fails
* ***SoftAssertScreenshot***: If MAQS should take a screenshot when a test fails


## AppiumCapsMaqs
The AppiumCapsMaqs configuration section contains key value pairs of Appium capabilities. These key values pairs are used as mobile options within Appium options.  
*This is how thing like Sauce Labs credentials are passed in.*
```csharp
// Get mobile options
Dictionary<string, object> capabilitiesAsObjects= AppiumConfig.GetCapabilitiesAsObjects();

AppiumOptions options = new AppiumOptions();
options.AddAdditionalCapability(MobileCapabilityType.DeviceName, AppiumConfig.GetDeviceName());
options.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, AppiumConfig.GetPlatformVersion());
options.AddAdditionalCapability(MobileCapabilityType.PlatformName, AppiumConfig.GetPlatformName().ToUpper());

// Add mobile options
options.SetMobileOptions(capabilitiesAsObjects);

```

## Available methods
Get the mobile OS type as a string:
```csharp
// Android, iOS, or Windows
string platformName = AppiumConfig.GetPlatformName();
```

Get the mobile OS type as an enum:
```csharp
// Android, iOS, or Windows
PlatformType platform = AppiumConfig.GetDeviceType();
```

Get the mobile OS Version:
```csharp
string platformVersion = AppiumConfig.GetPlatformVersion();
```

Get the Device Name:
```csharp
string name = AppiumConfig.GetDeviceName();
```

Get mobile hub url:
```csharp
Url mobileHubUrl = AppiumConfig.GetMobileHubUrl();
```

Get the mobile timeout:
```csharp
TimeSpan timeoutTime = AppiumConfig.GetMobileTimeout();
```

Get the mobile wait time:
```csharp
TimeSpan waitTime = AppiumConfig.GetMobileWaitTime();
```

Get the mobile command timeout:
```csharp
TimeSpan timeout = AppiumConfig.GetCommandTimeout();
```

Get if we should save page source on fail:
```csharp
boolean pageSourceOnFail = AppiumConfig.GetSavePagesourceOnFail();
```

Get if we should save screenshots on soft alert fails:
```csharp
boolean getScreenshot = AppiumConfig.GetSoftAssertScreenshot();
```

Get dictionary of Appium capabilities:
```csharp
Dictionary<string, object> capabilitiesAsObjects= AppiumConfig.GetCapabilitiesAsObjects();
```

# Sample config files
## App.config
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="MagenicMaqs" type="System.Configuration.NameValueSectionHandler" />
    <section name="AppiumMaqs" type="System.Configuration.NameValueSectionHandler" />
    <section name="AppiumCapsMaqs" type="System.Configuration.NameValueSectionHandler" />
  </configSections>
  <MagenicMaqs>
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
    <add key="LogLevel" value="VERBOSE" />

    <!-- Logging Types
    <add key="LogType" value="CONSOLE"/>
    <add key="LogType" value="TXT"/>
    <add key="LogType" value="HTML"/>-->
    <add key="LogType" value="TXT" />

    <!-- Log file path - Defaults to build location if no value is defined
    <add key="FileLoggerPath" value="C:\Frameworks\"/>-->

    <!--Retry and overall timeout in milliseconds-->
    <add key="WaitTime" value="1000" />
    <add key="Timeout" value="10000" />
  </MagenicMaqs>
  <AppiumMaqs>
    <!-- Device settings -->


    <add key="PlatformName" value="Android" />
    <add key="PlatformVersion" value="6.0" />
    <add key="DeviceName" value="Android GoogleAPI Emulator" />

    <!-- Application name - if testing an app -->
    <add key="App" value="sauce-storage:app-name.extension" />

    <!-- Browser information - if testing via the web browser 
    <add key="BrowserName" value="Chrome" />
    <add key="BrowserVersion" value="latest" />-->

    <!-- Appium or grid connection -->
    <add key="MobileHubUrl" value="http://ondemand.saucelabs.com:80/wd/hub" />

    <!-- Command time-out in milliseconds -->
    <add key="MobileCommandTimeout" value="122000" />

    <!-- Wait time in milliseconds - AKA how long do you wait for rechecking something -->
    <add key="MobileWaitTime" value="1000" />

    <!-- Time-out in milliseconds -->
    <add key="MobileTimeout" value="10000" />

    <!-- Do you want to take screenshots upon Soft Assert Failures
    <add key="SoftAssertScreenshot" value="YES"/>
    <add key="SoftAssertScreenshot" value="NO"/>-->
    <add key="SoftAssertScreenshot" value="NO" />

    <!-- Screenshot Image Formats
    <add key="ImageFormat" value="Bmp"/>
    <add key="ImageFormat" value="Gif"/>
    <add key="ImageFormat" value="Jpeg"/>
    <add key="ImageFormat" value="Png"/>
    <add key="ImageFormat" value="Tiff"/>-->
    <add key="ImageFormat" value="Png" />

    <!-- Do you want to save page source when a Soft Assert fails
    <add key="SavePagesourceOnFail" value="YES"/>
    <add key="SavePagesourceOnFail" value="NO"/> -->
    <add key="SavePagesourceOnFail" value="NO" />
  </AppiumMaqs>
  <AppiumCapsMaqs>
    <add key="sauce:options">
      <add key="username" value="S_NAME" />
      <add key="accessKey" value="S_KEY" />
      <add key="appiumVersion" value="1.20.2" />
    </add>
  </AppiumCapsMaqs>

</configuration>
```

## appsettings.json
```json
{
  "MagenicMaqs": {
    "WaitTime": "100",
    "Timeout": "10000",
    "Log": "OnFail",
    "LogLevel": "INFORMATION",
    "LogType": "TXT",
    "UseFirstChanceHandler": "YES"
  },
  "AppiumMaqs": {
    "App": "sauce-storage:app-name.extension",
    "PlatformName": "Android",
    "PlatformVersion": "6.0",
    "DeviceName": "Android GoogleAPI Emulator",
    "MobileHubUrl": "http://ondemand.saucelabs.com:80/wd/hub",
    "MobileWaitTime": "1000",
    "MobileTimeout": "10000",
    "SoftAssertScreenshot": "NO",
    "ImageFormat": "Png",
    "SavePagesourceOnFail": "NO"
  },
   "AppiumCapsMaqs": {
    "sauce:options": {
      "username":'S_NAME',
      "accessKey":'S_KEY',
      "appiumVersion": "1.20.2" 
     }
  }
}
```