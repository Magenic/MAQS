# <img src="resources/maqslogo.ico" height="32" width="32"> Base Email Test

## Overview
The BaseEmailTest class provides access to the EmailTestObject and EmailDriver.

# Available calls
[GetEmailConnection](#GetEmailConnection)  
[CreateNewTestObject](#CreateNewTestObject)  

## GetEmailConnection
This method gets the database connection. 
```csharp
protected virtual ImapClient GetEmailConnection()
{
    bool loggingEnabled = this.LoggingEnabledSetting != LoggingEnabled.NO;

    string host = EmailConfig.GetHost();
    string username = EmailConfig.GetUserName();
    int port = EmailConfig.GetPort();

    if (loggingEnabled)
    {
        this.Log.LogMessage(
            MessageType.INFORMATION,
            StringProcessor.SafeFormatter("Connect to email with user '{0}' on host '{1}', port '{2}'", username, host, port));
    }

    ImapClient emailConnection = ClientFactory.GetDefaultEmailClient();
    emailConnection.NoOp();

    if (loggingEnabled)
    {
        this.Log.LogMessage(MessageType.INFORMATION, "Connected to email account");
    }

    return emailConnection;
}
```

## CreateNewTestObject
This method creates an Email test object.
```csharp
 protected override void CreateNewTestObject()
{
    Logger newLogger = this.CreateLogger();
    this.TestObject = new EmailTestObject(() => this.GetEmailConnection(), newLogger, new SoftAssert(newLogger), this.GetFullyQualifiedTestClassName());
}
```