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

### Get Specific Section Value

This pulls configuration values from the provided section of your app.config. It will return an emtpy string if the key is not found in the app.config.

```csharp
Config.GetValueForSection("SeleniumMaqs", "Key"))
```
This pulls configuration values from the provided section of your app.config. It will return the default value if the key is not found in the app.config.

```csharp
Config.GetValueForSection("SeleniumMaqs", "Key", "DefaultValueToBeReturned");
```

### Checking if key exists
Does a key exist in the MagenicMaqs section of the config file.

```csharp
Config.DoesKeyExist("Key");
```
Does a key exist in the specified section of the config file.

```csharp
Config.DoesKeyExist("Key", "SeleniumMaqs");    
```

### Add or Override configurations
Ability to Add or Override settings for MagenicMaqs section of the config.

```csharp
        Dictionary<string, string> overrides = new Dictionary<string, string>();
            overrides.Add("Key", "Value");
            Config.AddGeneralTestSettingValues(overrides); 
```
Ability to Add or Override settings for the specified section of the config.

```csharp
        Dictionary<string, string> overrides = new Dictionary<string, string>();
            overrides.Add("Key", "Value");
            Config.AddTestSettingValues(overrides,"SeleniumMaqs"); 
```
