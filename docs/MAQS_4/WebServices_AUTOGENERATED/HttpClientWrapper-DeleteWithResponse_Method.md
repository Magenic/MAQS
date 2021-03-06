# HttpClientWrapper.DeleteWithResponse Method 
 

Execute a web service delete

**Namespace:**&nbsp;<a href="#/MAQS_4/WebServices_AUTOGENERATED/Magenic-MaqsFramework-BaseWebServiceTest_Namespace">Magenic.MaqsFramework.BaseWebServiceTest</a><br />**Assembly:**&nbsp;Magenic.MaqsFramework.WebServiceTester (in Magenic.MaqsFramework.WebServiceTester.dll) Version: 4.0.4.0 (4.0.4)

## Syntax

**C#**<br />
``` C#
public HttpResponseMessage DeleteWithResponse(
	string requestUri,
	string expectedMediaType,
	bool expectSuccess = true
)
```


#### Parameters
&nbsp;<dl><dt>requestUri</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />The request uri</dd><dt>expectedMediaType</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />The type of media being requested</dd><dt>expectSuccess (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/a28wyd50" target="_blank">System.Boolean</a><br />Assert a success code was returned</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/hh159046" target="_blank">HttpResponseMessage</a><br />The http response message

## Examples

**C#**<br />
``` C#
[TestMethod]
[TestCategory(TestCategories.WebService)]
public void DeleteStringMakeContentStatusCode()
{
    var result = this.WebServiceWrapper.DeleteWithResponse("/api/String/Delete/1", "text/plain", true);
    Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
}
```


## See Also


#### Reference
<a href="#/MAQS_4/WebServices_AUTOGENERATED/HttpClientWrapper_Class">HttpClientWrapper Class</a><br /><a href="#/MAQS_4/WebServices_AUTOGENERATED/Magenic-MaqsFramework-BaseWebServiceTest_Namespace">Magenic.MaqsFramework.BaseWebServiceTest Namespace</a><br />