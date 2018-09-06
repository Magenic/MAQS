# <img src="resources/maqslogo.ico" height="32" width="32"> Configurations

## Introduction
The below section will cover global test settings.

## Application Configuration
The MAQS project solutions make use of a collection of configurations. These configurations are stored in an XML document called the **app.config**.
The **app.config** file includes configurations for each project solution, as well as generic configurations for handling MAQS specific functions.

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
This setting dictates if and/or when logs are created. With the default option "Yes," a test will always create a corresponding log after the test finishes. The other options are “No” and “OnFail.” “No” will never create a log under any circumstance, while “OnFail” will only create a log if the test fails.
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

### SeleniumMaqs - Selenium Test Configuration
Selenium specific configurations assist in switching between web browsers, setting up remote browser settings, selecting a specific WebDriver, or setting base root information.
#### Local Browser Settings 
This setting dictates which browser is used for Selenium tests. 
- Chrome (Default)
- HeadlessChrome - Chrome running without the UI
- Internet Explorer or IE
- Firefox
- PhantomJS - A headless web browser, this will be depricated in the future
- Edge
- Remote - Tells MAQS it will be running against a grid.  
*See Remote Browser Settings section below if you intend to use Grid

##### Examples
```xml
 <!--Local browser settings
 <add key="Browser" value="Chrome"/>
 <add key="Browser" value="HeadlessChrome"/>
 <add key="Browser" value="Internet Explorer"/>
 <add key="Browser" value="Firefox"/>
 <add key="Browser" value="PhantomJS"/>
 <add key="Browser" value="Edge"/> 
 <add key="Browser" value="Remote"/> -->
 <add key="Browser" value="Chrome" />
```

#### Browser Resize Settings - Optional 

This option will specify what size you would like the browser window to be when it is initialized.
- Maximize (Default) - Opens the browser maximized
- Default - MAQS does not change your browser size. 
- Custom - Specify any size you want the browser to open up with in pixels (ie 600x1600).

##### Examples
```xml
<!--Browser Resize settings
<add key="BrowserSize" value ="MAXIMIZE"/>
<add key="BrowserSize" value="DEFAULT"/>
<add key="BrowserSize" value="600x1600"/>-->
<add key="BrowserSize" value="MAXIMIZE"/>
```


#### WebDriver Hint Path - Optional
This setting allows you to provide an alternate path for where to find your webdriver.  By default the test will look for the webdriver in it's current folder.  
_*By default, MAQS will only look in the test DLL folder for webdrivers._

##### Examples
```xml
<!-- Web driver hint path override - This is the first place Maqs will try to find your web drive 
<add key="WebDriverHintPath" value="C:\Frameworks"/>-->
```

#### Remote Browser Settings when using Selenium Grid
The app.config file can be configured to send tests to a hub for distribution. 
1. Update “Browser” key to “Remote” value.
2. Set “RemoteBrowser” key to the web browser that the tests will be run against.  
(for options see the above section Local Browser Settings or use GENERIC and define the browserName in RemoteSeleniumCapsMaqs)
3. Update the “HubUrl” value to the path of your Selenium grid instance.

*To use these configurations, Selenium Grid hub and node servers must be set up beforehand (see http://www.seleniumhq.org/docs/07_selenium_grid.jsp for more information)*  

There are additional options for configuring the remote browser environment. You can specify the remote platform to use (Windows, macOS, Linux, etc.), as well as what browser version should be used for the specified remote browser. 

*Remote Platform and Remote Browser Version are both optional when configuring Remote Browser settings*

##### Examples
```xml
<!-- Remote browser settings - RemoteBrowser can be any standard browser (IE, Firefox, Chrome, Edge or Safari) or use GENERIC and define the browserName in RemoteSeleniumCapsMaqs -->
<add key="Browser" value="REMOTE"/>
<add key="RemoteBrowser" value="GENERIC"/>
<add key="HubUrl" value="http://localhost:4444/wd/hub"/>

<!-- Extended remote browser settings - OS (xp, win7, win8, win8.1, win10, os x, os x 10.6, os x 10.8, os x 10.9, os x 10.10, os x 10.11, solaris, linux, android, +more)-->
<add key="RemotePlatform" value="win7"/>

<!-- Extended remote browser settings - Browser version-->
<add key="RemoteBrowserVersion" value="44"/>
```

#### Selenium Command Timeout
This setting allows the user to set the global Selenium command timeout which is the maximum amount of time (in milliseconds) MAQS will wait to connect to Selenium.

##### Examples
```xml
<!-- Command Time-out in milliseconds -->
<add key="SeleniumCommandTimeout" value="60000"/>
```

#### Browser Wait Time
This setting is the polling time (how long the code waits between retries) used for Selenium waits in milliseconds.
##### Examples
```xml
<!-- Wait time in milliseconds - AKA how long do you wait for rechecking something -->
<add key="BrowserWaitTime" value="1000" />
```

#### Browser Timeout
The overall timeout for Selenium waits in milliseconds.

##### Examples
```xml
<!-- Time-out in milliseconds -->
<add key="BrowserTimeout" value="10000" />
```