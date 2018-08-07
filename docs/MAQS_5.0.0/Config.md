# <img src="resources/maqslogo.ico" height="32" width="32"> Config

## Overview
Config is class file granting access to properties in the App.config. 

## Available methods
### Get General Value
This pulls configuration values from MagenicMaqs section of your app.config. It will return an emtpy string if the key is not found in the app.config.

```csharp
Config.GetGeneralValue("Key”);  
```

This pulls configuration values from MagenicMaqs section of your app.config. It will return the default value if the key is not found in the app.config.
```csharp
Config.GetGeneralValue("Key", "DefaultValueToBeReturned");  
```

This pulls configuration values from the provided section of your app.config. It will return an emtpy string if the key is not found in the app.config.

```csharp
Config.GetValueForSection("SeleniumMaqs", "Key"))

```
This pulls configuration values from the provided section of your app.config. It will return the default value if the key is not found in the app.config.

```csharp
Config.GetValueForSection("SeleniumMaqs", "Key", "DefaultValueToBeReturned");
```












In some cases, testers may need to add a dynamic value in the Configuration in order to use it later in the test. In order to do that, use the following method:
```csharp
Config.AddTestSettingValues(IDictionary<string, string> configurations, bool overrideExisting = false);
``` 

You can also check if the Config contains value for a key by using: 
```csharp
Config.DoesKeyExist("key");
``` 

Assert.AreEqual("SAMPLEGenz", Config.GetValueForSection("MagenicMaqs", "SectionAdd"));
            Assert.AreEqual("SAMPLEGen", Config.GetGeneralValue("SectionOverride"));
            Assert.AreEqual("SAMPLEAppz", Config.GetValueForSection("AppiumMaqs", "SectionAdd")); 

