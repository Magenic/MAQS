# <img src="resources/maqslogo.ico" height="32" width="32"> Mobile Driver Manager

## Overview
The Mobile Driver Manager has overreach of the Base Driver Manager.

## Get
Get the Appium driver
```csharp
public override object Get()
{
    return this.GetMobileDriver();
}
```

## DriverDispose
Cleanup the Appium driver
```csharp
protected override void DriverDispose()
{
    // If we never created the driver we don't have any cleanup to do
    if (!this.IsDriverIntialized())
    {
        return;
    }

    try
    {
        AppiumDriver<IWebElement> driver = this.GetMobileDriver();
        driver?.KillDriver();
    }
    catch (Exception e)
    {
        this.Log.LogMessage(MessageType.ERROR, StringProcessor.SafeFormatter("Failed to close mobile driver because: {0}", e.Message));
    }

    this.BaseDriver = null;
}
```