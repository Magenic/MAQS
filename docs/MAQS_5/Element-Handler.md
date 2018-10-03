# <img src="resources/maqslogo.ico" height="32" width="32"> ElementHandler Class
This class provides additional helper methods on top of Selenium's base interaction methods for common operations done when interacting with HTML elements.

## Check a Checkbox
Checks or unchecks an element. It will check the element if the bool argument is true. It will uncheck the element if the bool argument is false.  If the box is already in state the action wants it to be in upon completing that action, then the action will not do any checking or unchecking.
### Written as
```csharp
CheckCheckBox(By bySelector, bool checkBoxState);
```

### Example
```csharp
// By selector for a checkbox
private static By checkBox = By.Id("CheckBox");

// Checkbox is initially unchecked, waits for the element to become clickable
this.webDriver.WaitUntilClickableElement(checkBox);

// If the checkbox is unchecked, then the driver will check the checkbox
this.webDriver.CheckCheckBox(By.CssSelector(checkBox), true);

// If the checkbox is unchecked, then the driver will uncheckcheck the checkbox
this.webDriver.CheckCheckBox(By.CssSelector(checkBox), false);
```
In applications a checkbox will either be checked or unchecked.  Stating "click this checkbox" does not take into consideration the state of that checkbox.  

## Click Button and Wait For Button Disappear
Clicks an element. If the bool argument is true, it will wait until the button disappears after clicking it, else it will immediately return.  If it waits for the button to disappear, it will throw an exception if it does not.  If it does not wait for the button to disappear, it will continue.
### Written as
```csharp
ClickButton(By bySelector, bool waitForButton);
```
### Example
```csharp
// By selector for a checkbox
private static By button = By.Id("LargeButton");

this.webDriver.ClickButton(By.TagName("button"), true);
```

## Click Element With JavaScript
Clicks an element using JavaScript's [click][1] method where using the normal Selenium Click method may not work correctly, such as hidden or hover triggered elements.
### Written as
```csharp
ClickElementByJavaScript(By bySelector);
```
### Example
```csharp
private static By button = By.Id("LargeButton");

this.webDriver.ClickElementByJavaScript(button);
```

## Create a Comma Delimited String
Creates a collection of elements based on a selector, loops through the collection, gathering text, adding it to a list, removes any white space, sorts the list alphabetically, and then returns that list as a string with the values separated by commas.  By default it will retrieve the value from the value attribute.

### Written as
```csharp
string CreateCommaDelimitedString(By by, bool sort = false));
```
### Examples
```csharp
// A by selector for a list of computer parts.
private static By computerParts = By.CssSelector("ul>#options);

// Returns the text of the elements as an ordered string
string computerOptions =  this.WebDriver.CreateCommaDelimitedString(computerParts, true);
```
```csharp
// A by selector for a list of computer parts.
private static By computerParts = By.CssSelector("ul>#options");

// Returns the text of the elements without ordering them
string computerOptions =  this.WebDriver.CreateCommaDelimitedString(computerParts);
```
## Scroll By X and Y
Executes horizontal and vertical scrolling using the JavaScript [scroll][3] method based on the x and y arguments.
### Written as
```csharp
ExecuteScrolling(int xCoordinates, int yCoordinates);
```

### Example
```csharp
// Scrolls by this element
this.webDriver.ExecuteScrolling(50, -100);
```

## Return an Element's Attribute's Value
Gets the value of an attribute for an element. By default, it gets the value of the "value" attribute.
### Written as
```csharp
string GetElementAttribute(By bySelector, string attributeName);
```
### Examples
```csharp
// By selector for the page title
private static By pageTitle = By.CssSelector(".title");

// Returns the value of the element's value
string pageTitleValue = this.webDriver.GetElementAttribute(pageTitle);
```
```csharp
// By selector for the page title
private static By pageTitle = By.CssSelector(".title");

// Returns the value of the elements href
string pageTitleHrefValue =  this.webDriver.GetElementAttribute(pageTitle, "href");
```
## Get Text From Selected Dropdown Item
Gets the selected option's displayed text from a select element and returns it.
### Written as
```csharp
string GetSelectedOptionFromDropdown(By bySelector);
```
### Example
```csharp
// A by selector for a list of names from a dropdown
private static By nameDropdown = By.CssSelector("#namesDropdown");

// Gets the text of the current selected option
string nameSelected = this.webDriver.GetSelectedOptionFromDropdown(nameDropdown );
```

## Get List of Text From Selected Dropdown Items
Gets all the selected options' displayed text from a select element and returns them in a list.
### Written as
```csharp
List<string> GetSelectedOptionsFromDropdown(By bySelector);
```
### Example
```csharp
// A by selector for a list of names from a dropdown
private static By nameDropdown = By.CssSelector("#namesDropdown");

List<string> listOfNamesSelected = this.webDriver.GetSelectedOptionsFromDropdown(nameDropdown);
```

## Scroll until Element is in View
Uses the JavaScript [scrollIntoView][2] method to scroll an element into the view.
### Written as
```csharp
this.webDriver.ScrollIntoView(By bySelector);
```
### Example
```csharp
// By selector for the page title
private static By pageTitle = By.CssSelector(".title");

// Scrolls until the page title is in view
this.webDriver.ScrollIntoView(pageTitle);
```

## Scrolls Until Element is in View then Scroll to X and Y Coordinates
Uses the JavaScript [scrollIntoView][2] and [scroll][3] methods to scroll an element into view and then scroll based on an offset from the location of that element using the x and y arguments.
### Written as
```csharp
ScrollIntoView(By bySelector, int xOffset, int yOffset);
```
### Example
```csharp
// By selector for the page title
private static By pageTitle = By.CssSelector(".title");

// Scrolls to the page title and then scrolls by the x and y offsets
this.webDriver.ScrollIntoView(pageTitle , -20, 150);
```

## Select Dropdown Option by Option's Text
Selects an option element from a select element using the option's displayed text.
### Written as
```csharp
SelectDropDownOption(By bySelector, string optionText);
```
### Example
```csharp
// A by selector for a list of computer parts.
private static By computerParts = By.CssSelector("ul>#options");

// Selects the element inside the computer parts options where the text matches "Motherboard"
this.webDriver.SelectDropDownOption(computerParts, "Motherboard");
```

## Select Dropdown Option by Value
Selects an option element from a select element using the option's value attribute text.

### Written as
```csharp
SelectDropDownOptionByValue(By bySelector, string valueAttributeText);
```
### Example
```csharp
// A by selector for a list of computer parts.
private static By computerParts = By.CssSelector("ul>#options");

// Selects the element where the value attribute is equal to "1"
this.webDriver.SelectDropDownOptionByValue(computerParts , "1");
```

## Select Multiple Dropdown Options by Options Text
Selects multiple option elements from a list box using a list of strings of the option elements' displayed texts.

### Written as
```csharp
SelectMultipleElementsFromListBox(By bySelector, List<string> optionText);
```

### Example
```csharp
// A by selector for a list of computer parts.
private static By computerParts = By.CssSelector("ul>#options");

this.webDriver.SelectMultipleElementsFromListBox(computerParts , new List<string> { "Motherboard", "CPU", "Flux Capacitor" });
```

## Select Multiple Dropdown Options by Options Value
Selects multiple option elements from a list box using a list of strings of the option elements' value attribute texts.
### Written as
```csharp
SelectMultipleElementsFromListBoxByValue(By bySelector, List<string> optionValues);
```
### Example
```csharp
// A by selector for a list of computer parts.
private static By computerParts = By.CssSelector("ul>#options");

this.webDriver.SelectMultipleElementsFromListBoxByValue(computerParts , new List<string> { "1", "2" });
```

## Set Text Box
Enters text into an element. It also clears the element before entering anything. If the tabOff  is not set or is set to true, then the last key sent will be a tab, else it won't send a tab key at the end of typing the string argument.
### Written as
```csharp
SetTextBox(By bySelector, string textToEnter, bool tabOff = true);
```
### Example
```csharp
// By selector for a textField
private static By textField = By.Id("textBox);

// Sends the words "hello, world" to the text box, and then sends tab
this.webDriver.SetTextBox(textField , "hello, world");
```

## Type Text Slowly
Slowly types a string. Useful in scenarios where the normal Selenium SendKeys method types too quickly and causes issues. It sends key presses every 500 milliseconds.
### Written as
```csharp
SlowType(By bySelector, string textToEnter);
```

### Example
```csharp
// By selector for a textField
private static By textField = By.Id("textBox);

this.webDriver.SlowType(By.CssSelector("input[type=text]"), "hello, world");
```

[1]: https://developer.mozilla.org/en-US/docs/Web/API/HTMLElement/click
[2]: https://developer.mozilla.org/en-US/docs/Web/API/Element/scrollIntoView
[3]: https://developer.mozilla.org/en-US/docs/Web/API/Window/scroll

