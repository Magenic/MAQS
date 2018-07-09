# <img src="resources/maqslogo.ico" height="32" width="32"> Faker Data

## Overview
Faker Data provides the ability to generate random and valid (wherever applicable) data during runtime.

## Time Utilities
For getting current system time, write:

```csharp
FakerData.GenerateInstantSpecificTime();  
```
## Data Generation
To generate a unique id, you can use:

```csharp
FakerData.GenerateUniqueId();  
```
To generate a valid US phone number, you can use:

```csharp
FakerData.GenerateUSPhoneNumber();  
```

To generate a valid Social Securuty number, you can use:

```csharp
FakerData.GenerateSocialSecurityNumber(withDashesBoolean);  
```
To generate a random value from a list, you can use:

```csharp
FakerData.GeneralRandomizer(stringList);  
```


