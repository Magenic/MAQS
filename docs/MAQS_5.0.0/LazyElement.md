# <img src="resources/maqslogo.ico" height="32" width="32"> Lazy Element

## Overview 
A Lazy Element is a handler that enables lazy initialization(not creating an object in memory til its needed)with built in wait methods. 
## Lazy Element Methods

### Intialization 
Intializing Lazy Element

#### Written as
```csharp
// Initializing Lazy Element
//*Note*
private LazyElement ElementID TODO: Var note
    {
        get 
        { 
            return new LazyElement(
                    this.TestObject, 
                    By.CssSelector("#ElementSelector"), 
                    "User friendly name for logging"); 
        }
    }
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

### FindElement 
This method requires a parent lazy element, which returns the first element under the parent that matches the given selector.
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
### FindElements
This method requires a parent lazy element, which returns an array of all elements under the parent that matches the given selector.
#### Written as
```csharp
this.ParentElementID.FindElements(By.MethodOfSelction("Selector"));
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
table.FindElements(By.HTMLSelector(".tr")); // TODO: HTML??????
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
### GetTheClickableElement
This method waits for the element to be clickable and returns it. 
#### Written as
```csharp
this.ElementID.GetTheClickableElement();
```

#### Example
```csharp
LazyElement homeButton = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#HomeButton"),
    "The button to return to home");

homeButton.GetTheClickableElement();
```
### GetTheExistingElement
This method waits for the element to exist and returns it. 
#### Written as
```csharp
this.ElementID.GetTheExistingElement();
```

#### Example
```csharp
LazyElement table = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#entryTable"),
    "The table element which contains all rows of user data");

table.GetTheExistingElement();
```
### GetTheVisibleElement
This method waits the element to be visible and returns it. 
#### Written as
```csharp
this.ElementID.GetTheVisibleElement();
```

#### Example
```csharp
LazyElement welcomeMessage = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#WelcomeMessage"),
    "The Welcome Message on the HomePage");

welcomeMessage.GetTheVisibleElement();
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
### SendKeys
Simulates typing text into the lazy element.
#### Written as
```csharp
this.ElementID.SendKeys("UserName")
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
Simulates typing text into the lazy element without logging.
#### Written as
```csharp
this.ElementID.SendSecretKeys("Password1")
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
Submits this element to the web server.
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