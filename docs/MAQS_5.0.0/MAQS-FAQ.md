# <img src="resources/maqslogo.ico" height="32" width="32"> FAQ

## What Browsers can I use?
- Supported browsers are:  Firefox | Chrome | Edge | PhantomJS | Internet Explorer. 
   Find the configuration of browsers within the app.config file and define one and only one.

- App.Config: Want to make changes to the Browser, Logging, Remote Integration Hubs, Wait times, Creating Logs, Log File Format and Log folder location ?? Look to the App.Config within Tests

## How do I fix common user errors?

- My code can't find the config class : Make sure you've imported the Full namespace such as Magenic.MaqsFramework.Utilities.Helper.Config
- Test can't find the page element for a test, errors indicate something is wrong with Selenium = Kill the existing Chrome processes spawned and orphaned from the test kick off then update Browser and start fresh
- Test fails on driver, such as Chrome = solution is to update the Chrome browser  (In Chrome click the HotDog icon (formerly known as Wrench) > help > about Google Chrome > Update , the key is that the Chrome browser version and the ChromeDriver versions must be compatible with each other.

### Build Solution Fails
- If NUget packages are missing, Right-click the solution and Enable NUget Package restore (this requires VPN or LAN connection) Unless an off network storage location or shared folder, was setup to hold the nuget packages.  Visual Studio allows you to configure additional Nuget locations (see below)

### PhantomJS doesn't want to run
- You should receive a pop-up stating Windows Firewall blocked some features of this app.  Enable Private networks, such as my home or work network and Enable Public networks, such as those in airports and coffee shops (note this is not recommended) Finally, Click Allow Access.

### Compile Server Browser cannot find the Chrome binary

- A perfect reason for running headless mode. Compile servers use PhantomJS   AND.... please verify the Test References > right-click references has Microsoft.VisualStudio.QualityTools.UnitTestFramework checked version 10.0.0.0 or 10.1.0.0 work.

### Configuration Errors from Platform
- right-click the Solution and select Properties then locate the Configuration Properties and set Platform to Any CPU for all Projects such as Tests and WebServiceModel (in this case the solution would be a WebService)

### Errors trying to locate v4.0.0.0 when restoring Nuget packages
- right-click the Solution and select Properties > Options > NuGet Package Manager > Package Sources  here you will find Available package sources and Machine-wide package sources.  A mapping to \\magenic.net\FileShare\Installs\QATools\ (internal only) or an external reference such as Git and the Git source path will resolve the issue.


## What Updates exist?


### Version 4.0.1 has the ability for using Sauce Labs
- Extends mobile automation capability using Magenic MAQS framework

## What Integrations exist?

### Sauce Labs
- Support for and configuration of integration with Sauce Labs can be done by providing a username and accesskey for a remote site and running tests in their cloud.

### Note: Magenic MAQS utilizes NuGet
- A package manager for the Microsoft development platform which is a central repository for bundling client tools in a single process

## Explain Configs to me... please!

# Templates:  out of the box templates
- For  Appium, Database and Email (imap only), Selenium, Web Service (json & xml models), Composite (mix and match MAQS capabilities)
   -Project Level for Model which is a Page Object Model
   -Tests Level for Test class and App.config
   -Item Level for Page Object Model and Test Class (in most cases except Selenium, test class only)
### NUnit Only
   Cannot be mixed with Visual Studio unit test framework.  Locate this in Magenic's Open Test
   Choosing NUnit is good for: Full Microsoft Stack. or Another compile server such as Jenkins or Team City
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
"There are two types of file for configuring tests. *.runsettings are used for unit tests. And *.testsettings for lab environment tests, web performance and load tests, and for customizing some types of diagnostic data adapters such as Intellitrace and event log adapters"
https://msdn.microsoft.com/en-us/library/jj635153.aspx

Both test settings files are written in XML (eXtensible Markup Language).
### Run Settings Configurations
A .runsettings file can be added as a test setting to add additional configurations when running unit tests. This allows tests to be run on additional cores on a single machine, to run on different versions of the unit test framework, or to specify where the results of the test should be output.
#### Adding Run Settings
To add a .runsettings file to a test solution, go to the top toolbar and under Test → Test Settings, choose the "Select Test Settings File" option, and finally add a .runsettings file.  
![Remote Browser Settings](resources/AddNewTestSettings.png)  
Since there is no way to have Visual Studio generate a template of a .runsettings file, you can instead add an example .runsettings file to your solution such as this one found on Microsoft's MSDN website: https://msdn.microsoft.com/en-us/library/jj635153.aspx#example
### Test Settings Configurations
.test settings files are useful for collecting data from tests running across multiple platforms, specifying environmental conditions, or collecting diagnostic data.
.testsettings are always used by default in load tests and web performance tests.
#### Creating Test Settings
To create a .testsettings file, open the context menu on your solution, go to Add → New Item, select the Test Settings category and choose a Test Settings file. A wizard window will appear providing options to be configured for the .testsettings file.  
![Remote Browser Settings](resources/remotebrowsersettings.png)  
#### Adding Test Settings
To add a .testsettings file to a test solution, go to the top toolbar and under Test → Test Settings, choose the "Select Test Settings File" option, and finally add a .testsettings file.  
![Remote Browser Settings](resources/AddNewTestSettings.png)  
#### Test Settings for Selenium Grid
The main use of a .testsettings file is to control the distribution of tests over Selenium Grid, allowing tests to be executed in parallel.
### Major Differences
While a .testsettings file can be used in the same way as a .runsettings file to control settings such as framework version, target platform, or a results directory, it is mainly used for controlling the distribution of tests run in parallel over multiple machines.
.testsettings can also be configured to collect other information, such as recording video of the tests running.
.runsettings can only run a test.dll single-threaded, meaning it can only be run on one machine.
.runsettings is negligibly faster than .testsettings, but can only be used to run tests locally.
