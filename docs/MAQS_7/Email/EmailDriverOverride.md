# <img src="resources/maqslogo.ico" height="32" width="32"> BaseEmailTest
The BaseEmailTest class provides access to the TestObject and EmailDriver.

By default, BaseEmailTest will create an email driver for you based on your [configuration](MAQS_7/Email/EmailConfig.md). Authentication related requirements often require users to override the default web service client.  This is why we provide several different ways for you to provide your own web service driver implementation.

There are three primary ways to override the web service client.

### Override the base email test get connection function
```csharp
/// <summary>
/// Test email functionality
/// </summary>
[TestClass]
public class EmailDriverManagerTests : BaseEmailTest
{
    /// <summary>
    /// Get the Email Imap client
    /// </summary>
    /// <returns>The client</returns>
    protected override ImapClient GetEmailConnection()
    {
        return base.GetEmailConnection();
    }
```

### Override email connection
```csharp
// Override with a function call
this.TestObject.OverrideEmailClient(GetConnectionFunction);

// Override with a lambda expression
this.TestObject.OverrideEmailClient(() => ClientFactory.GetDefaultEmailClient());
```

### Override the email driver directly
```csharp
// Override with a email driver with a new email client
ImapClient client = GetConnectionFunction();
EmailDriver driver = new EmailDriver(() => client);

// Override the email driver new drive with new connection info
this.EmailDriver = new EmailDriver("imaphost.com", "user", "pass", 1234);
```