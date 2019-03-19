# <img src="resources/maqslogo.ico" height="32" width="32"> Faker Data

## Overview
Faker Data provides the ability to generate random and valid (wherever applicable) data during runtime.

### Time Utilities
For getting current system time, you can use:  

#### Written As

```csharp
FakerData.GenerateInstantSpecificTime();  
```

#### Examples
```csharp
string S = FakerData.GenerateInstantSpecificTime().toString();   
Console.WriteLine(S);
// Output: 08/06/2018 15:46:13 
```

### Data Generation

#### IDs
To generate a unique ID, you can use:
##### Written As

```csharp
FakerData.GenerateUniqueId();  
```
##### Examples
```csharp
string S = FakerData.GenerateUniqueId().toString();   
Console.WriteLine(S);
// Output: 118c4cf9-49b9-4d34-8f6a-0185990dbf86

```
#### Phone Number
To generate a valid US phone number, you can use:

##### Written As

```csharp
FakerData.GenerateUSPhoneNumber();  
```
##### Examples
```csharp
string regexFormat = @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}";
Assert.IsTrue(Regex.IsMatch(FakerData.GenerateUSPhoneNumber(true), regexFormat));
```
#### Social Security Number
##### Written As
To generate a valid Social Security number, you can use:

```csharp
FakerData.GenerateSocialSecurityNumber(withDashesBoolean);  
```
##### Examples
```csharp
string regexFormat = @"\d{3}-{0,1}\d{2}-{0,1}\d{4}";
Assert.IsTrue(Regex.IsMatch(FakerData.GenerateSocialSecurityNumber(true), regexFormat));
```

#### Random Number from a List
##### Written As

To generate a random value from a list, you can use:

```csharp
FakerData.GeneralRandomizer(stringList);  
```
#### Examples
```csharp
List<int> intList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
int randomInt = FakerData.GeneralRandomizer(intList);
Assert.IsTrue(intList.Contains(randomInt));
```

