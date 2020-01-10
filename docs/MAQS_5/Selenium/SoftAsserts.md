# <img src="resources/maqslogo.ico" height="32" width="32"> Selenium Soft Asserts

## Overview
Soft Assert override for selenium tests

[AreEqual](#AreEqual)  
[IsFalse](#IsFalse)  
[IsTrue](#IsTrue)  
[TextToAppend](#TextToAppend)  

## AreEqual
Soft assert method to check if the files are equal
```csharp
public override bool AreEqual(string expectedText, string actualText, string message = "")
{
    return this.AreEqual(expectedText, actualText, string.Empty, message);
}
```

Soft assert method to check if the strings are equal
```csharp
this.AreEqual(expectedText, actualText, string.Empty, message);
```

## IsFalse
Soft assert method to check if the boolean is false
```csharp
 public override bool IsFalse(bool condition, string softAssertName, string failureMessage = "")
{
    bool didPass = base.IsFalse(condition, softAssertName, failureMessage);

    if (!didPass && this.testObject.GetDriverManager<SeleniumDriverManager>().IsDriverIntialized())
    {
        if (SeleniumConfig.GetSoftAssertScreenshot())
        {
            SeleniumUtilities.CaptureScreenshot(this.testObject.WebDriver, this.testObject, this.TextToAppend(softAssertName));
        }

        if (SeleniumConfig.GetSavePagesourceOnFail())
        {
            SeleniumUtilities.SavePageSource(this.testObject.WebDriver, this.testObject, StringProcessor.SafeFormatter(" ({0})", this.NumberOfAsserts));
        }

        return false;
    }
    else if (!didPass)
    {
        return false;
    }

    return true;
}
```

## IsTrue
Soft assert method to check if the boolean is false
```csharp
 public override bool IsTrue(bool condition, string softAssertName, string failureMessage = "")
{
    bool didPass = base.IsTrue(condition, softAssertName, failureMessage);

    if (!didPass && this.testObject.GetDriverManager<SeleniumDriverManager>().IsDriverIntialized())
    {
        if (SeleniumConfig.GetSoftAssertScreenshot())
        {
            SeleniumUtilities.CaptureScreenshot(this.testObject.WebDriver, this.testObject, this.TextToAppend(softAssertName));
        }

        if (SeleniumConfig.GetSavePagesourceOnFail())
        {
            SeleniumUtilities.SavePageSource(this.testObject.WebDriver, this.testObject, StringProcessor.SafeFormatter(" ({0})", this.NumberOfAsserts));
        }

        return false;
    }
    else if (!didPass)
    {
        return false;
    }

    return true;
}
```

## TextToAppend
Method to determine the text to be appended to the screenshot file names
```csharp
string text = this.TextToAppend(softAssertName)
```