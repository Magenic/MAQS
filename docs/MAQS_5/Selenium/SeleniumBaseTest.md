# <img src="resources/maqslogo.ico" height="32" width="32"> Base Selenium Test

## Overview
The BaseSeleniumTest class provides access to the SeleniumTestObject and SeleniumDriver.

# Available calls
[GetBrowser](#GetBrowser)  
[BeforeLoggingTeardown](#BeforeLoggingTeardown)  
[CreateNewTestObject](#CreateNewTestObject)  

## GetBrowser
The default get web driver function.
```csharp
protected virtual IWebDriver GetBrowser()
{
    return WebDriverFactory.GetDefaultBrowser();
}
```

## BeforeLoggingTeardown
Take a screen shot if needed and tear down the web driver.
```csharp
protected override void BeforeLoggingTeardown(TestResultType resultType)
{
    // Try to take a screen shot
    try
    {
        if (this.TestObject.GetDriverManager<SeleniumDriverManager>().IsDriverIntialized() && this.Log is FileLogger && resultType != TestResultType.PASS && this.LoggingEnabledSetting != LoggingEnabled.NO)
        {
            SeleniumUtilities.CaptureScreenshot(this.WebDriver, this.TestObject, " Final");

            if (SeleniumConfig.GetSavePagesourceOnFail())
            {
                SeleniumUtilities.SavePageSource(this.WebDriver, this.TestObject, "FinalPageSource");
            }
        }
    }
    catch (Exception e)
    {
        this.TryToLog(MessageType.WARNING, "Failed to get screen shot because: {0}", e.Message);
    }
}
```

## CreateNewTestObject
Create a Selenium test object.
```csharp
protected override void CreateNewTestObject()
{
    Logger newLogger = this.CreateLogger();
    this.TestObject = new SeleniumTestObject(() => this.GetBrowser(), newLogger, this.GetFullyQualifiedTestClassName());
}
```