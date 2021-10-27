# <img src="resources/maqslogo.ico" height="32" width="32"> Email Test Object

## Overview
Email test context data

[EmailManager](#EmailManager)  
[EmailDriver](#EmailDriver)  
[OverrideDatabaseConnection](#OverrideDatabaseConnection)  
[OverrideDatabaseDriver](#OverrideDatabaseDriver)  

## EmailManager
Gets the email driver manager
```csharp
EmailManager manager = this.EmailManager.GetEmailDriver();
```

## EmailDriver
Gets the email driver
```csharp
public EmailDriver EmailDriver
{
    get
    {
        if (this.driver != null)
        {
            return this.driver;
        }

        return this.EmailManager.GetEmailDriver();
    }
}
```

## OverrideDatabaseConnection
Override the email driver
```csharp
public void OverrideDatabaseConnection(Func<ImapClient> emailConnection)
{
    if (this.driver != null)
    {
        this.driver.Dispose();
        this.driver = null;
    }

    this.OverrideDriverManager(typeof(EmailDriverManager).FullName, new EmailDriverManager(emailConnection, this));
}
```

## OverrideDatabaseDriver
Override the email driver
```csharp
public void OverrideDatabaseDriver(EmailDriver emailDriver)
{
    if (this.driver != null)
    {
        this.driver.Dispose();
    }

    this.driver = emailDriver;
}
```