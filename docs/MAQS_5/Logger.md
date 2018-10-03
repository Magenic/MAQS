# <img src="resources/maqslogo.ico" height="32" width="32"> Logger


## Overview
The Logger captures test execution information.   
_*By default, log files end up in the "log" folder. This is located in the same folder as the test DLL._

###  Types of Loggers

 - Console Logger
 - File Logger
    - File (.txt)
    - HTML (.html)

###  Console Logger
Writes test execution logging information to the console

###  File Logger
Writes test execution logging information as a .txt file   

*Naming convention: _FullyQualifiedName - UTCTime_*

#### Examples
```
CompositeUnitTests.Base.CanRunTest - 2018-08-07-02-01-28-3500.txt 
```
###  HTML Logger
Writes test execution logging information as a html file

*Naming convention: _FullyQualifiedName - UTCTime_*

#### Examples
```
CompositeUnitTests.Base.CanRunTest - 2018-08-07-02-01-28-3500.html 
```

## How to Use Logger

### Logging with MessageTypes
##### Written As

```csharp
        [TestMethod]
        public void TestWithLogging() 
        {
            this.Log.LogMessage(MessageType.VERBOSE, "Verbose logging message");
            this.Log.LogMessage(MessageType.INFORMATION, "Information logging message");
            this.Log.LogMessage(MessageType.GENERIC, "Generic logging message");
            this.Log.LogMessage(MessageType.SUCCESS, "Success logging message");
            this.Log.LogMessage(MessageType.WARNING, "Warning logging message");
            this.Log.LogMessage(MessageType.ERROR, "Error logging message"); 
        }
```

### Logging without MessageTypes
*Messages that don't provide MessageTypes are categorized as generic messages.*
##### Written As

```csharp
        [TestMethod]
        public void TestWithLogging() 
        {
            this.Log.LogMessage("Generic massage"); 
        }
```

## Changing Logging Level
Ability to dynamically change logging level at runtime.
##### Written As

```csharp
              // Change your logging level
            this.Log.SetLoggingLevel(MessageType.WARNING);
            this.Log.LogMessage(MessageType.GENERIC, "Will not be logged");
```
## Suspend and Resume Logging
Ability to Suspend and Resume Logging.
##### Written As

```csharp
            this.Log.LogMessage(MessageType.ERROR, "Logged"); 
            this.Log.SuspendLogging();
            this.Log.LogMessage(MessageType.ERROR, "Not Logged"); 
            this.Log.ContinueLogging(); 
            this.Log.LogMessage(MessageType.ERROR, "Logged"); 
```
