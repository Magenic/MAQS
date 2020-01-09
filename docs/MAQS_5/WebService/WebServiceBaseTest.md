# <img src="resources/maqslogo.ico" height="32" width="32"> Base Web Service Test

## Overview
The BaseEmailTest class provides access to the EmailTestObject and EmailDriver.

# Available calls
[WebServiceDriver](#WebServiceDriver)  
[GetHttpClient](#GetHttpClient)  
[GetBaseWebServiceUri](#GetBaseWebServiceUri)  
[GetBaseWebServiceUrl](#GetBaseWebServiceUrl)  
[GetUseProxy](#GetUseProxy)  
[GetProxyAddress](#GetProxyAddress)  
[CreateNewTestObject](#CreateNewTestObject)  

## WebServiceDriver
Gets or sets the web service driver
```csharp
public WebServiceDriver WebServiceDriver
{
    get
    {
        return this.TestObject.WebServiceDriver;
    }

    set
    {
        this.TestObject.OverrideWebServiceDriver(value);
    }
}
```

## GetHttpClient
Get a new http client
```csharp
 HttpClient baseClient = base.GetHttpClient();
```

## GetBaseWebServiceUri
Get the base web service url
```csharp
 Uri uri = this.GetBaseWebServiceUri();
```

## GetBaseWebServiceUrl
Get the base web service url
```csharp
string url = this.GetBaseWebServiceUrl();
```

## GetUseProxy
Get if proxy should be used
```csharp
bool useProxy = WebServiceConfig.GetUseProxy();
```

## GetProxyAddress
Get proxy address
```csharp
bool findProxyAddress = WebServiceConfig.GetProxyAddress();
```

## CreateNewTestObject
Create a web service test object
```csharp
protected override void CreateNewTestObject()
{
    Logger newLogger = this.CreateLogger();
    this.TestObject = new WebServiceTestObject(() => this.GetHttpClient(), newLogger, this.GetFullyQualifiedTestClassName());
}
```