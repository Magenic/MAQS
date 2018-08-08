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
This method allows the user to clear a specific element.

#### Written as
```csharp

```

#### Example
```csharp

```





