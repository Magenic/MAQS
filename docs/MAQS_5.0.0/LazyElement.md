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
private LazyElement ElementID
    {
        get { return new LazyElement(this.TestObject, By.CssSelector("#ElementSelector"), "User friendly name for logging"); }
    }
```

#### Example  
```csharp
// Initializing Lazy Element
private LazyElement DialogOneButton
    {
        get { return new LazyElement(this.TestObject, By.CssSelector("#showDialog1"), "Dialog"); }
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
// Initializing Lazy Element
private LazyElement DialogOneButton
    {
        get { return new LazyElement(this.TestObject, By.CssSelector("#showDialog1"), "Dialog"); }
    }

//Using Lazy Element Method
[TestMethod]
[TestCategory(TestCategories.Selenium)]
    public void LazyElementClick()
    {
        this.DialogOneButton.Click();
    }
```
### Clear 
This method clears a specific element.

#### Written as
```csharp

```

#### Example
```csharp

```

### FindElement 
This method returns the first element based on the referenced element and selection strategy.
#### Written as
```csharp

```

#### Example
```csharp

```
### FindElements
This method returns all elements based on the referenced element and selection strategy.
#### Written as
```csharp

```

#### Example
```csharp

```
### GetAttribute
This method returns the attribute's value.
#### Written as
```csharp

```

#### Example
```csharp

```
### GetCssValue
This method returns the Css value for a given attribute.
#### Written as
```csharp

```

#### Example
```csharp

```
### GetProperty
This method returns the value of a Javascript property of an element.
#### Written as
```csharp

```

#### Example
```csharp

```
### GetTheClickableElement
This method waits for and returns the clickable element. 
#### Written as
```csharp

```

#### Example
```csharp

```
### GetTheExistingElement
This method waits for and returns the element. 
#### Written as
```csharp

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