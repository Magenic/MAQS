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