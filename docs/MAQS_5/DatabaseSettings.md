# <img src="resources/maqslogo.ico" height="32" width="32"> Database Configuration

## Overview
The DatabaseConfig class is used to get values from the DatabaseMaqs section of your test run properties.
<br>These values come from your App.config, appsettings.json and/or test run parameters.

## DatabaseMaqs
The DatabaseMaqs configuration section contains the following Keys:

* ***DataBaseProviderType*** : The database specific .NET ADO provider
* ***DataBaseConnectionString*** : The provider specific connection string

## DataBaseProviderType

MAQS with Dapper supports the following providers:

* ***SQL Server***
* ***PostgreSql***
* ***SQLite***

## Available methods
Get the database connection string:
```csharp
string connection = DatabaseConfig.GetConnectionString();
```

Get the database provider:
```csharp
string provider = DatabaseConfig.GetProviderTypeString();
```

Get a database connection using the provide and connection string from the configuration:
```csharp
IDbConnection connection = DatabaseConfig.GetOpenConnection();
```

Get a database connection using a passed in provide and connection string:
```csharp
IDbConnection connection = DatabaseConfig.GetOpenConnection("SQLITE", $"Data Source={GetDByPath()}");
```

# Sample config files
## App.config
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="DatabaseMaqs" type="System.Configuration.NameValueSectionHandler" />
    <section name="MagenicMaqs" type="System.Configuration.NameValueSectionHandler" />
  </configSections>
  <DatabaseMaqs>
    <!--<add key="DataBaseProviderType" value="SQLSERVER" />
    <add key="DataBaseConnectionString" value="Data Source=DB;Initial Catalog=MagenicAutomation;Persist Security Info=True;User ID=ID;Password=PW;Connection Timeout=30" />   
    <add key="DataBaseProviderType" value="POSTGRE" />
    <add key="DataBaseConnectionString" value="Server=127.0.0.1;Port=1234;Database=maqs;User Id=UserID;Password=PW;" />    
    <add key="DataBaseProviderType" value="SQLITE" />
    <add key="DataBaseConnectionString" value="Data Source=PATH\TO\MyDatabase.sqlite;" />-->
    <add key="DataBaseProviderType" value="SQLSERVER" />
    <add key="DataBaseConnectionString" value="CONNECTION" />
  </DatabaseMaqs>
  <MagenicMaqs>
    <!-- Generic wait time in milliseconds - AKA how long do you wait for rechecking something -->
    <add key="WaitTime" value="1000" />

    <!-- Generic time-out in milliseconds -->
    <add key="Timeout" value="10000" />

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
    <add key="LogLevel" value="INFORMATION" />

    <!-- Logging Types
    <add key="LogType" value="CONSOLE"/>
    <add key="LogType" value="TXT"/>
    <add key="LogType" value="HTML"/>-->
    <add key="LogType" value="TXT" />

    <!-- Log file path - Defaults to build location if no value is defined
    <add key="FileLoggerPath" value="C:\Frameworks\"/>-->
  </MagenicMaqs>
</configuration>
```
## appsettings.json
```json
{
  "DatabaseMaqs": {
    "DataBaseProviderType": "SQLSERVER",
    "DataBaseConnectionString": "Data Source=DATABASE;Initial Catalog=TEST_DB;Persist Security Info=True;User ID=USER_ID;Password=USER_PASSWORD;Connection Timeout=30"
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