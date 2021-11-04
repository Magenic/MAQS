# <img src="resources/maqslogo.ico" height="32" width="32"> Appium Soft Asserts

## Overview
MAQS provides soft assert functionality in Appium testing. Soft asserts will collect multiple assertions during a test run. At the end of the test MAQS will turn all soft assert failures in to hard assertion failure. 
*_**If the SavePagesourceOnFail configuration is 'Yes' a screenshot will be captured for each soft assert failure.**_  

## Assert
Soft assert method to check if an assertion fails
```csharp
// Simple
this.SoftAssert.Assert(() => Assert.IsTrue(child.Exists));

// Named
this.SoftAssert.Assert(() => Assert.IsTrue(child.Exists), "Expect Child");

// Named with error message
this.SoftAssert.Assert(() => Assert.IsTrue(child.Exists), "Expect Child", "Child does not exist");
```

## FailTestIfAssertFailed
Soft assert method to turn soft assertion failures in to real failures
```csharp
// Failing assertion 1
this.SoftAssert.Assert(() => Assert.IsTrue(false), "Failure 1");

// Failing assertion 2
this.SoftAssert.Assert(() => Assert.IsTrue(false), "Failure 2");

// Raise assertion failures 
this.SoftAssert.FailTestIfAssertFailed();
```
*_**MAQS runs FailTestIfAssertFailed on test teardown so you only need to call FailTestIfAssertFailed directly if you want to run this mid test.**_  