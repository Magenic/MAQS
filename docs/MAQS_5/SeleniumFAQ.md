# <img src="resources/maqslogo.ico" height="32" width="32"> FAQ

## What Browsers can I use?
- Any browser that has a IWebDriver.  
If you want to use a browser that MAQS doesn't natively support you can just initialize the driver yourself.
```csharp
this.WebDriver = new OperaDriver("path_to_opera_driver.exe");
```
- Natively supported browsers are:  Firefox | Chrome | Edge | PhantomJS | Internet Explorer. 
Find the configuration of browsers within the app.config file and define one and only one.

- App.Config: Want to make changes to the Browser, Logging, Remote Integration Hubs, Wait times, Creating Logs, Log File Format and Log folder location ?? Look to the App.Config within Tests

## Why doesn't PhantomJS work
- Selenium 3.14.0 removed support for PhantomJS. Headless Chrome typically is a good substitute. 

### How do I fix common user errors?

### My code can't find the config class  
- Make sure you've imported the Full namespace such as Magenic.Maqs.Utilities.Helper.Config
### Test can't find the page element for a test, errors indicate something is wrong with Selenium  
- Kill the existing Chrome processes spawned and orphaned from the test kick off then update Browser and start fresh
### Test fails on driver, such as Chrome  
- Update the Chrome browser  (In Chrome click the HotDog icon (formerly known as Wrench) > help > about Google Chrome > Update , the key is that the Chrome browser version and the ChromeDriver versions must be compatible with each other.

## Build Solution Fails
- If NuGet packages are missing, Right-click the solution and Enable NuGet Package restore (this requires VPN or LAN connection) Unless an off network storage location or shared folder, was setup to hold the NuGet packages.  Visual Studio allows you to configure additional NuGet locations (see below)

## Driver doesn't want to run
- You should receive a pop-up stating Windows Firewall blocked some features of this app.  Enable Private networks, such as my home or work network and Enable Public networks, such as those in airports and coffee shops (note this is not recommended) Finally, Click Allow Access.

## Configuration Errors from Platform
- right-click the Solution and select Properties then locate the Configuration Properties and set Platform to Any CPU for all Projects such as Tests and WebServiceModel (in this case the solution would be a WebService)

## Errors trying to locate Enterprise when restoring NuGet packages
- right-click the Solution and select Properties > Options > NuGet Package Manager > Package Sources  here you will find Available package sources and Machine-wide package sources.  A mapping to the internal Magenic package storage url (for internal use only) or an external reference such as and internal NuGet repository or file share.

## What Integrations exist?

### Sauce Labs
- Support for and configuration of integration with Sauce Labs can be done by providing a username and accesskey in the RemoteSeleniumCapsMaqs section of your configuration file.
```xml
  <SeleniumMaqs>
    <!--Remote browser settings-->
    <add key="Browser" value="REMOTE"/>
    <add key="RemoteBrowser" value="Generic" />
    <add key="HubUrl" value="http://ondemand.saucelabs.com:80/wd/hub" />
  </SeleniumMaqs>
  <RemoteSeleniumCapsMaqs>
    <add key="username" value="Sauce_Labs_Username" />
    <add key="accessKey" value="Sauce_Labs_Accesskey" />
    <add key="browserName" value="Chrome" />
    <add key="platform" value="Windows 10" />
  </RemoteSeleniumCapsMaqs>
  ```

### BrowserStack
- Support for and configuration of integration with BrowserStack can be done by providing a browserstack.use and browserstack.key in the RemoteSeleniumCapsMaqs section of your configuration file.
```xml
  <SeleniumMaqs>
    <!--Remote browser settings-->
    <add key="Browser" value="REMOTE"/>
    <add key="RemoteBrowser" value="Generic" />
    <add key="HubUrl" value="http://hub-cloud.browserstack.com/wd/hub/" />
  </SeleniumMaqs>
  <RemoteSeleniumCapsMaqs>
    <add key="browserstack.user" value="BrowserStack_Username" />
    <add key="browserstack.key" value="BrowserStack_Accesskey" />
    <add key="browser" value="Chrome" />
    <add key="os" value="Windows" />
    <add key="os_version" value="10" />
  </RemoteSeleniumCapsMaqs>
  ```