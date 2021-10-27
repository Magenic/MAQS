# <img src="resources/maqslogo.ico" height="32" width="32"> Configurations

## Introduction
The below section will cover global test settings.

## Application Configuration
The MAQS project solutions make use of a collection of configurations. These configurations are stored in an XML document called the **app.config**.
The **app.config** file includes configurations for each project solution, as well as generic configurations for handling MAQS specific functions.  
*The .Net Core version of MAQS leverages a JSON version of this file called **appsettings.json**.  
The XML and JSON versions of the configuration files follow a very similar format.*

### MagenicMaqs - General Test Configurations
General test configurations are included in every project template. They control wait time, time-out, and log levels.
#### Wait Time
Polling time (how long the code waits between retries) used for generic waits in milliseconds.
##### Examples
```xml
<!-- Generic wait time in milliseconds - AKA how long do you wait for rechecking something -->
<add key="WaitTime" value="1000" />
```

#### Timeout
The overall timeout for generic waits in milliseconds.
##### Examples
```xml
<!-- Generic time-out in milliseconds -->
<add key="Timeout" value="10000" />
```

#### Log
This setting dictates if and/or when logs are created. With the default option "Yes," a test will always create a corresponding log after the test finishes. The other options are "No" and "OnFail." "No" will never create a log under any circumstance, while "OnFail" will only create a log if the test fails.
##### Examples
```xml
 <!-- Do you want to create logs for your tests
<add key="Log" value="YES"/>
<add key="Log" value="NO"/>
<add key="Log" value="OnFail"/>-->
<add key="Log" value="OnFail" />
```
#### Logging Levels
This setting dictates how verbose the logging will be. With the default option, "Information", the log will include everything but Verbose messages. 

Levels
 - Verbose option - Logs everything
 - Information(Default) - Logs informative, generic, success, warning, and error messages
 - Generic - Logs generic, success, warning, and error messages
 - Success - Logs success, warning, and error messages
 - Warning - Logs all warning and error messages
 - Error - Only logs error messages

##### Examples
```xml
<!--Logging Levels
<add key="LogLevel" value="VERBOSE"/>
<add key="LogLevel" value="INFORMATION"/>
<add key="LogLevel" value="GENERIC"/>
<add key="LogLevel" value="SUCCESS"/>
<add key="LogLevel" value="WARNING"/>
<add key="LogLevel" value="ERROR"/>-->
<add key="LogLevel" value="INFORMATION" />
```
#### Logging Types
This setting dictates the format of the log files. 

Types
 - Console - Prints to console logger, no file is created.
 - TXT(Default) - Creates a TXT file.
 - HTML - Creates a HTML file.

##### Examples
```xml
<!-- Logging Types
<add key="LogType" value="CONSOLE"/>
<add key="LogType" value="TXT"/>
<add key="LogType" value="HTML"/>-->
<add key="LogType" value="HTML" />
```

#### Log Output Location - Optional
The log file path can be set to a specific folder or shared drive that the test runner has access to.  
_*By default, logs end up in the "log" folder. This is located in the same folder as the test DLL._

##### Examples
```xml
<!-- Log file path - Defaults to build location if no value is defined
<add key="FileLoggerPath" value="C:\Frameworks\"/>-->
```

#### First Chance Exception Handler - Optional
The first chance exception handler will capture any exceptions thrown that are unexpected.  Use this configuration to disable this feature.  Values are "YES" and "NO".

*By default this is enabled

```xml
<!-- Use First Chance Handler - Defaults to enabled
<add key="UseFirstChanceHandler" value="NO"/>
<add key="UseFirstChanceHandler" value="YES"/>-->
```

#### SkipConfigValidation - Optional
The configuration validation will throw an exception if required configuration settings are missing.
Use this configuration to disable this feature.  Values are "YES" and "NO".

*By default validation is not skipped
##### Examples
```xml
 <!-- Should the configuration validation be skipped
<add key="SkipConfigValidation" value="YES"/>
<add key="SkipConfigValidation" value="NO"/>-->
```

# Full Configuration
## App.Config
Primarily uses with the .Net Framework implementation of MAQS.
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="MagenicMaqs" type="System.Configuration.NameValueSectionHandler" />
    <section name="AppiumMaqs" type="System.Configuration.NameValueSectionHandler"/>
    <section name="AppiumCapsMaqs" type="System.Configuration.NameValueSectionHandler"/>
    <section name="DatabaseMaqs" type="System.Configuration.NameValueSectionHandler" />
    <section name="EmailMaqs" type="System.Configuration.NameValueSectionHandler"/>
    <section name="SeleniumMaqs" type="System.Configuration.NameValueSectionHandler"/>
    <section name="RemoteSeleniumCapsMaqs" type="System.Configuration.NameValueSectionHandler"/>
    <section name="WebServiceMaqs" type="System.Configuration.NameValueSectionHandler"/>
  </configSections>
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

    <!-- Use First Chance Handler - Defaults to enabled
    <add key="UseFirstChanceHandler" value="NO"/>
    <add key="UseFirstChanceHandler" value="YES"/>-->

     <!-- Should the configuration validation be skipped
    <add key="SkipConfigValidation" value="YES"/>
    <add key="SkipConfigValidation" value="NO"/>-->
  </MagenicMaqs>
  <AppiumMaqs>
    <!--Device platform
    <add key="PlatformName" value="ANDROID"/>
    <add key="PlatformName" value="IOS"/>
    <add key="PlatformName" value="WINDOWS"/> -->
    <add key="PlatformName" value="Android"/>
    
    <!--Device settings - Optional, used primarily for cloud based services    -->
    <add key="PlatformVersion" value="8.1"/>
    <add key="DeviceName" value="Android GoogleAPI Emulator"/>

        <!-- Application name - if testing an app -->
    <add key="App" value="sauce-storage:app-name.extension" />

    <!-- Browser information - if testing via the web browser 
    <add key="BrowserName" value="Chrome" />
    <add key="BrowserVersion" value="latest" />-->

    <!-- Appium or grid connection -->
    <!-- <add key="MobileHubUrl" value="http://ondemand.saucelabs.com:80/wd/hub" /> -->
    <add key="MobileHubUrl" value="http://127.0.0.1:4723/wd/hub" />
    
    <!-- Command time-out in milliseconds
    <add key="MobileCommandTimeout" value="60000"/> -->

    <!-- Wait time in milliseconds - AKA how long do you wait for rechecking something -->
    <add key="MobileWaitTime" value="1000" />

    <!-- Time-out in milliseconds -->
    <add key="MobileTimeout" value="10000" />

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
  </AppiumMaqs>
  <AppiumCapsMaqs>
    <!-- Remote settings -->
    <add key="sauce:options" value="{username:'S_NAME', accessKey:'S_KEY', appiumVersion:'1.20.2' }"  />
    <add key="appActivity" value="com.magenic.appiumtesting.maqsregistrydemo.LoginPage" />
    <add key="appPackage" value="com.magenic.appiumtesting.maqsregistrydemo" /> 
  </AppiumCapsMaqs>
  <DatabaseMaqs>
    <!--<add key="DataBaseProviderType" value="SQLSERVER" />
    <add key="DataBaseConnectionString" value="Data Source=DB;Initial Catalog=MagenicAutomation;Persist Security Info=True;User ID=ID;Password=PW;Connection Timeout=30" />   
    <add key="DataBaseProviderType" value="POSTGRE" />
    <add key="DataBaseConnectionString" value="Server=127.0.0.1;Port=1234;Database=maqs;User Id=UserID;Password=PW;" />    
    <add key="DataBaseProviderType" value="SQLITE" />
    <add key="DataBaseConnectionString" value="Data Source=PATH\TO\MyDatabase.sqlite;" />-->
    <add key="DataBaseProviderType" value="SQLSERVER" />
    <add key="DataBaseConnectionString" value="CONNECTION" />
  </DatabaseMaqs>
  <EmailMaqs>
    <!--IMAP connection settings-->
    <add key="EmailHost" value="imap.PROVIDER.com" />
    <add key="EmailUserName" value="FAKENAME@PROVIDER.com" />
    <add key="EmailPassword" value="PASSWORD" />
    <add key="EmailPort" value="993" />
    <add key="ConnectViaSSL" value="Yes" />
    <add key="SkipSslValidation" value="Yes" />
    
    <!-- Time-out in milliseconds -->
    <add key="EmailTimeout" value="10000" />

    <!-- Download attachment path - Defaults to attachments folder under the build location if no value is defined
    <add key="AttachmentDownloadPath" value="C:\Frameworks\downloads"/>-->
  </EmailMaqs>
  <SeleniumMaqs>
    <!-- Root to website -->
    <add key="WebSiteBase" value="http://magenicautomation.azurewebsites.net/" />

    <!--Local browser settings
    <add key="Browser" value="Chrome"/>
    <add key="Browser" value="HeadlessChrome"/>
    <add key="Browser" value="Internet Explorer"/>
    <add key="Browser" value="Firefox"/>
    <add key="Browser" value="Edge"/> -->
    <add key="Browser" value="Chrome" />

    <!--Browser Resize settings
    <add key="BrowserSize" value ="MAXIMIZE"/>
    <add key="BrowserSize" value="DEFAULT"/>
    <add key="BrowserSize" value="600x1600"/>-->
    <add key="BrowserSize" value="MAXIMIZE"/>

    <!-- Web driver hint path override - This is the first place Maqs will try to find your web drive -->
    <add key="WebDriverHintPath" value="C:\Frameworks"/>

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

    <!-- Proxy options -->
    <add key="UseProxy" value="NO" />
    <add key="ProxyAddress" value="127.0.0.1:8080" />
  </SeleniumMaqs>
  <RemoteSeleniumCapsMaqs>
    <!-- Cloud based Grid settings
     <add key="sauce:options" value="{username: 'SAUCE_NAME', accessKey:  'SAUCE_KEY' }"  /> -->
  </RemoteSeleniumCapsMaqs>
  <WebServiceMaqs>
    <!-- Web service root -->
    <add key="WebServiceUri" value="http://magenicautomation.azurewebsites.net" />
    
    <!-- Time-out in milliseconds -->
    <add key="WebServiceTimeout" value="10000" />

    <!-- Proxy options -->
    <add key="UseProxy" value="NO" />
    <add key="ProxyAddress" value="127.0.0.1:8080" />
  </WebServiceMaqs>
</configuration>
```
## appsettings.json
Primarily uses with the .Net Core implementation of MAQS.
```json
{
  "MagenicMaqs": {
    "WaitTime": "100",
    "Timeout": "10000",
    "Log": "OnFail",
    "LogLevel": "INFORMATION",
    "LogType": "TXT",
    "UseFirstChanceHandler": "YES",
    "SkipConfigValidation": "NO",
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
    "username": "{username:'S_NAME', accessKey:'S_KEY'}"
  },
  "DatabaseMaqs": {
    "DataBaseProviderType": "SQLSERVER",
    "DataBaseConnectionString": "Data Source=DATABASE;Initial Catalog=TEST_DB;Persist Security Info=True;User ID=USER_ID;Password=USER_PASSWORD;Connection Timeout=30"
  },
  "EmailMaqs": {
    "EmailHost": "imap.PROVIDER.com",
    "EmailUserName": "FAKENAME@PROVIDER.com",
    "EmailPassword": "PASSWORD",
    "EmailPort": "993",
    "ConnectViaSSL": "Yes",
    "SkipSslValidation": "Yes",
    "AttachmentDownloadPath": "C:\\Frameworks\\downloads",
    "EmailTimeout": "10000"
  },
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
    "SavePagesourceOnFail": "NO",
    "UseProxy": "No",
    "ProxyAddress": "127.0.0.1:8080"
  },
  "RemoteSeleniumCapsMaqs": {
     "sauce:options": "{username: 'SAUCE_NAME', accessKey:  'SAUCE_KEY' }"
  },
  "WebServiceMaqs": {
    "WebServiceUri": "http://magenicautomation.azurewebsites.net",
    "WebServiceTimeout": "1000",
    "UseProxy": "No",
    "ProxyAddress": "127.0.0.1:8080"
  }
}
```