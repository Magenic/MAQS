# <img src="resources/maqslogo.ico" height="32" width="32"> Web Service Driver

## Overview
The WebServiceDriver object allows you to interact with web services.

Each web driver call has a similar format
* Request URI
  * Path beyond the base URL
  * The URI may also include query parameters
* Expected response type
  * This tells the service what format the response body should take.
* Content
  * This is optional and only needs to be used when sending content to the web service.  Typically this would not be used with **GET** calls, but would be used with **PUT** calls.
* What response we expect
  * You have two types of expects.  The first is expect success.  If you use this type and pass in true, an error will be thrown if the service returns anything but a success code. If you pass in no than it will not throw an exception.  The second type of expect is HTTP status code.  With this type of expect an error wil be thrown if the web service does not return and expected status code.  This is especially helpful when testing that the correct error code is returned.


# Available calls
[Get](##Get)  
[Put](##Put)  
[Post](##Post)  
[Patch](##Patch)  
[Delete](##Delete)  
[Custom](##Custom)

## Get
Execute a "get" call and get the response body back as a string
```csharp
string result = this.WebServiceDriver.Get("/api/String/1", "text/plain");
Assert.IsTrue(result.Contains("Tomato Soup"), "Was expecting a result with Tomato Soup but instead got - " + result);
```

Execute a "get" call and get the response body back as a specific object 
```csharp
ArrayOfProduct result = this.WebServiceDriver.Get<ArrayOfProduct>("/api/XML_JSON/GetAllProducts", "application/xml");
Assert.AreEqual(3, result.Product.Length, "Expected 3 products to be returned");
```

Execute a "get" call and get back the HTTP response
```csharp
HttpResponseMessage result = this.WebServiceDriver.GetWithResponse("/api/PNGFile/GetImage?image=Red", "image/png");

// Get the image
Image image = Image.FromStream(result.Content.ReadAsStreamAsync().Result);
Assert.AreEqual(200, image.Width, "Image width should be 200");
Assert.AreEqual(200, image.Height, "Image hight should be 200");
```
## Put
Execute a "put" call and get the response body back as a string
```csharp
StreamContent content = WebServiceUtils.MakeStreamContent("Test", Encoding.UTF8, "text/plain");
string result = this.WebServiceDriver.Put("/api/String/Put/1", "text/plain", content);
Assert.AreEqual(string.Empty, result);
```
Execute a "put" call and get the response body back as a specific object 
```csharp
ProductJson p = new ProductJson
{
    Category = "ff",
    Id = 4,
    Name = "ff",
    Price = 3.25f
};
var content = WebServiceUtils.MakeStringContent<ProductJson>(p, Encoding.UTF8, "application/json");
ProductJson result = this.WebServiceDriver.Put<ProductJson>("/api/XML_JSON/Put/1", "application/json", content);
Assert.AreEqual(null, result)
```

Execute a "put" call and get back the HTTP response
```csharp
HttpResponseMessage result = this.WebServiceDriver.PutWithResponse("/api/String/Put/1", "text/plain", "Test", Encoding.UTF8, "text/plain");
Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
```

## Post
Execute a "post" call and get the response body back as a string
```csharp
string result = this.WebServiceDriver.Post("/api/String", "text/plain", "Test", Encoding.UTF8, "text/plain");
Assert.AreEqual(string.Empty, result);
```
Execute a "post" call and get the response body back as a specific object 
```csharp
ProductJson p = new ProductJson
{
    Category = "ff",
    Id = 4,
    Name = "ff",
    Price = 3.25f
};
var content = WebServiceUtils.MakeStreamContent<ProductJson>(p, Encoding.UTF8, "application/json");
var result = this.WebServiceDriver.Post<ProductJson>("/api/XML_JSON/Post", "application/json", content);
Assert.IsTrue(result == null);
```

Execute a "post" call and get back the HTTP response
```csharp
HttpResponseMessage result = this.WebServiceDriver.PutWithResponse("/api/String/Put/1", "text/plain", "Test", Encoding.UTF8, "text/plain", true);
Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
```

## Patch
Execute a "patch" call and get the response body back as a string
```csharp
string result = this.WebServiceDriver.Patch("/api/String/Patch/1", "text/plain", "Test", Encoding.UTF8, "text/plain");
Assert.AreEqual("\"Patched\"", result);
```
Execute a "patch" call and get the response body back as a specific object 
```csharp
Product p = new Product
{
    Category = "ff",
    Id = 4,
    Name = "ff",
    Price = 3.25f
};

var content = WebServiceUtils.MakeStringContent<Product>(p, Encoding.UTF8, "application/xml");

Product result = this.WebServiceDriver.Patch<Product>("/api/XML_JSON/Patch/1", "application/xml", content, true);

Assert.AreEqual(p.Category, result.Category);
Assert.AreEqual(p.Id, result.Id);
Assert.AreEqual(p.Name, result.Name);
Assert.AreEqual(p.Price, result.Price);
```

Execute a "patch" call and get back the HTTP response
```csharp
Product p = new Product
{
    Category = "ff",
    Id = 4,
    Name = "ff",
    Price = 3.25f
};

var content = WebServiceUtils.MakeStreamContent<Product>(p, Encoding.UTF8, "application/xml");
HttpResponseMessage result = this.WebServiceDriver.PatchWithResponse("/api/XML_JSON/Patch/1", "application/xml", content);

Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
```
## Delete
Execute a "get" call and get the response body back as a string
```csharp
string result = this.WebServiceDriver.Delete("/api/String/Delete/1", "text/plain", true);
Assert.AreEqual(string.Empty, result);
```

Execute a "get" call and get the response body back as a specific object 
```csharp
ProductJson result = this.WebServiceDriver.Delete<ProductJson>("/api/XML_JSON/Delete/1", "application/json");
Assert.AreEqual(result, null);
```

Execute a "get" call and get back the HTTP response
```csharp
HttpResponseMessage result = this.WebServiceDriver.DeleteWithResponse("/api/String/Delete/43", "text/plain", false);
Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
```


## Custom
Execute a custom call type and get the response body back as a string
```csharp
var content = WebServiceUtils.MakeStringContent("ZEDTest", Encoding.UTF8, "text/plain");
string result = this.WebServiceDriver.Custom("ZED", "/api/ZED", "text/plain", content, true);

Assert.AreEqual("\"ZEDTest\"", result.ToString());
```

Execute a custom call type and get the response body back as a specific object 
```csharp
var content = WebServiceUtils.MakeStringContent("ZEDTest", Encoding.UTF8, "text/plain");
string result = this.WebServiceDriver.Custom<string>("ZED", "/api/ZED", "text/plain", content, true);

Assert.AreEqual("ZEDTest", result.ToString());
```

Execute a custom call type and get back the HTTP response
```csharp
var content = WebServiceUtils.MakeStringContent("ZED?", Encoding.UTF8, "application/json");
HttpResponseMessage result = this.WebServiceDriver.CustomWithResponse("ZED", "/api/ZED", "application/json", content.ToString(), Encoding.UTF8, "application/json", true, false);

Assert.AreEqual(HttpStatusCode.UseProxy, result.StatusCode);
```




