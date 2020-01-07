# <img src="resources/maqslogo.ico" height="32" width="32"> Web Service Test Object

## Overview
Takes care of the base test context data.

[WebServiceDriver](#WebServiceDriver)
[WebServiceManager](#WebServiceManager)  
[OverrideWebServiceDriver](#OverrideWebServiceDriver)

## WebServiceDriver
Gets the web service driver
```csharp
this.TestObject.WebServiceDriver
```

## WebServiceManager
Gets the web service driver manager
```csharp
this.TestObject.WebServiceDriver
```

## OverrideWebServiceDriver
Override the http client
```csharp
public void OverrideWebServiceDriver(HttpClient httpClient)
{
	this.OverrideDriverManager(typeof(WebServiceDriverManager).FullName, new WebServiceDriverManager(() => httpClient, this));
}
```

Override the http client driver
```csharp
public void OverrideWebServiceDriver(WebServiceDriver webServiceDriver)
{
    (this.ManagerStore[typeof(WebServiceDriverManager).FullName] as WebServiceDriverManager).OverrideDriver(webServiceDriver);
}
```