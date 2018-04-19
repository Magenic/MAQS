# <img src="resources/maqslogo.ico" height="32" width="32"> Wait Methods

## Overview of WaitFor and WaitUntil
There are two different kinds of wait methods provided, ones prefixed with "WaitFor" and ones prefixed with "WaitUntil." The "WaitFor" methods wait until the condition described in the method name is satisfied, and if it times out without the condition being satisfied, then it throws an exception. Whereas if a "WaitUntil" method times out without its condition being satisfied, it will simply just return false, and if its condition is satisfied, it will return true.

## WaitFor Methods

### For Element to be Absent
Waits for the element to not appear on the page. The element can be gone or just not displayed.
To see how Selenium determines if an element is displayed or not, refer to the [W3C WebDriver specification][1].
If the element is not absent after the timeout, an exception is thrown.

#### Written as
```csharp
ForAbsentElement(By bySelector);
```

#### Example
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

// Wait for absent button element.  If wait times out, throw an exception
this.webDriver.Wait().ForAbsentElement(button );
```
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

//element waits for an absent element
element.Wait()ForAbsentElement(button);
```

### For an Element's Attribute Text to Contain Text
Waits for the element's attribute to contain the correct, case-insensitive text value.
If the element's attribute value does not contain the text value after the timeout, an exception is thrown.

#### Written as
```csharp
this.webDriver.Wait().ForAttributeTextContains(By selector, string attributeText, string attributeName)
```
#### Example
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

// Waits for attribute text to contain "example.com", if the attribute text never does, throw an exception
this.webDriver.Wait().ForAttributeTextContains(button, "example.com", "href");
```
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

// Waits for attribute text to contain "example.com", if the attribute text never does, throw an exception
element.Wait().ForAttributeTextContains(button, "example.com", "href");
```
### For an Element's Attribute Text to Equal Text
Waits for the element's attribute to equal the correct, case-sensitive text value.
If the element's attribute value does not equal the text value after the timeout, an exception is thrown.
#### Written as
```csharp
ForAttributeTextEquals(By bySelector, string textValue, string attribute)
```
#### Example
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

// Waits for attribute text to equal "example.com", if the attribute text never does, throw an exception
this.webDriver.Wait().ForAttributeTextEquals(button, "example.com", "href");
```
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

// Waits for attribute text to equal "example.com", if the attribute text never does, throw an exception
element.Wait().ForAttributeTextEquals(button, "example.com", "href");
```
### For an Element to Become Clickable
Waits for the element to become displayed and enabled.
If the element is not clickable after the timeout, an exception is thrown.
#### Written as
```csharp
ForClickableElement(By bySelector)
```

#### Example
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

// Waits for an element to become clickable, if it never does, or it's not found, throw an exception
this.webDriver.Wait().ForClickableElement(button);
```
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

// Waits for an element to become clickable, if it never does, or it's not found, throw an exception
element.Wait().ForClickableElement(button);
```
### For an Element to Become Clickable and Scroll Until the Element is In View
Waits for the element to become displayed and enabled and scrolls the element into view, usually to the top of the viewport.
This method uses JavaScript's [scrollIntoView][2] method to execute the scrolling.
If the element is not clickable after the timeout, an exception is thrown.

#### Written as
```csharp
ForClickableElementAndScrollIntoView(By bySelector)
```
#### Example
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

this.webDriver.Wait().ForClickableElementAndScrollIntoView(button);
```
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

element.Wait().ForClickableElementAndScrollIntoView(button);
```
### For an Element to Become Clickable and Scroll Based on Elements Location
Waits for the element to become displayed and enabled and scrolls the viewport to an offset relative to the element's location.
This method uses JavaScript's [scrollIntoView][2] and [scroll][3] methods to execute the scrolling.
If the element is not clickable after the timeout, an exception is thrown.

#### Written as
```csharp
ForClickableElementAndScrollIntoView(By bySelector, int x, int y);
```

### Example
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

this.webDriver.Wait().ForClickableElementAndScrollIntoView(button, 20, 40);
```
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

element.Wait().ForClickableElementAndScrollIntoView(button, 20, 40);
```
### For an Element Text to Contain Text
Waits for the element to contain the expected, case-insensitive text.
If the element does not contain the expected text after the timeout, an exception is thrown.

#### Written as
```csharp
ForContainsText(By bySelector, string elementText);
```
#### Example
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

this.webDriver.Wait().ForContainsText(button, "ello, world");
```
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

element.Wait().ForContainsText(button, "ello, world");
```
### For an Element Text to Equal Text
Waits for the element to equal the case-sensitive text.
If the element does not equal the exact text after the timeout, an exception is thrown.
#### Written as
```csharp
ForExactText(By bySelector, string elementText);
```
#### Example:
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

this.webDriver.Wait().ForExactText(button, "Hello, world!");
```
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

element.Wait().ForExactText(button, "Hello, world!");
```
### For Page to Finish Loading
Waits for the web page to load.
If the web page is not loaded after the timeout, an exception is thrown.
#### Written as
```csharp
ForPageLoad();
```
#### Example:
```csharp
this.webDriver.Wait().ForPageLoad();
```
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

element.Wait().ForPageLoad();
```
### For Element to be Visible
Waits for the element to be displayed.
To see how Selenium determines if an element is displayed or not, refer to the [W3C WebDriver specification][1].
If the element is not displayed after the timeout, an exception is thrown.
#### Written as
```csharp
ForVisibleElement(By bySelector);
```

#### Example:
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

this.webDriver.Wait().ForVisibleElement(button);
```
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

element.Wait().ForVisibleElement(button);
```

## WaitUntil Methods

### Until an Element is Absent
Waits until the element does not appear on the page. The element can be gone or just not displayed.
To see how Selenium determines if an element is displayed or not, refer to the [W3C WebDriver specification][1].
If the element becomes absent before the timeout, then it returns true, else it returns false.
#### Written as
```csharp
UntilAbsentElement(By bySelector);
```

#### Example:
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

this.webDriver.Wait().UntilAbsentElement(button);
```
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

element.Wait().UntilAbsentElement(button);
```
### Until an Element Text to Contain Text
Waits until the element's attribute contains the correct, case-insensitive text value.
If the element's attribute text contains the correct text value before the timeout, then it returns true, else it returns false.
#### Written as
```csharp
UntilAttributeTextContains(By bySelector, string attributeText, string attributeName);
```
#### Example:
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

this.webDriver.Wait().UntilAttributeTextContains(button, "example.com", "href");
```
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

element.Wait().UntilAttributeTextContains(button, "example.com", "href");
```

### Until an Element Text to Equal Text
Waits until the element's attribute equals the correct, case-sensitive text value.
If the element's attribute text equals the correct text value before the timeout, then it returns true, else it returns false.
#### Written as
```csharp
UntilAttributeTextEquals(By bySelector, string attributeText, string attributeName);
```
#### Example:
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

this.webDriver.Wait().UntilAttributeTextEquals(button, "http://example.com/index", "href");
```
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

element.Wait().UntilAttributeTextEquals(button, "http://example.com/index", "href");
```
### Until Element Becomes Clickable
Waits until the element to become displayed and enabled.
If the element is clickable before the timeout, then it returns true, else it returns false.
#### Written as
```csharp
UntilClickableElement(By bySelector);
```
#### Example:
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

this.webDriver.Wait().UntilClickableElement(button);
```
```csharp
// By selector for a button
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

element.Wait().UntilClickableElement(button);
```
### Until an Element to Become Clickable and Scroll Until the Element is In View
Waits until the element becomes displayed and enabled and scrolls the element into view.
This method uses JavaScript's [scrollIntoView][2] method to execute the scrolling.
If the element is clickable before the timeout, then it will scroll the element into view, usually to the top of the viewport, then return true, else it returns false.
### Written as
```csharp
UntilClickableElementAndScrollIntoView(By bySelector);
```
#### Example:
```csharp
private static By button = By.CssSelector("button");

this.webDriver.Wait().UntilClickableElementAndScrollIntoView(button);
```
```csharp
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

element.Wait().UntilClickableElementAndScrollIntoView(button);
```

### For an Element to Become Clickable and Scroll Based on Elements Location
Waits until the element becomes displayed and enabled and scrolls to an offset of that element.
This method uses JavaScript's [scrollIntoView][2] and [scroll][3] methods to execute the scrolling.
If the element is clickable before the timeout, then it will scroll the viewport to an offset relative to the element's location, then return true, else it returns false.
#### Written as
```csharp
UntilClickableElementAndScrollIntoView(By bySelector, int xOffset, int yOffset);
```
#### Example:
```csharp
private static By button = By.CssSelector("button");

this.webDriver.Wait().UntilClickableElementAndScrollIntoView(button, 20, 40);
```
```csharp
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

element.Wait().UntilClickableElementAndScrollIntoView(button, 20, 40);
```
### Until Element Contains Text
Waits until the element contains the expected, case-insensitive text.
If the element contains the expected text before the timeout, then it returns true, else it returns false.
#### Written as
```csharp
UntilContainsText(By bySelector, string elementText);
```
#### Example:
```csharp
private static By button = By.CssSelector("button");

this.webDriver.Wait().UntilContainsText(button, "ello, world");
```
```csharp
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

element.Wait().UntilContainsText(button, "ello, world");
```
### Until an Element's Text to Equal Text
Waits until the element has specific, case-sensitive text.
If the element's text is exactly equal to the expected text before the timeout, then it returns true, else it returns false.
#### Written as
```csharp
UntilContainsText(By bySelector, string elementText);
```
#### Example:
```csharp
private static By button = By.CssSelector("button");

this.webDriver.Wait().UntilExactText(button, "Hello, world!");
```
```csharp
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

element.Wait().UntilExactText(button, "Hello, world!");
```
### Until Page Finishes Loading
Waits until the web page has loaded.
If the web page loads before the timeout, then it returns true, else it returns false.
#### Written as
```csharp
UntilPageLoad();
```
#### Example:
```csharp
this.webDriver.Wait().UntilPageLoad();
```

### Until Element Becomes Visible
Waits until the element is displayed.
To see how Selenium determines if an element is displayed or not, refer to the [W3C WebDriver specification][1].
If the element is displayed before the timeout, then it returns true, else it returns false.
#### Written as
```csharp
UntilVisibleElement(By bySelector);
```
#### Example:
```csharp
private static By button = By.CssSelector("button");

this.webDriver.Wait().UntilVisibleElement(button);
```
```csharp
private static By button = By.CssSelector("button");

// An IWebElement
IWebElement element = this.webDriver.FindElement(button);

element.Wait().UntilVisibleElement(button);
```

[1]: https://w3c.github.io/webdriver/webdriver-spec.html#element-displayedness
[2]: https://developer.mozilla.org/en-US/docs/Web/API/Element/scrollIntoView
[3]: https://developer.mozilla.org/en-US/docs/Web/API/Window/scroll

