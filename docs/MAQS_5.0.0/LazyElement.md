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
// TODO: Come back and fill this in
```

#### Example
```csharp
// TODO: Come back and fill this in
```
### GetTheClickableElement
This method requires a parent lazy element, which returns the first clickable element under the parent that matches the given selector. 
#### Written as
```csharp
this.ParentElementID.GetClickableElement(By.MethodOfSelction("Selector"));
```

#### Example
```csharp
// Initializing Lazy Element for a parent element, in this case a table
LazyElement table = 
    new LazyElement(
    this.TestObject,
    By.CssSelector("#entryTable"),
    "The table element which contains all rows of user data");

// Find the first clickable link within the table
table.GetTheClickableElement(By.HTMLSelector(".href"));
```
### GetTheExistingElement
This method requires a parent lazy element, which returns the first visable element under the parent that matches the given selector. 
#### Written as
```csharp
this.ParentElementID.GetClickableElement(By.MethodOfSelction("Selector"));
```

#### Example
```csharp

```
### GetTheVisibleElement
This method waits for and returns the visible element. 
#### Written as
```csharp

```

#### Example
```csharp

```
### GetValue
????
#### Written as
```csharp

```

#### Example
```csharp

```
### SendKeys
????
#### Written as
```csharp

```

#### Example
```csharp

```
### SendSecretKeys
????
#### Written as
```csharp

```

#### Example
```csharp

```
### Submit
????
#### Written as
```csharp

```

#### Example
```csharp

```