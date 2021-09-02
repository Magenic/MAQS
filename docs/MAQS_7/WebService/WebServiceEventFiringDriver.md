# <img src="resources/maqslogo.ico" height="32" width="32"> Web Service Event Firing Manager

## Overview
The Web Service Event Firing Manager Wraps the basic http client interactions.

[OnEvent](#OnEvent)  
[OnErrorEvent](#OnErrorEvent)  
[PostContent](#PostContent)  
[CustomContent](#CustomContent)  
[PutContent](#PutContent)  
[DeleteContent](#DeleteContent)  
[GetContent](#GetContent)  
[RaiseEvent](#RaiseEvent)  
[RaiseErrorMessage](#RaiseErrorMessage)  
 
## OnEvent
Web service event logging.
```csharp
WebServiceEvent?.Invoke(this, message);
```

## OnErrorEvent
Web service error event logging.
```csharp
 WebServiceErrorEvent?.Invoke(this, message);
```

## PostContent
Do a web service post for the given uri, content and media type
```csharp
protected override async Task<HttpResponseMessage> PostContent(string requestUri, string responseMediaType, HttpContent content, bool expectSuccess = true)
{
    try
    {
        RaiseEvent(content, "Post", requestUri, responseMediaType);
        HttpResponseMessage response = base.PostContent(requestUri, responseMediaType, content, expectSuccess).GetAwaiter().GetResult();
        RaiseEvent("Post", response);
        return await Task.FromResult(response);
    }
    catch (Exception e)
    {
        RaiseErrorMessage(e);
        throw e;
    }
}
```

## CustomContent
Make a HTTP call using the custom verb to the URI with the given verb, media response type and content
```csharp
protected override async Task<HttpResponseMessage> CustomContent(string requestUri, string customVerb, string responseMediaType, HttpContent content, bool expectSuccess = true)
{
    try
    {
        RaiseEvent(content, customVerb, requestUri, responseMediaType);
        HttpResponseMessage response = base.CustomContent(requestUri, customVerb, responseMediaType, content, expectSuccess).GetAwaiter().GetResult();
        RaiseEvent(customVerb, response);
        return await Task.FromResult(response);
    }
    catch (Exception e)
    {
        RaiseErrorMessage(e);
        throw e;
    }
}
```

## PutContent
Do a web service put for the given uri, content and media type
```csharp
 protected override async Task<HttpResponseMessage> PutContent(string requestUri, string responseMediaType, HttpContent content, bool expectSuccess = true)
{
    try
    {
        RaiseEvent(content, "Put", requestUri, responseMediaType);
        HttpResponseMessage response = base.PutContent(requestUri, responseMediaType, content, expectSuccess).GetAwaiter().GetResult();
        RaiseEvent("Put", response);
        return await Task.FromResult(response);
    }
    catch (Exception e)
    {
        RaiseErrorMessage(e);
        throw e;
    }
}
```

## DeleteContent
Do a web service delete for the given uri
```csharp
 protected override async Task<HttpResponseMessage> DeleteContent(string requestUri, string returnMediaType, bool expectSuccess = true)
{
    try
    {
        RaiseEvent("Delete", requestUri, returnMediaType);
        HttpResponseMessage response = base.DeleteContent(requestUri, returnMediaType, expectSuccess).GetAwaiter().GetResult();
        RaiseEvent("Delete", response);
        return await Task.FromResult(response);
    }
    catch (Exception e)
    {
        RaiseErrorMessage(e);
        throw e;
    }
}
```

## GetContent
Do a web service get for the given uri and media type
```csharp
protected override async Task<HttpResponseMessage> GetContent(string requestUri, string mediaType, bool expectSuccess = true)
{
    try
    {
        RaiseEvent("Get", requestUri, mediaType);
        HttpResponseMessage response = base.GetContent(requestUri, mediaType, expectSuccess).GetAwaiter().GetResult();
        RaiseEvent("Get", response);
        return await Task.FromResult(response);
    }
    catch (Exception e)
    {
        RaiseErrorMessage(e);
        throw e;
    }
}
```

## RaiseEvent
Raise an action message without content
```csharp
private void RaiseEvent(string actionType, string requestUri, string mediaType)
{
    OnEvent(StringProcessor.SafeFormatter("Sending {0} request to base: '{1}' endpoint: '{2}' with the media type: '{3}'", actionType, HttpClient.BaseAddress, requestUri, mediaType));
}

 private void RaiseEvent(HttpContent content, string actionType, string requestUri, string mediaType)
{
    try
    {
        StringBuilder message = new StringBuilder();
        message.AppendLine(StringProcessor.SafeFormatter("Sending {0} request with content to base: '{1}' endpoint: '{2}' with the media type: '{3}'", actionType, HttpClient.BaseAddress, requestUri, mediaType));

        mediaType = mediaType.ToUpper();

        if (mediaType.Contains("TEXT") || mediaType.Contains("XML") || mediaType.Contains("JSON"))
        {
            message.AppendLine("Content:");

            if (content != null)
            {
                message.AppendLine(content.ReadAsStringAsync().Result);
            }
        }

        OnEvent(message.ToString());
    }
    catch (Exception e)
    {
        OnErrorEvent(StringProcessor.SafeFormatter("Failed to log event because: {0}", e.ToString()));
    }
}

private void RaiseEvent(string actionType, HttpResponseMessage response)
{
    try
    {
        StringBuilder message = new StringBuilder();
        HttpRequestMessage responseMessage = response.RequestMessage;
        message.AppendLine(StringProcessor.SafeFormatter("Received {0} response from {1}", actionType, responseMessage.RequestUri));
        message.AppendLine(StringProcessor.SafeFormatter("Returned {0}({1})", response.ReasonPhrase, (int)response.StatusCode));

        // Only pull content if we are returned content
        if (response.Content.Headers.ContentType != null)
        {
            string mediaType = response.Content.Headers.ContentType.MediaType.ToUpper();

            // Only add the text if we know it is human readable
            if (mediaType.Contains("TEXT") || mediaType.Contains("XML") || mediaType.Contains("JSON"))
            {
                message.AppendLine("Content:");
                if (response.Content != null)
                {
                    message.AppendLine(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        OnEvent(message.ToString());
    }
    catch (Exception e)
    {
        OnErrorEvent(StringProcessor.SafeFormatter("Failed to log event because: {0}", e.ToString()));
    }
}
```

## RaiseErrorMessage
Raise an exception message
```csharp
private void RaiseErrorMessage(Exception e)
{
    OnErrorEvent(StringProcessor.SafeFormatter("Failed because: {0} {1} {2}", e.Message, Environment.NewLine, e.ToString()));
}
```