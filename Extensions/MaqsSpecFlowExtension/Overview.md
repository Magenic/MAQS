# MAQS SpecFlow Extension
## About  
The SpecFlow extension add BDD/ATDD templates for [MAQS](https://github.com/Magenic/MAQS).

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

