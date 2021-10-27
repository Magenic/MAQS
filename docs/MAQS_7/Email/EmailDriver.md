# <img src="resources/maqslogo.ico" height="32" width="32"> Email Driver

## Overview
The EmailDriver object is included in the EmailTestObject. The driver sets and opens a connection to the email on instansitaiton.

### EventFiringEmailDriver
Similar to the EmailDriver, except raises an event before an Email interaction. 

### BaseEmailTest and EmailDriver
Using the EmailDriver within a BaseEmailTest is easy, simply call the driver: 
```csharp
 EmailDriver temp = new EmailDriver(() => GetClient());
```

### EmailDriver without BaseEmailTest
To use the EmailDriver without the BaseEmailTest, simply create the driver object.

# Available calls
[EmailConnection](#EmailConnection)  
[CurrentMailBox ](#CurrentMailBox )  
[CurrentFolder](#CurrentFolder)  
[Dispose](#Dispose)  
[CanAccessEmailAccount](#CanAccessEmailAccount)  
[GetMailBoxNames](#GetMailBoxNames)  
[GetMailBoxNamesInNamespace](#GetMailBoxNamesInNamespace)  
[GetMailbox](#GetMailbox)  
[SelectMailbox](#SelectMailbox)  
[CreateMailbox](#CreateMailbox)  
[GetMessage](#GetMessage)  
[GetAllMessageHeaders](#GetAllMessageHeaders)  
[DeleteMessage](#DeleteMessage)  
[MoveMailMessage](#MoveMailMessage)  
[GetAttachments](#GetAttachments)  
[DownloadAttachments](#DownloadAttachments)  
[SearchMessagesSince](#SearchMessagesSince)  
[SearchMessages](#SearchMessages)  
[GetContentTypes](#GetContentTypes)  
[GetBodyByContentTypes](#GetBodyByContentTypes)  
[GetEmailFlags](#GetEmailFlags)  
[GetUniqueIDString](#GetUniqueIDString)  
[Dispose](#Dispose)  
[GetSearchResults](#GetSearchResults)  
[DefaultToInboxIfExists](#DefaultToInboxIfExists)  
[BaseNamespace](#BaseNamespace)  
[GetCurrentFolder](#GetCurrentFolder)  

## EmailConnection
Gets the Imap email connection
```csharp
ImapClient client = this.EmailConnection;
```

## CurrentMailBox
Gets the current mailbox
```csharp
string mailBox = this.EmailDriver.CurrentMailBox;
```

## CurrentFolder
Gets the current folder
```csharp
IMailFolder folder = this.CurrentFolder;
```

## Dispose
Dispose of the database connection
```csharp
this.driver.Dispose();
```

## CanAccessEmailAccount
Check if the account is accessible
```csharp
bool canAccess = this.EmailDriver.CanAccessEmailAccount();
```

## GetMailBoxNames
Get the list of mailbox names
```csharp
List<string> mailBoxes = this.EmailDriver.GetMailBoxNames();
```

## GetMailbox
Get a mailbox by name
```csharp
IMailFolder box = this.EmailDriver.GetMailbox(mailBox);
```

## SelectMailbox
Select a mailbox by name
```csharp
this.SelectMailbox(mailBox);
```

## CreateMailbox
Create a mailbox
```csharp
this.EmailDriver.CreateMailbox(newMailBox);
```

## GetMessage
Get an email message from the current mailbox
```csharp
MimeMessage singleMessage = this.EmailDriver.GetMessage("3");
```

## GetAllMessageHeaders
Get a list of email messages from the current mailbox
```csharp
List<MimeMessage> messageHeaders = this.EmailDriver.GetAllMessageHeaders();
```

## DeleteMessage
Delete the given email
```csharp
this.EmailDriver.DeleteMessage(message);
```

## MoveMailMessage
Move the given email to the destination mailbox
```csharp
this.EmailDriver.MoveMailMessage(message, "Test");
```

## GetAttachments
Get the list of attachments for the email with the given unique identifier
```csharp
List<MimeEntity> attchments = this.EmailDriver.GetAttachments(this.EmailDriver.GetUniqueIDString(singleMessage));
```

## DownloadAttachments
Download all the attachments for the given message
```csharp
 List<string> attchments = this.EmailDriver.DownloadAttachments(singleMessage, downloadLocation);
```

## SearchMessagesSince
Get a list of messages from a specific Mailbox sent since a specific day - Only the date is used in the search, hour and below are ignored
```csharp
List<MimeMessage> messages = this.EmailDriver.SearchMessagesSince("Test/SubTest", new DateTime(2016, 3, 11), false);
```

## SearchMessages
Get a list of messages that meet the search criteria
```csharp
List<MimeMessage> messages = this.EmailDriver.SearchMessages("Test/SubTest", condition, false);
```

## GetContentTypes
Get the list of content types for the given message
```csharp
List<string> types = this.EmailDriver.GetContentTypes(messages[0]);
```

## GetBodyByContentTypes
Get the email body for the given message that matches the content type
```csharp
string content = this.EmailDriver.GetBodyByContentTypes(messages[0], "text/html");
```

## GetEmailFlags
Get a list of flags for the email with the given uniqueID
```csharp
List<IMessageSummary> flags = this.EmailDriver.GetEmailFlags(this.EmailDriver.GetUniqueIDString(message));
```

## GetUniqueIDString
Get the UniqueID for the inputted MimeMessage
```csharp
message = this.EmailDriver.GetMessage(this.EmailDriver.GetUniqueIDString(message), true, true);
```

## Dispose
Cleanup the driver
```csharp
this.Dispose(true);
```

## GetUniqueIDString
Get the UniqueID for the inputted MimeMessage
```csharp
message = this.EmailDriver.GetMessage(this.EmailDriver.GetUniqueIDString(message), true, true);
```

## GetSearchResults
Get the list of emails for a search
```csharp
List<MimeMessage> message = this.GetSearchResults
```

## DefaultToInboxIfExists
Set the default mailbox - Use inbox by default
```csharp
this.DefaultToInboxIfExists();
```

## BaseNamespace
Get the default folder namespace
```csharp
FolderNamespace namespace = this.EmailConnection.PersonalNamespaces[0];
```

## GetCurrentFolder
Gets and update the Folder representing the CurrentMailBox
```csharp
IMailFolder folder = this.GetCurrentFolder();
```