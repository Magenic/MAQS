# Magenic's Automation Quick Start  
## About  
MAQS stands for Magenic's automation quick start.

It …
 - is a modular test automation framework
 - can be used as the base for your automation project or individual pieces can be used to enhance existing frameworks
 - is maintained/extended by automation engineers that use MAQS on a day to day bases

The main idea behind MAQS is to avoid **reinventing the wheel**. Most automation engagements have you doing the same basic steps to get a functioning framework implemented. Utilizing project templates, NuGet, and utility libraries we are able to have a functioning framework up and running in minutes, almost entirely removing on the initial time investment on implementing an automation solution.
  
![templates.PNG](templates.PNG)
  
![NewItem.PNG](NewItem.PNG)
  
![TestRun.PNG](TestRun.PNG)

## Why MAQS 
MAQS is a quick start for most any automation project. MAQS provides templates, configuration files, libraries, and helper methods to reduce the time required to initiate a test project with little setup. 
## How to use 
To create a new MAQS project, simply create a new solution using the MAQS templates and compile the project.  The templates included are compilable test solutions that includes a number of generic pre-written sample tests. 
Templates for Selenium, Appium, Web Service, Database, Email, Generic and Composite (includes all the other types) tests are all included. 
## Features 
### Generic Waits 
MAQS provides generic wait methods that can be used without having to write code for explicit/implicit waits that Selenium provides. These methods wrap up common patterns that are used when a test interacts with a page, methods that wait for dynamic page elements to appear/disappear or become clickable. 
### Logging 
MAQS provides logging in multiple file formats, as well as different logging levels depending on the importance and category of the log message. The logging levels can be used to suppress or log different kinds of information such as errors, warnings, successes, informational, generic messages, or logging to be altogether suspended. 
### Soft Asserts 
MAQS provides soft asserts that are used to store test assertions in a collection and will not fail a test until the test explicitly calls for it. This is useful for making multiple assertions, logging the results of those assertions, storing those results into a collection, and letting the tester decide how to handle any failed asserts without throwing an exception. 
### Drivers
MAQS automatically setups up and cleans up Selenium, Appium, Web Service, Database and Email drives. These drivers provide you an interface with which to interact with the underlying system under test.  MAQS is also written in such a way that you can easily add your own driver type or use multiple driver and/or driver types within the same test.

