# <img src="resources/maqslogo.ico" height="32" width="32"> Web Service Utilities

## Overview
The WebServiceUtils class is a utility class for working with HTTP content and serialization 

# Available methods
[MakeStringContent](#MakeStringContent)  
[MakeStreamContent](#MakeStreamContent)  
[MakeNonStandardStreamContent](#MakeNonStandardStreamContent)  
[DeserializeXmlDocument](#DeserializeXmlDocument)  
[DeserializeJson](#DeserializeJson)  
[DeserializeResponse](#DeserializeResponse)  

##  MakeStringContent
Turn a string into string content
```csharp
var content = WebServiceUtils.MakeStringContent("Test", Encoding.UTF8, "text/plain");
var result = this.WebServiceDriver.Post("/api/String", "text/plain", content, true);
Assert.AreEqual(string.Empty, result);
```
Turn an object into string content
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
```

## MakeStreamContent
Turn a string into stream content
```csharp
StreamContent content = WebServiceUtils.MakeStreamContent("Test", Encoding.UTF8, "text/plain");
var result = this.WebServiceDriver.Put("/api/String/Put/1", "text/plain", content, true);
Assert.AreEqual(string.Empty, result);
```
Turn an object into stream content
```csharp
ProductJson p = new ProductJson
{
    Category = "ff",
    Id = 4,
    Name = "ff",
    Price = 3.25f
};

var content = WebServiceUtils.MakeStreamContent<ProductJson>(p, Encoding.UTF8, "application/json");
ProductJson result = this.WebServiceDriver.Post<ProductJson>("/api/XML_JSON/Post", "application/json", content, true);
Assert.IsTrue(result == null);
```

## MakeNonStandardStreamContent
### Allows the user to provide non standard media types
Turn string data into user defined stream content
```csharp
MultipartFormDataContent multiPartContent = new MultipartFormDataContent(formDataBoundary);

var content = WebServiceUtils.MakeNonStandardStreamContent(randomData.ToString(), Encoding.ASCII, "multipart/form-data");
var content2 = WebServiceUtils.MakeNonStandardStreamContent(randomData2.ToString(), Encoding.ASCII, "multipart/form-data");

multiPartContent.Add(content, "MyResume", "Resume.abc");
multiPartContent.Add(content2, "MyDefintion", "MyDefintion.def");

var result = this.TestObject.WebServiceDriver.Post<FilesUploaded>("api/upload", "application/json", multiPartContent, true);
```

Turn stream data into user defined stream content
```csharp
MemoryStream memStream = new MemoryStream(data.Length);
memStream.Write(data, 0, data.Length);

MemoryStream memStream2 = new MemoryStream(data2.Length);
memStream2.Write(data2, 0, data2.Length);
MultipartFormDataContent multiPartContent = new MultipartFormDataContent(formDataBoundary);

var content = WebServiceUtils.MakeNonStandardStreamContent(memStream, "multipart/form-data");
var content2 = WebServiceUtils.MakeNonStandardStreamContent(memStream2, "multipart/form-data");

multiPartContent.Add(content, "MyTaxReturns2017", "RandomTestData.abc");
multiPartContent.Add(content2, "MyTripPhoto", "RandomTestData2.def");

var result = this.WebServiceDriver.Post<FilesUploaded>("api/upload", "application/json", multiPartContent, true);
```

## DeserializeXmlDocument
Deserialize the body of a response message.  
*This will only work if the body is XML and can be deserialized as the given object.
```csharp
HttpResponseMessage message = this.WebServiceDriver.GetWithResponse("/api/XML_JSON/GetAllProducts", "application/xml");
ArrayOfProduct result = WebServiceUtils.DeserializeXmlDocument<ArrayOfProduct>(message);
Assert.AreEqual(3, result.Product.Length, "Expected 3 products to be returned");
```


## DeserializeJson
Deserialize the body of a response message.  
*This will only work if the body is JSON and can be deserialized as the given object.
```csharp
HttpResponseMessage message = this.WebServiceDriver.GetWithResponse("/api/XML_JSON/GetAllProducts", MediaType.AppJson);
List<ProductJson> result = WebServiceUtils.DeserializeJson<List<ProductJson>>(message);
Assert.AreEqual(result.Count, 3, "Expected 3 products to be returned");
```

## DeserializeResponse
Deserialize the body of a response message.  
*Allows you to provide multiple media type formats.
```csharp
StringContent content = WebServiceUtils.MakeStringContent<ProductJson>(Product, Encoding.UTF8, "application/json");
HttpResponseMessage response = this.WebServiceDriver.PutWithResponse("/api/XML_JSON/GetAnErrorPLZ", "application/json", content, false);
ProductJson retObject = WebServiceUtils.DeserializeResponse<ProductJson>(response, new List<MediaTypeFormatter> { new CustomXmlMediaTypeFormatter("image/gif", typeof(ProductJson)) });
```