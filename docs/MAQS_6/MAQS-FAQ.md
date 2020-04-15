# <img src="resources/maqslogo.ico" height="32" width="32"> FAQ

# Open source VS enterprise version
As of July 2019, MAQS became fully open source.  
There are no longer separate open source and enterprise versions.

# Templates: Out of the box templates
Templates can be found in the [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=vs-publisher-1465771.MAQSOpenFramework) or  Visual Studio Tools -> Extensions and Updates.

## Template types
- DotNet Framework project templates - These templates are available to Magenic clients
   - Appium (with tests and page object models) 
   - Base
   - Composite (mix and match all MAQS capabilities)  
   - Database 
   - Email (IMAP only)  
   - Selenium (with tests and page object models) 
   - Web Service (with tests, json models and xml models)  
- DotNet Framework item templates  
   - Appium tests (Visual Studio and NUnit versions)
   - Appium page object model
   - Base tests (Visual Studio and NUnit versions)  
   - Database tests (Visual Studio and NUnit versions)
   - Email tests (Visual Studio and NUnit versions) 
   - Selenium tests (Visual Studio and NUnit versions)
   - Selenium page object model
   - Web Service tests (Visual Studio and NUnit versions)
- DotNet Core project templates - https://www.nuget.org/packages/Magenic.Maqs.Templates 
   - Appium (with tests and page object models) 
   - Base
   - Composite (mix and match all MAQS capabilities)  
   - Database 
   - Email (IMAP only)  
   - Selenium (with tests and page object models) 
   - Web Service (with tests, json models and xml models) 


# Base features
## Logging
- Yes, No, or On Fail and the Defaults are  Log = No, LogType = Console LogLevel = INFORMATION and FileLoggerPath = Test dll "log" sub folder    File Format options: txt, html, console
### Log Level
![The levels of logs](resources/logleveldiagram.png)  
Each log level is grouped into a hierarchy, with the highest log levels also including any messages from lower log levels in the hierarchy.
Verbose mode includes all information gathered by the logger, which includes any navigation that occurs on the page, and anytime the WebDriver attempts to find an element or interact with an element. Verbose mode will result in many, often superfluous, lines of information.
Information mode includes messages for when a WebDriver is loaded, a value is identified, an element is interacted with, or whenever the WebDriver throws an exception.
Generic mode is the default logging level. It includes performance timers, and log interactions such as when a performance timer log is saved.
Success mode will output a message anytime a soft assert is successful, or as a test ends and the test is successful.
Warning mode will output a message anytime a soft assert fails, the test is met with unexpected results, or the test configuration fails to update.
Error mode will only display messages that would fail a test. This includes a test failed, a test resulted in being inconclusive, a setup failed, or the final soft assert data contains any failed soft asserts.
Suspended mode will result in no information written to the log. 

## Configuration
- Configuration overrides from MSTest (properties) or NUnit (parameters) are only executed within the context of a test run. If you are using the configuration outside a test, such as part of an assembly initialize, you will need to trigger the update your self by calling [UpdateWithVSTestContext](MAQS_6/Utilities/Config.md#UpdateWithVSTestContext) or  [UpdateWithNUnitTestContext](MAQS_6/Utilities/Config.md#UpdateWithNUnitTestContext) depending on which test runner you are using.

## Soft Asserts
- Specific to a test (inside) These get logged to the Log and become Hard Asserts after use. AreEqual or FailTestIfAssertFailed or Assert.AreEqual  helps your testing approach be more efficient instead of having to fix individual failures.

## Performance Measures
- Human readable and requires a Name, Embedded Perf tests to start at different times, Basically it tells how long the data collection took using this as a stop watch

## TestObject
- Holds test specific objects and available in all Magenic MAQS flavors

## Waits
- GenericWait.WaitFor waits until exceptions are no longer thrown (calls the function N number of times) or GenericWait.WaitUntil  responds with true / false

## Customizations
- Utilizes the app.config which is Globally used information, avoids hard-coded code and you can add Key|Value custom pairs such as UserName =YOU PassWord=ABC   We also allow you to use 'default' 

# Old Features
## Faker Data
- All data faking functionality has been removed from MAQS.
- We strongly encourage you to leverage the [Faker.Data](https://www.nuget.org/packages/Faker.Data/) NuGet package in it's place.

## PhantomJS
- The PhantomJS project has been archived and Selenium support has been deprecated.
- Headless Chrome is the primary replacement for PhantomJS  

# Test Run Settings - MSTest 
There are two types of file for configuring tests. *.runsettings are used for unit tests. And *.testsettings for lab environment tests, web performance and load tests, and for customizing some types of diagnostic data adapters such as IntelliTrace and event log adapters.  
https://docs.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file?view=vs-2017.  
Both test settings files are written in XML (eXtensible Markup Language).  
*_**TestSettings files have been deprecated and should no longer be used!**_  


## Run Settings Configurations
A .runsettings file can be added as a test setting to add additional configurations when running unit tests. This allows tests to be run on additional cores on a single machine, to run on different versions of the unit test framework, or to specify where the results of the test should be output.
### Adding Run Settings
To add a .runsettings file to a test solution, go to the top toolbar and under Test → Test Settings, choose the "Select Test Settings File" option, and finally add a .runsettings file.  
![Remote Browser Settings](resources/AddNewTestSettings.png)  
Since there is no way to have Visual Studio generate a template of a .runsettings file, you can instead add an example .runsettings file to your solution such as this one found on Microsoft's MSDN website: https://docs.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file?view=vs-2017

# MSTest V2 documentation

https://github.com/Microsoft/testfx

# NUnit documentation

https://github.com/nunit/docs/wiki/