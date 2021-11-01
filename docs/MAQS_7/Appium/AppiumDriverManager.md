# <img src="resources/maqslogo.ico" height="32" width="32"> Appium Driver Manager

## Overview
The Appium Driver Manager has overreach of the Base Driver Manager.

## Get
Get the Appium driver
```csharp
public override object Get()
{
    return this.GetAppiumDriver();
}
```

## DriverDispose
Cleanup the Appium driver
```csharp
protected override void DriverDispose()
{
    // If we never created the driver we don't have any cleanup to do
    if (!this.IsDriverInitialized())
    {
        return;
    }

    try
    {
        AppiumDriver driver = this.GetAppiumDriver();
        driver?.KillDriver();
    }
    catch (Exception e)
    {
        this.Log.LogMessage(MessageType.ERROR, StringProcessor.SafeFormatter("Failed to close mobile driver because: {0}", e.Message));
    }

    this.BaseDriver = null;
}
```