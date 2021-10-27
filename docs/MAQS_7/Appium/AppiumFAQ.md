# <img src="resources/maqslogo.ico" height="32" width="32"> Appium FAQ

## What Devices can I use?
- Any device that has an Appium driver.  
If you want to use a browser that MAQS doesn't natively support you can just initialize the driver yourself.
```csharp
 this.AppiumDriver = new TizenDriver(new Uri(DriverConfig.APPIUM_SERVER_URI), option);
```
- Natively supported devices are:  Android | iOS | Windows. 
Find the configuration of devices within the app.config file and define one and only one.

- App.Config: Want to make changes to the Platform, Device, Logging, Remote Integration Hubs, Wait times, Creating Logs, Log File Format and Log folder location. Look to the App.Config within Tests

## Build Solution Fails
- If NuGet packages are missing, Right-click the solution and Enable NuGet Package restore (this requires VPN or LAN connection) Unless an off network storage location or shared folder, was setup to hold the NuGet packages.  Visual Studio allows you to configure additional NuGet locations (see below)

## Driver doesn't want to run
- You should receive a pop-up stating Windows Firewall blocked some features of this app.  Enable Private networks, such as my home or work network and Enable Public networks, such as those in airports and coffee shops (note this is not recommended) Finally, Click Allow Access.

## Configuration Errors from Platform
- right-click the Solution and select Properties then locate the Configuration Properties and set Platform to Any CPU for all Projects such as Tests and WebServiceModel (in this case the solution would be a WebService)

## Errors trying to locate Enterprise when restoring NuGet packages
- right-click the Solution and select Properties > Options > NuGet Package Manager > Package Sources  here you will find Available package sources and Machine-wide package sources.  A mapping to the internal Magenic package storage url (for internal use only) or an external reference such as and internal NuGet repository or file share.
