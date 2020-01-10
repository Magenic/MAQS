# <img src="resources/maqslogo.ico" height="32" width="32"> Lazy Mobile Element

## Overview
The Lazy Mobile Element class is used for dynamically finding and interacting with elements

## FindElement
Finds the first IWebElement using the given method.
```csharp
IWebElement webElement = this.FindElement(by, "Child element");
```

### FindElements
Finds all IWebElement within the current context using the given mechanism.
```csharp
ReadOnlyCollection<IWebElement> elements = this.FindElements(by, "Child elements");
```