# <img src="resources/maqslogo.ico" height="32" width="32"> Soft Assert Exceptions

## Overview
Specified Exceptions to be thrown during soft asserts if an error occurs. 

### Uses
```csharp
public virtual bool AreEqual(string expectedText, string actualText, string softAssertName, string message = "")
{
    void test()
    {
        if (expectedText != actualText)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new SoftAssertException(StringProcessor.SafeFormatter("SoftAssert.AreEqual failed for {0}.  Expected '{1}' but got '{2}'", softAssertName, expectedText, actualText));
            }

            throw new SoftAssertException(StringProcessor.SafeFormatter("SoftAssert.AreEqual failed for {0}.  Expected '{1}' but got '{2}'.  {3}", softAssertName, expectedText, actualText, message));
        }
    }

    return this.InvokeTest(test, expectedText, actualText, message);
}
```