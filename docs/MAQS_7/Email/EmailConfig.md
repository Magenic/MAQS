# <img src="resources/maqslogo.ico" height="32" width="32"> Email Configuration

## Overview
The Email Config class is used to get values from the EmailMaqs section of your test run properties.
<br>These values come from your App.config, appsettings.json and/or test run parameters.

## EmailMaqs
The EmailMaqs configuration section contains the following Keys:

* ***EmailHost*** : The email host path.
* ***EmailUserName*** : The email username.
* ***EmailPassword*** : The email password.
* ***EmailPort*** : The email port.
* ***ConnectViaSSL*** : If connection via SSL should happen.
* ***SkipSslValidation*** : If skipping SSL validation should happen.
* ***AttachmentDownloadPath*** : The attachment download path.
* ***EmailTimeout*** : How long to wait for something before timing out - Used heavily with the MAQS waits

## Available methods
Get the email the host:
```csharp
string host = EmailConfig.GetHost();
```

Get the email username:
```csharp
string username = EmailConfig.GetUserName();
```

Get the email password:
```csharp
string password = EmailConfig.GetPassword();
```

Get the email port:
```csharp
int port = EmailConfig.GetPort();
```

Get if connection via SSL should occur:
```csharp
bool ssl = EmailConfig.GetEmailViaSSL();
```

Get if connection should skip SSL validation:
```csharp
bool skipSsl = EmailConfig.GetEmailSkipSslValidation();
```

Get string of the file path:
```csharp
string downloadDirectory = EmailConfig.GetAttachmentDownloadDirectory();
```

Get the email port:
```csharp
TimeSpan timeout = EmailConfig.GetTimeout();
```

# Sample config files
## App.config
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="MagenicMaqs" type="System.Configuration.NameValueSectionHandler" />
    <section name="EmailMaqs" type="System.Configuration.NameValueSectionHandler" />
  </configSections>
  <MagenicMaqs>
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
    <add key="LogLevel" value="VERBOSE" />

    <!-- Logging Types
    <add key="LogType" value="CONSOLE"/>
    <add key="LogType" value="TXT"/>
    <add key="LogType" value="HTML"/>-->
    <add key="LogType" value="TXT" />

    <!-- Log file path - Defaults to build location if no value is defined
    <add key="FileLoggerPath" value="C:\Frameworks\"/>-->

    <!--Retry and overall timeout in milliseconds-->
    <add key="WaitTime" value="1000" />
    <add key="Timeout" value="10000" />
  </MagenicMaqs>
  <EmailMaqs>
    <!--IMAP connection settings-->
    <add key="EmailHost" value="imap.gmail.com" />
    <add key="EmailUserName" value="maqsfakeemailtest@gmail.com" />
    <add key="EmailPassword" value="Magenic3" />
    <add key="EmailPort" value="993" />
    <add key="ConnectViaSSL" value="Yes" />
    <add key="SkipSslValidation" value="Yes" />

    <!--Email attachment download folder-->
    <add key="AttachmentDownloadPath" value="C:\Frameworks\downloads" />

    <!-- Time-out in milliseconds -->
    <add key="EmailTimeout" value="10000" />
  </EmailMaqs>
</configuration>
  </MagenicMaqs>
</configuration>

```
## appsettings.json
```json
{
  "EmailMaqs": {
    "EmailHost": "imap.gmail.com",
    "EmailUserName": "maqsfakeemailtest@gmail.com",
    "EmailPassword": "Magenic3",
    "EmailPort": "993"
    "ConnectViaSSL": "Yes"
    "SkipSslValidation": "Yes"
    "AttachmentDownloadPath": "C:\Frameworks\downloads"
  },
  "MagenicMaqs": {
    "WaitTime": "100",
    "Timeout": "10000",
    "Log": "OnFail",
    "LogLevel": "INFORMATION",
    "LogType": "TXT"
  }
}
```