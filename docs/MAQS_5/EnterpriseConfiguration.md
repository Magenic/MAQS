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
    <!-- Local App File Path -->
    <!-- <add key="app" value="/path/to/app/package"/> -->
    <!-- Sauce Labs Configuration Settings-->
    <!--<add key="username" value="Sauce_Labs_Username"/> -->
    <!--<add key="accessKey" value="Sauce_Labs_Accesskey"/> -->
    <!--<add key="appiumVersion" value="1.7.1"/> -->
    <!-- <add key="app" value="sauce-storage:app-name.extension"/> -->
    <add key="app" value="App_Path" />
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
    <add key="Browser" value="PhantomJS"/>
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
  </SeleniumMaqs>
  <RemoteSeleniumCapsMaqs>
    <!-- Cloud based Grid settings
    <add key="username" value="Sauce_Labs_Username"/>
    <add key="accessKey" value="Sauce_Labs_Accesskey"/>
    <add key="browserName" value="Chrome"/>
    <add key="platform" value="OS X 10.11"/>
    <add key="version" value="54.0"/> -->
  </RemoteSeleniumCapsMaqs>
  <WebServiceMaqs>
    <!-- Web service root -->
    <add key="WebServiceUri" value="http://magenicautomation.azurewebsites.net" />
    
    <!-- Time-out in milliseconds -->
    <add key="WebServiceTimeout" value="10000" />
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
    "LogType": "TXT"
  },
  "AppiumMaqs": {
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
    "username": "Sauce_Labs_Username",
    "accessKey": "Sauce_Labs_Accesskey",
    "appiumVersion": "1.7.1",
    "app": "sauce-storage:app-name.extension"
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
    "SavePagesourceOnFail": "NO"
  },
  "RemoteSeleniumCapsMaqs": {
    "Username": "Sauce_Labs_Username",
    "AccessKey": "Sauce_Labs_Accesskey",
    "BrowserName": "Chrome",
    "Platform": "OS X 10.11",
    "Version": "54.0"
  },
  "WebServiceMaqs": {
    "WebServiceUri": "http://magenicautomation.azurewebsites.net",
    "WebServiceTimeout": "1000"
  }
}
```