# <img src="resources/maqslogo.ico" height="32" width="32"> Config

## Overview
Config is class file granting access to properties in the App.config. 

## Available methods
If you need use value for key named 'input' from the App.config file, you can write:

```csharp
Config.GetValue("input”);  
```

If you want to return a default value if "input" key is not found, you can write:

```csharp
Config.GetValue("input”, "DefaultValueToBeReturned");  
```

In some cases, testers may need to add a dynamic value in the Configuration in order to use it later in the test. In order to do that, use the following method:
```csharp
Config.AddTestSettingValues(IDictionary<string, string> configurations, bool overrideExisting = false);
``` 

You can also check if the Config contains value for a key by using: 
```csharp
Config.DoesKeyExist("key");
``` 



