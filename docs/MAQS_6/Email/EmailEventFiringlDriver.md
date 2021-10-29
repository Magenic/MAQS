# <img src="resources/maqslogo.ico" height="32" width="32"> Email Event Firing Driver

## Overview
Wraps the basic database interactions

[CanAccessEmailAccount](#CanAccessEmailAccount)  
[GetMailBoxNames](#GetMailBoxNames)  
[GetMailbox](#GetMailbox)  
[SelectMailbox](#SelectMailbox)  
[CreateMailbox](#CreateMailbox)  
[GetMessage](#GetMessage)  
[GetAllMessageHeaders](#GetAllMessageHeaders)  
[DeleteMessage](#DeleteMessage)  
[MoveMailMessage](#MoveMailMessage)  
[GetAttachments](#GetAttachments)  
[DownloadAttachments](#DownloadAttachments)  
[SearchMessages](#SearchMessages)  
[GetContentTypes](#GetContentTypes)  
[GetBodyByContentTypes](#GetBodyByContentTypes)  
[OnEvent](#OnEvent)  
[OnErrorEvent](#OnErrorEvent)  
[RaiseErrorMessage](#RaiseErrorMessage)  

## CanAccessEmailAccount
Check if the account is accessible
```csharp
public override bool CanAccessEmailAccount()
{
    try
    {
        bool canAccess = base.CanAccessEmailAccount();
        this.OnEvent(StringProcessor.SafeFormatter("Access account check returned {0}", canAccess));
        return canAccess;
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## GetMailBoxNames
Get the list of mailbox names
```csharp
public override List<string> GetMailBoxNames()
{
    try
    {
        this.OnEvent("Get mailbox names");
        return base.GetMailBoxNames();
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## GetMailbox
Get a mailbox by name
```csharp
public override IMailFolder GetMailbox(string mailbox)
{
    try
    {
        this.OnEvent(StringProcessor.SafeFormatter("Get mailbox named '{0}'", mailbox));
        return base.GetMailbox(mailbox);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## SelectMailbox
Select a mailbox by name
```csharp
public override void SelectMailbox(string mailbox)
{
    try
    {
        this.OnEvent(StringProcessor.SafeFormatter("Select mailbox named '{0}'", mailbox));
        base.SelectMailbox(mailbox);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## CreateMailbox
Create a mailbox
```csharp
public override void CreateMailbox(string newMailBox)
{
    try
    {
        this.OnEvent(StringProcessor.SafeFormatter("Create mailbox named '{0}'", newMailBox));
        base.CreateMailbox(newMailBox);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## GetMessage
Get an email message from the current mailbox
```csharp
public override MimeMessage GetMessage(string uid, bool headerOnly = false, bool markRead = false)
{
    try
    {
        this.OnEvent(
            StringProcessor.SafeFormatter("Get message with uid '{0}',  get header only '{1}' and mark as read '{2}'", uid, headerOnly, markRead));
        return base.GetMessage(uid, headerOnly, markRead);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## GetAllMessageHeaders
Get a list of email messages from the given mailbox
```csharp
public override List<MimeMessage> GetAllMessageHeaders(string mailBox)
{
    try
    {
        this.OnEvent(StringProcessor.SafeFormatter("Get all message headers from '{0}'", mailBox));
        return base.GetAllMessageHeaders(mailBox);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## DeleteMessage
Delete the given email
```csharp
public override void DeleteMessage(MimeMessage message)
{
    try
    {
        this.OnEvent(StringProcessor.SafeFormatter("Delete message '{0}' from '{1}' received '{2}'", message.Subject, message.From, message.Date));
        base.DeleteMessage(message);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}

public override void DeleteMessage(string uid)
{
    try
    {
        this.OnEvent(StringProcessor.SafeFormatter("Delete message with uid '{0}' from mailbox '{1}'", uid, this.CurrentMailBox));
        base.DeleteMessage(uid);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## MoveMailMessage
Move the given email to the destination mailbox
```csharp
public override void MoveMailMessage(MimeMessage message, string destinationMailbox)
{
    try
    {
        this.OnEvent(StringProcessor.SafeFormatter("Move message '{0}' from '{1}' received '{2}' to mailbox '{3}'", message.Subject, message.From.ToString(), message.Date.ToString(), destinationMailbox));
        base.MoveMailMessage(message, destinationMailbox);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```
Move the email with the given unique identifier to the destination mailbox
```csharp
public override void MoveMailMessage(string uid, string destinationMailbox)
{
    try
    {
        this.OnEvent(StringProcessor.SafeFormatter("Move message with uid '{0}' from mailbox '{1}' to mailbox '{2}'", uid, this.CurrentMailBox, destinationMailbox));
        base.MoveMailMessage(uid, destinationMailbox);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## GetAttachments
Get the list of attachments for the email with the given unique identifier
```csharp
public override List<MimeEntity> GetAttachments(string uid)
{
    try
    {
        this.OnEvent(StringProcessor.SafeFormatter("Get list of attachments for message with uid '{0}' in mailbox '{1}'", uid, this.CurrentMailBox));
        return base.GetAttachments(uid);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```
Get the list of attachments for the email with the given message
```csharp
 public override List<MimeEntity> GetAttachments(MimeMessage message)
{
    try
    {
        this.OnEvent(
            StringProcessor.SafeFormatter("Get list of attachments for message '{0}' from '{1}' received '{2}' in mailbox '{3}'", message.Subject, message.From, message.Date, this.CurrentMailBox));
        return base.GetAttachments(message);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## DownloadAttachments
Download all the attachments for the given message
```csharp
public override List<string> DownloadAttachments(MimeMessage message, string downloadFolder)
{
    try
    {
        this.OnEvent(
            StringProcessor.SafeFormatter("Download attachments for message '{0}' from '{1}' revived '{2}' in mailbox '{3}' to '{4}'", message.Subject, message.From, message.Date, this.CurrentMailBox, downloadFolder));
        return base.DownloadAttachments(message, downloadFolder);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## SearchMessages
Get a list of messaged that meet the search criteria
```csharp
public override List<MimeMessage> SearchMessages(SearchQuery condition, bool headersOnly = true, bool markRead = false)
{
    try
    {
        this.OnEvent(
            StringProcessor.SafeFormatter("Search for messages in mailbox '{0}' with search condition '{1}', header only '{2}' and mark as read '{3}'", this.CurrentMailBox, condition, headersOnly, markRead));
        return base.SearchMessages(condition, headersOnly, markRead);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## GetContentTypes
Get the list of content types for the given message
```csharp
public override List<string> GetContentTypes(MimeMessage message)
{
    try
    {
        this.OnEvent(StringProcessor.SafeFormatter("Get list of content types for message '{0}' from '{1}' received '{2}'", message.Subject, message.From, message.Date));
        return base.GetContentTypes(message);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## GetBodyByContentTypes
Get the email body for the given message that matches the content type
```csharp
public override string GetBodyByContentTypes(MimeMessage message, string contentType)
{
    try
    {
        this.OnEvent(StringProcessor.SafeFormatter("Get '{0}' content for message '{1}' from '{2}' received '{3}'", contentType, message.Subject, message.From, message.Date));
        string body = base.GetBodyByContentTypes(message, contentType);
        this.OnEvent(StringProcessor.SafeFormatter("Got message body:\r\n{0}", body));
        return body;
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## OnEvent
Email event logging.
```csharp
this.OnEvent("Get mailbox names");
```

## OnErrorEvent
Email error event logging
```csharp
this.OnErrorEvent(StringProcessor.SafeFormatter("Failed because: {0}{1}{2}", e.Message, Environment.NewLine, e.ToString()));
```

## RaiseErrorMessage
Raise an exception message
```csharp
this.RaiseErrorMessage(ex);
```