# <img src="resources/maqslogo.ico" height="32" width="32"> FAQ

## What Browsers can I use?
- Any browser that has a IWebDriver.  
If you want to use a browser that MAQS doesn't natively support you can just initialize the driver yourself.
```csharp
this.WebDriver = new OperaDriver("path_to_opera_driver.exe");
```
- Natively supported browsers are:  Firefox | Chrome | Edge | PhantomJS | Internet Explorer. 
Find the configuration of browsers within the app.config file and define one and only one.

- App.Config: Want to make changes to the Browser, Logging, Remote Integration Hubs, Wait times, Creating Logs, Log File Format and Log folder location ?? Look to the App.Config within Tests

## Why doesn't PhantomJS work
- Selenium 3.14.0 removed support for PhantomJS. Headless Chrome typically is a good substitute. 

### How do I fix common user errors?

### My code can't find the config class  
- Make sure you've imported the Full namespace such as Magenic.Maqs.Utilities.Helper.Config
### Test can't find the page element for a test, errors indicate something is wrong with Selenium  
- Kill the existing Chrome processes spawned and orphaned from the test kick off then update Browser and start fresh
### Test fails on driver, such as Chrome  
- Update the Chrome browser  (In Chrome click the HotDog icon (formerly known as Wrench) > help > about Google Chrome > Update , the key is that the Chrome browser version and the ChromeDriver versions must be compatible with each other.

## Build Solution Fails
- If NuGet packages are missing, Right-click the solution and Enable NuGet Package restore (this requires VPN or LAN connection) Unless an off network storage location or shared folder, was setup to hold the NuGet packages.  Visual Studio allows you to configure additional NuGet locations (see below)

## Driver doesn't want to run
- You should receive a pop-up stating Windows Firewall blocked some features of this app.  Enable Private networks, such as my home or work network and Enable Public networks, such as those in airports and coffee shops (note this is not recommended) Finally, Click Allow Access.

## Configuration Errors from Platform
- right-click the Solution and select Properties then locate the Configuration Properties and set Platform to Any CPU for all Projects such as Tests and WebServiceModel (in this case the solution would be a WebService)

## Errors trying to locate Enterprise when restoring NuGet packages
- right-click the Solution and select Properties > Options > NuGet Package Manager > Package Sources  here you will find Available package sources and Machine-wide package sources.  A mapping to the internal Magenic package storage url (for internal use only) or an external reference such as and internal NuGet repository or file share.

## What Integrations exist?

### Sauce Labs
- Support for and configuration of integration with Sauce Labs can be done by providing a username and accesskey in the RemoteSeleniumCapsMaqs section of your configuration file.
```xml
  <SeleniumMaqs>
    <!--Remote browser settings-->
    <add key="Browser" value="REMOTE"/>
    <add key="RemoteBrowser" value="Generic" />
    <add key="HubUrl" value="http://ondemand.saucelabs.com:80/wd/hub" />
  </SeleniumMaqs>
  <RemoteSeleniumCapsMaqs>
    <add key="username" value="Sauce_Labs_Username" />
    <add key="accessKey" value="Sauce_Labs_Accesskey" />
    <add key="browserName" value="Chrome" />
    <add key="platform" value="Windows 10" />
  </RemoteSeleniumCapsMaqs>
  ```

### BrowserStack
- Support for and configuration of integration with BrowserStack can be done by providing a browserstack.use and browserstack.key in the RemoteSeleniumCapsMaqs section of your configuration file.
```xml
  <SeleniumMaqs>
    <!--Remote browser settings-->
    <add key="Browser" value="REMOTE"/>
    <add key="RemoteBrowser" value="Generic" />
    <add key="HubUrl" value="http://hub-cloud.browserstack.com/wd/hub/" />
  </SeleniumMaqs>
  <RemoteSeleniumCapsMaqs>
    <add key="browserstack.user" value="BrowserStack_Username" />
    <add key="browserstack.key" value="BrowserStack_Accesskey" />
    <add key="browser" value="Chrome" />
    <add key="os" value="Windows" />
    <add key="os_version" value="10" />
  </RemoteSeleniumCapsMaqs>
  ```

### Note: Magenic MAQS utilizes NuGet
- A package manager for the Microsoft development platform which is a central repository for bundling client tools in a single process

## Explain Configs to me... please!

# Templates:  out of the box templates
- For  Appium, Database and Email (IMAP only), Selenium, Web Service (json & xml models), Composite (mix and match MAQS capabilities)
   -Project Level for Model which is a Page Object Model
   -Tests Level for Test class and App.config
   -Item Level for Page Object Model and Test Class (in most cases except Selenium, test class only
### Logging
- Yes, No, or On Fail and the Defaults are  Log = No, LogType = Console LogLevel = INFORMATION and FileLoggerPath = Test dll "log" subfolder    File Format options: txt, html, console
### Soft Asserts
- Specific to a test (inside) These get logged to the Log and become Hard Asserts after use. AreEqual or FailTestIfAssertFailed or Assert.AreEqual  helps your testing approach be more efficient instead of having to fix individual failures.
### Performance Measures
- Human readable and requires a Name, Embedded Perf tests to start at different times, Basically it tells how long the data collection took using this as a stop watch
### TestObject
- Holds test specific objects and available in all Magenic MAQS flavors
### Waits
- GenericWait.WaitFor waits until exceptions are no longer thrown (calls the function N number of times) or GenericWait.WaitUntil  responds with true / false
### Customizations
- Utilizes the app.config which is Globally used information, avoids hard-coded code and you can add Key|Value custom pairs such as UserName =YOU PassWord=ABC   We also allow you to use 'default' 
##### Log Level
![The levels of logs](resources/logleveldiagram.png)  
Each log level is grouped into a hierarchy, with the highest log levels also including any messages from lower log levels in the hierarchy.
Verbose mode includes all information gathered by the logger, which includes any navigation that occurs on the page, and anytime the WebDriver attempts to find an element or interact with an element. Verbose mode will result in many, often superfluous, lines of information.
Information mode includes messages for when a WebDriver is loaded, a value is identified, an element is interacted with, or whenever the WebDriver throws an exception.
Generic mode is the default logging level. It includes performance timers, and log interactions such as when a performance timer log is saved.
Success mode will output a message anytime a soft assert is successful, or as a test ends and the test is successful.
Warning mode will output a message anytime a soft assert fails, the test is met with unexpected results, or the test configuration fails to update.
Error mode will only display messages that would fail a test. This includes a test failed, a test resulted in being inconclusive, a setup failed, or the final soft assert data contains any failed soft asserts.
Suspended mode will result in no information written to the log. 
## Test Settings
"There are two types of file for configuring tests. *.runsettings are used for unit tests. And *.testsettings for lab environment tests, web performance and load tests, and for customizing some types of diagnostic data adapters such as IntelliTrace and event log adapters"
https://msdn.microsoft.com/en-us/library/jj635153.aspx  
***TestSettings files should no longer be used**
Both test settings files are written in XML (eXtensible Markup Language).
### Run Settings Configurations
A .runsettings file can be added as a test setting to add additional configurations when running unit tests. This allows tests to be run on additional cores on a single machine, to run on different versions of the unit test framework, or to specify where the results of the test should be output.
#### Adding Run Settings
To add a .runsettings file to a test solution, go to the top toolbar and under Test → Test Settings, choose the "Select Test Settings File" option, and finally add a .runsettings file.  
![Remote Browser Settings](resources/AddNewTestSettings.png)  
Since there is no way to have Visual Studio generate a template of a .runsettings file, you can instead add an example .runsettings file to your solution such as this one found on Microsoft's MSDN website: https://msdn.microsoft.com/en-us/library/jj635153.aspx#example
