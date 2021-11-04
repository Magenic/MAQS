# <img src="resources/maqslogo.ico" height="32" width="32"> Selenium Driver Manager

## Overview
Selenium driver store

[GetWebDriver](#GetWebDriver)  
[Get](#Get)  
[LogVerbose](#LogVerbose)  
[DriverDispose](#DriverDispose)  
[LoggingStartup](#LoggingStartup)  
[MapEvents](#MapEvents)  
[WebDriver_NavigatingForward](#WebDriver_NavigatingForward)  
[WebDriver_NavigatingBack](#WebDriver_NavigatingBack)  
[WebDriver_NavigatedForward](#WebDriver_NavigatedForward)  
[WebDriver_NavigatedBack](#WebDriver_NavigatedBack)  
[WebDriver_Navigating](#WebDriver_Navigating)  
[WebDriver_ScriptExecuting](#WebDriver_ScriptExecuting)  
[WebDriver_FindingElement](#WebDriver_FindingElement)  
[WebDriver_ElementValueChanging](#WebDriver_ElementValueChanging)  
[WebDriver_ElementClicking](#WebDriver_ElementClicking)  
[WebDriver_ExceptionThrown](#WebDriver_ExceptionThrown)    
[WebDriver_Navigated](#WebDriver_Navigated)    
[WebDriver_ScriptExecuted](#WebDriver_ScriptExecuted)    
[WebDriver_FindElementCompleted](#WebDriver_FindElementCompleted)    
[WebDriver_ElementValueChanged](#WebDriver_ElementValueChanged)    
[WebDriver_ElementClicked](#WebDriver_ElementClicked)    

## GetWebDriver
Get the web driver
```csharp
IWebDriver driver = this.GetWebDriver();
``

## Get
Get the web driver
```csharp
public override object Get()
{
    return this.GetWebDriver();
}
```

## LogVerbose
Log a verbose message and include the automation specific call stack data
```csharp
this.LogVerbose("Navigating to: {0}", e.Url);
```

## DriverDispose
Have the driver cleanup after itself
```csharp
protected override void DriverDispose()
{
    Log.LogMessage(MessageType.VERBOSE, "Start dispose driver");

    // If we never created the driver we don't have any cleanup to do
    if (!this.IsDriverIntialized())
    {
        return;
    }

    try
    {
        IWebDriver driver = this.GetWebDriver();
        driver?.KillDriver();
    }
    catch (Exception e)
    {
        Log.LogMessage(MessageType.ERROR, StringProcessor.SafeFormatter("Failed to close web driver because: {0}", e.Message));
    }

    this.BaseDriver = null;
    Log.LogMessage(MessageType.VERBOSE, "End dispose driver");
}
```

## LoggingStartup
Log that the web driver setup
```csharp
this.LoggingStartup(tempDriver);
```

## MapEvents
Map selenium events to log events, this is done in the GetWebDriverMethod
```csharp
 this.MapEvents(tempDriver as EventFiringWebDriver);
```

## WebDriver_NavigatingForward
Event for webdriver that is navigating forward
```csharp
 private void WebDriver_NavigatingForward(object sender, WebDriverNavigationEventArgs e)
{
    this.LogVerbose("Navigating to: {0}", e.Url);
}
```

## WebDriver_NavigatingBack
Event for webdriver that is navigating back
```csharp
private void WebDriver_NavigatingBack(object sender, WebDriverNavigationEventArgs e)
{
    this.LogVerbose("Navigating back to: {0}", e.Url);
}
```
## WebDriver_NavigatedForward
Event for webdriver that has navigated forward
```csharp
private void WebDriver_NavigatedForward(object sender, WebDriverNavigationEventArgs e)
{
    Log.LogMessage(MessageType.INFORMATION, "Navigated Forward: {0}", e.Url);
}
```

## WebDriver_NavigatedBack
Event for webdriver that is navigated back
private void WebDriver_NavigatedBack(object sender, WebDriverNavigationEventArgs e)
{
    Log.LogMessage(MessageType.INFORMATION, "Navigated back: {0}", e.Url);
}

## WebDriver_Navigating
Event for webdriver that is navigating
```csharp
private void WebDriver_Navigating(object sender, WebDriverNavigationEventArgs e)
{
    this.LogVerbose("Navigating to: {0}", e.Url);
}
```

## WebDriver_ScriptExecuting
Event for webdriver that is script executing
```csharp
private void WebDriver_ScriptExecuting(object sender, WebDriverScriptEventArgs e)
{
    this.LogVerbose("Script executing: {0}", e.Script);
}
```

## WebDriver_FindingElement
Event for webdriver that is finding an element
```csharp
private void WebDriver_FindingElement(object sender, FindElementEventArgs e)
{
    this.LogVerbose("Finding element: {0}", e.FindMethod);
}
```

## WebDriver_ElementValueChanging
Event for webdriver that is changing an element value
```csharp
private void WebDriver_ElementValueChanging(object sender, WebElementEventArgs e)
{
    this.LogVerbose("Value of element changing: {0}", e.Element);
}
```

## WebDriver_ElementClicking
Event for webdriver that is clicking an element
```csharp
private void WebDriver_ElementClicking(object sender, WebElementEventArgs e)
{
    Log.LogMessage(MessageType.INFORMATION, "Element clicking: {0} Text:{1} Location: X:{2} Y:{3}", e.Element, e.Element.Text, e.Element.Location.X, e.Element.Location.Y);
}
```

## WebDriver_ExceptionThrown
Event for webdriver when an exception is thrown
```csharp
private void WebDriver_ExceptionThrown(object sender, WebDriverExceptionEventArgs e)
{
    // First chance handler catches these when it is a real error - These are typically retry loops
    Log.LogMessage(MessageType.VERBOSE, "Exception thrown: {0}", e.ThrownException);
}
```

## WebDriver_Navigated
Event for webdriver that has navigated
```csharp
private void WebDriver_Navigated(object sender, WebDriverNavigationEventArgs e)
{
    Log.LogMessage(MessageType.INFORMATION, "Navigated to: {0}", e.Url);
}
```

## WebDriver_ScriptExecuted
Event for webdriver has executed a script
```csharp
 private void WebDriver_ScriptExecuted(object sender, WebDriverScriptEventArgs e)
{
    Log.LogMessage(MessageType.INFORMATION, "Script executed: {0}", e.Script);
}
```

## WebDriver_FindElementCompleted
Event for webdriver that is finished finding an element
```csharp
private void WebDriver_FindElementCompleted(object sender, FindElementEventArgs e)
{
    Log.LogMessage(MessageType.INFORMATION, "Found element: {0}", e.FindMethod);
}
```

## WebDriver_ElementValueChanged
Event for webdriver that has changed an element value
```csharp
private void WebDriver_ElementValueChanged(object sender, WebElementEventArgs e)
{
    string element = e.Element.GetAttribute("value");
    Log.LogMessage(MessageType.INFORMATION, "Element value changed: {0}", element);
}
```

## WebDriver_ElementClicked
Event for webdriver that has clicked an element
```csharp
private void WebDriver_ElementClicked(object sender, WebElementEventArgs e)
{
    try
    {
        this.LogVerbose("Element clicked: {0} Text:{1} Location: X:{2} Y:{3}", e.Element, e.Element.Text, e.Element.Location.X, e.Element.Location.Y);
    }
    catch
    {
        this.LogVerbose("Element clicked");
    }
}
```