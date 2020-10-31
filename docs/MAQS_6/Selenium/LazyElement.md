# <img src="resources/maqslogo.ico" height="32" width="32"> Lazy Element

## Overview 
A Lazy Element is a handler that enables lazy initialization(not creating an object in memory til its needed)with built in wait methods. 

## Lazy Element Methods

### Initialization 
Initializing Lazy Element

#### Written as
```csharp
// Initializing Lazy Element
LazyElement elementID =  new LazyElement(this.TestObject, By.CssSelector("#ElementSelector"), "User friendly name for logging"); 
```

### Click 
This method allows the user to click a specific element.

#### Written as
```csharp
this.ElementID.Click();
```

#### Example
```csharp
// Initializing Lazy Element for a button
LazyElement button = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#button1"),
    "Radio Button Choice 1");

//Clicking the Lazy Element button
button.Click();
```
### Clear 
This method clears a specific element's value.

#### Written as
```csharp
this.ElementID.Clear();
```

#### Example
```csharp
// Initializing Lazy Element for a input field
LazyElement userNameInputField = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#inputField1"),
    "User Name Field");

//Clearing the Lazy Element user name value
userNameInputField.Clear();
```

### Displayed 
This method checks if an element is displayed.

#### Written as
```csharp
this.ElementID.Displayed;
```

#### Example
```csharp
// Initializing Lazy Element for a dialog button
LazyElement dialogOneButton = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#showDialog1"),
    "Dialog button 1");

// Check that the button is displayed 
Assert.AreEqual(true, dialogOneButton.Displayed);
```

### Enabled 
This method checks if an element is enabled.

#### Written as
```csharp
this.ElementID.Enabled;
```

#### Example
```csharp
// Initializing Lazy Element for a dialog button
LazyElement dialogOneButton = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#showDialog1"),
    "Dialog button 1");

// Check that the button is enabled 
Assert.AreEqual(true, dialogOneButton.Enabled);
```

### Exists 
This method checks if an element exists.

#### Written as
```csharp
this.ElementID.Exists;
```

#### Example
```csharp
// Initializing Lazy Element for a dialog button
LazyElement dialogOneButton = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#showDialog1"),
    "Dialog button 1");

// Check that the button is exists 
Assert.AreEqual(true, dialogOneButton.Exists);
```  

### ExistsNow 
This method checks if an element exists right now. - AKA We don't wait for the element

#### Written as
```csharp
this.ElementID.ExistsNow;
```

#### Example
```csharp
// Initializing Lazy Element for a dialog button
LazyElement dialogOneButton = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#showDialog1"),
    "Dialog button 1");

// Check that the button is exists right now
Assert.AreEqual(true, dialogOneButton.ExistsNow);
```  

### FindRawElement 
This method requires a parent lazy element, which returns the first element under the parent that matches the given selector as a WebElement, not a lazy element.
#### Written as
```csharp
this.ParentElementID.FindRawElement(By.MethodOfSelction("Selector"));
```

#### Example
```csharp
// Initializing Lazy Element for a parent element, in this case a table
LazyElement table = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#entryTable"),
    "The table element which contains all rows of user data");

// Find the first row element in the table
table.FindRawElement(By.HTMLSelector(".tr"));
```

### FindElement 
This method requires a parent lazy element, which returns the first element under the parent that matches the given selector as a LazyElement.
#### Written as
```csharp
this.ParentElementID.FindElement(By.MethodOfSelction("Selector"));
```

#### Example
```csharp
// Initializing Lazy Element for a parent element, in this case a table
LazyElement table = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#entryTable"),
    "The table element which contains all rows of user data");

// Find the first row element in the table
table.FindElement(By.HTMLSelector(".tr"));
```
### FindRawElements
This method requires a parent lazy element, which returns an array of all elements under the parent that matches the given selector from the lazy element as a List of WebElements, not LazyElements.

#### Written as
```csharp
this.ParentElementID.FindRawElements(By.MethodOfSelection("Selector"));
```

#### Example
```csharp
// Initializing Lazy Element for a parent element, in this case a table
LazyElement table = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#entryTable"),
    "The table element which contains all rows of user data");

// Finds all row elements in the table
table.FindRawElements(By.HTMLSelector(".tr"));
```

### FindElements
This method requires a parent lazy element, which returns an array of all elements under the parent that matches the given selector from the lazy element as a List of LazyElements.
#### Written as
```csharp
this.ParentElementID.FindElements(By.MethodOfSelection("Selector"));
```

#### Example
```csharp
// Initializing Lazy Element for a parent element, in this case a table
LazyElement table = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#entryTable"),
    "The table element which contains all rows of user data");

// Finds all row elements in the table
table.FindElements(By.HTMLSelector(".tr"));
```
### GetAttribute
This method returns the attribute's value from the lazy element.
#### Written as
```csharp
this.ElementID.GetAttribute("HTML Attibute Name");
```

#### Example
```csharp
LazyElement passwordField = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#userPassword"),
    "The user's password field");

passwordField.GetAttibute("focused");
```
### GetCssValue
This method returns the CSS value from the lazy element.
#### Written as
```csharp
this.ElementID.GetCssValue("CSS Value Name");
```

#### Example
```csharp
LazyElement welcomeText = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#welcome"),
    "The welcome text for the home page");

welcomeText.GetCssValue("color");
```
### GetProperty
This method returns the value of an element's Javascript property.
#### Written as
```csharp
this.ElementID.GetProperty("Property Name");
```

#### Example
```csharp
LazyElement chatWindow =
    new LazyElement(
    this.TestObject,
    By.CssSelector("#ChatWindow"),
    "The Chat Window popup");

chatWindow.GetProperty("isTyping");
```
### GetSelectedOptionFromDropdown
This method returns the selection option of a dropdown.
#### Written as
```csharp
this.ElementID.GetSelectedOptionFromDropdown();
```

#### Example
```csharp
LazyElement stateDropdown =
    new LazyElement(
    this.TestObject,
    By.CssSelector("#state"),
    "The State dropdown");

chatWindow.GetSelectedOptionFromDropdown();
```
### GetSelectedOptionsFromDropdown
This method returns the selection options of a dropdown.
#### Written as
```csharp
this.ElementID.GetSelectedOptionsFromDropdown();
```

#### Example
```csharp
LazyElement stateMultiSelect =
    new LazyElement(
    this.TestObject,
    By.CssSelector("#state"),
    "The state multiselectDropdown");

chatWindow.GetSelectedOptionsFromDropdown();
```
### GetRawClickableElement
This method waits for the element to be clickable and returns it. 
#### Written as
```csharp
this.ElementID.GetRawClickableElement();
```

#### Example
```csharp
LazyElement homeButton = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#HomeButton"),
    "The button to return to home");

homeButton.GetRawClickableElement();
```
### GetRawExistingElement
This method waits for the element to exist and returns it. 
#### Written as
```csharp
this.ElementID.GetRawExistingElement();
```

#### Example
```csharp
LazyElement table = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#entryTable"),
    "The table element which contains all rows of user data");

table.GetRawExistingElement();
```
### GetRawVisibleElement
This method waits the element to be visible and returns it. 
#### Written as
```csharp
this.ElementID.GetRawVisibleElement();
```

#### Example
```csharp
LazyElement welcomeMessage = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#WelcomeMessage"),
    "The Welcome Message on the HomePage");

welcomeMessage.GetRawVisibleElement();
```
### GetValue
This method returns the HTML attribute "value" from the lazy element.
#### Written as
```csharp
this.ElementID.GetValue()
```

#### Example
```csharp
LazyElement homeButton = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#HomeButton"),
    "The button to return to home");

passwordField.GetValue();
```
### SelectDropDownOption
This method selects the provided option from the lazy element.
#### Written as
```csharp
this.ElementID.SelectDropDownOption("DROPDOWN OPTION")
```

#### Example
```csharp
LazyElement stateDropdown =
    new LazyElement(
    this.TestObject,
    By.CssSelector("#state"),
    "The State dropdown");

stateDropdown.SelectDropDownOption("Minnesota");
```
### SelectDropDownOptionByValue
This method selects the provided option from the lazy element by value.
#### Written as
```csharp
this.ElementID.SelectDropDownOption("DROPDOWN OPTION")
```

#### Example
```csharp
LazyElement stateDropdown =
    new LazyElement(
    this.TestObject,
    By.CssSelector("#state"),
    "The State dropdown");

stateDropdown.SelectDropDownOptionByValue("23");
```

### DeselectDropDownOption
This method deselects the provided option from the lazy element.
#### Written as
```csharp
this.ElementID.DeselectDropDownOption("DROPDOWN OPTION")
```

#### Example
```csharp
LazyElement stateDropdown =
    new LazyElement(
    this.TestObject,
    By.CssSelector("#state"),
    "The State dropdown");

stateDropdown.SelectDropDownOption("California");
stateDropdown.SelectDropDownOption("Minnesota");

// Deselecting
stateDropdown.DeselectDropDownOption("California");
```
### DeselectDropDownOptionByValue
This method deselects the provided option from the lazy element by value.
#### Written as
```csharp
this.ElementID.DeselectDropDownOptionByValue("DROPDOWN VALUE")
```

#### Example
```csharp
LazyElement stateDropdown =
    new LazyElement(
    this.TestObject,
    By.CssSelector("#state"),
    "The State dropdown");

stateDropdown.SelectDropDownOptionByValue("5");
stateDropdown.SelectDropDownOptionByValue("23");

// Deselecting
stateDropdown.DeselectDropDownOptionByValue("5");
```

### DeselectAllDropDownOptions
This method deselects all options from the lazy element.
#### Written as
```csharp
this.ElementID.DeselectAllDropDownOptions()
```

#### Example
```csharp
LazyElement stateDropdown =
    new LazyElement(
    this.TestObject,
    By.CssSelector("#state"),
    "The State dropdown");

stateDropdown.SelectDropDownOptionByValue("five");
stateDropdown.SelectDropDownOptionByValue("twentythree");

// Deselecting all
stateDropdown.DeselectAllDropDownOptions();
```
### SendKeys
This method enters the provided text into the lazy element.
#### Written as
```csharp
this.ElementID.SendKeys("MESSAGE BEING SENT")
```

#### Example
```csharp
LazyElement chatWindow =
    new LazyElement(
    this.TestObject,
    By.CssSelector("#ChatWindow"),
    "The Chat Window popup");

chatWindow.SendKeys("Lorem Ipsum");
```
### SendSecretKeys
This method enters the provided text into the lazy element without test logging.
#### Written as
```csharp
this.ElementID.SendSecretKeys("SECRET MESSAGE BEING SENT")
```

#### Example
```csharp
LazyElement Password =
    new LazyElement(
    this.TestObject,
    By.CssSelector("#Password"),
    "The password field");

chatWindow.SendSecretKeys("Password1");
```
### Submit
This method submits the lazy element to the web server.
#### Written as
```csharp
this.ElementID.Submit()
```

#### Example
```csharp
LazyElement WebForm =
    new LazyElement(
    this.TestObject,
    By.CssSelector("#WebForm"),
    "The Web From for the claim");

WebForm.Submit()
```