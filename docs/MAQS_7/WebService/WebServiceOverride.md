# <img src="resources/maqslogo.ico" height="32" width="32"> Overriding The web service driver

## Overriding the web service driver 
By default, BaseWebServiceTest will create a web service driver for you based on your [configuration](MAQS_6/WebService/WebServiceConfig.md). Authentication related requirements often require users to override the default web service client.  This is why we provide several different ways for you to provide your own web service driver implementation.

There are three primary ways to override the web service client.

### Override the base web service test get HTTP client function
```csharp
[TestClass]
public class YOURTESTCLASS : BaseWebServiceTest
{
    /// <summary>
    /// Get the lower level HTTP Client
    /// </summary>
    /// <returns>The web service driver</returns>
    protected override HttpClient GetHttpClient()
    {
        return YourGetHttpClientFunction();
    }
```
### Override how to get the driver
```csharp
// Override with a function call
this.TestObject.OverrideWebServiceDriver(YourGetHttpClientFunction);

// Override with a lambda expression
this.TestObject.OverrideWebServiceDriver(() => HttpClientFactory.GetClient(new Uri(NEWADDRESS), WebServiceConfig.GetWebServiceTimeout()));
```
*_**The above examples do lazy instantiation of the web service driver - AKA You only create a driver if/when you use it**_  

### Override the driver directly
```csharp
// Override with a driver
HttpClient client = YourGetHttpClientFunction();
this.TestObject.OverrideWebServiceDriver(client);

// Override the driver directly 
HttpClient anotherClient = YourGetHttpClientFunction();
this.WebServiceDriver = new WebServiceDriver(anotherClient);

```
*_**Overriding the driver is not advised because it doesn't lazy load the web service driver and only provides limited logging capabilities**_  
