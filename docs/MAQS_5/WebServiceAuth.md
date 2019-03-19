# <img src="resources/maqslogo.ico" height="32" width="32"> Web Service Authentication

## Overview
Testing web services often requires your test to authenticate. There are many different authentication standards, such as form-based, NTML, SAML, OAuth 1, OAuth 2, etc. to name just a few. This is far more than we could realistically cover in this wiki.  Rather than trying to cover all the possible authentication types we will go over this topic from a high level.  

# Available methods
[Adding HTTP client authentication](##Adding-HTTP-client-authentication)  
[Using custom HTTP client in a single test](##Using-custom-HTTP-client-in-a-single-test)  
[Using custom HTTP client across inherited tests](##Using-custom-HTTP-client-across-inherited-tests)  


## Adding HTTP client authentication
Adding authentication to your HTTP client
### Adding builtin authorization header
```csharp
HttpClient client = new HttpClient { BaseAddress = new Uri(WebServiceConfig.GetWebServiceUri()) };
var byteArray = Encoding.ASCII.GetBytes("username:password1234");
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
```

### Adding non-builtin authorization header
```csharp
HttpClient client = new HttpClient { BaseAddress = new Uri(WebServiceConfig.GetWebServiceUri()) };
client.DefaultRequestHeaders.Add("AUTH", "KEY");
```

##  Using custom HTTP client in a single test

### Replace the default HTTP client
```csharp
[TestMethod]
public void SampleTest()
{
    // Direct override just the underlying HTTP client, does not lazy load the HTTP client
    HttpClient client = new HttpClient { BaseAddress = new Uri(WebServiceConfig.GetWebServiceUri()) };
    client.DefaultRequestHeaders.Add("AUTH", "KEY");
    this.WebServiceDriver.HttpClient = client;

    ProductXml result = this.WebServiceDriver.Get<ProductXml>("/api/XML_JSON/GetProduct/1", "application/xml", false);
    Assert.AreEqual(1, result.Id, "Expected to get product 1");
}
```

##  Using custom HTTP client across inherited tests
In this section we will show you how you can update your HTTP client across multiple tests. We accomplish this by having your tests extend a custom base test. To put it another way, you create a base class on top of BaseWebServiceTest that handles authentication.


### Replace the default HTTP client
```csharp
/// <summary>
/// Web service base test
/// </summary>
public class BaseWebService : BaseWebServiceTest
{
    [TestInitialize]
    public void HttpSetup()
    {
        // Direct override just the underlying HTTP client, does not lazy load the HTTP client
        this.WebServiceDriver.HttpClient = GetCustomHttpClient();
    }

    private HttpClient GetCustomHttpClient()
    {
        // Your auth or custom config stuff
        HttpClient client = new HttpClient { BaseAddress = new Uri(WebServiceConfig.GetWebServiceUri()) };
        client.DefaultRequestHeaders.Add("AUTH", "KEY");
        return client;
    }
}

[TestClass]
public class WebServiceTest : BaseWebService
{
    /// <summary>
    /// Get single product as XML
    /// </summary>
    [TestMethod]
    public void GetXmlDeserialized()
    {
        ProductXml result = this.WebServiceDriver.Get<ProductXml>("/api/XML_JSON/GetProduct/1", "application/xml", false);

        Assert.AreEqual(1, result.Id, "Expected to get product 1");
    }
}
```
### Replace the web service driver in the your manager store
```csharp
/// <summary>
/// Web service base test
/// </summary>
public class BaseWebService : BaseWebServiceTest
{
    [TestInitialize]
    public void HttpSetup()
    {
        // Override WebServiceDriver and lazy loads the initialization of the HTTP client
        ManagerStore.AddOrOverride(new WebServiceDriverManager(() => GetCustomHttpClient(), TestObject));
    }

    private HttpClient GetCustomHttpClient()
    {
        // Your auth or custom config stuff
        HttpClient client = new HttpClient { BaseAddress = new Uri(WebServiceConfig.GetWebServiceUri()) };
        client.DefaultRequestHeaders.Add("AUTH", "KEY");
        return client;
    }
}

[TestClass]
public class WebServiceTest : BaseWebService
{
    /// <summary>
    /// Get single product as XML
    /// </summary>
    [TestMethod]
    public void GetXmlDeserialized()
    {
        ProductXml result = this.WebServiceDriver.Get<ProductXml>("/api/XML_JSON/GetProduct/1", "application/xml", false);

        Assert.AreEqual(1, result.Id, "Expected to get product 1");
    }
}
```
### Create new web service driver
```csharp
/// <summary>
/// Web service base test
/// </summary>
public class BaseWebService : BaseWebServiceTest
{
    [TestInitialize]
    public void HttpSetup()
    {
        // Override the web service driver, does not lazy load and can loose some logging
        this.WebServiceDriver = new WebServiceDriver(GetCustomHttpClient());
    }

    private HttpClient GetCustomHttpClient()
    {
        // Your auth or custom config stuff
        HttpClient client = new HttpClient { BaseAddress = new Uri(WebServiceConfig.GetWebServiceUri()) };
        client.DefaultRequestHeaders.Add("AUTH", "KEY");
        return client;
    }
}

[TestClass]
public class WebServiceTest : BaseWebService
{
    /// <summary>
    /// Get single product as XML
    /// </summary>
    [TestMethod]
    public void GetXmlDeserialized()
    {
        ProductXml result = this.WebServiceDriver.Get<ProductXml>("/api/XML_JSON/GetProduct/1", "application/xml", false);

        Assert.AreEqual(1, result.Id, "Expected to get product 1");
    }
}
```