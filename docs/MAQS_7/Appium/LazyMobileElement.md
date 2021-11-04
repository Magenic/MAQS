# <img src="resources/maqslogo.ico" height="32" width="32"> Lazy Mobile Element

## Overview
The Lazy Mobile Element class is used for dynamically finding and interacting with elements.  It implements the IWebElement interface and can be used like any normal element.  The benefit is that lazy element handles waits, retires, logging and caching for you.

# Available methods

## By
Get By selector for the lazy element.
```csharp
LazyMobileElement inputField = new LazyMobileElement(this.TestObject, By.CssSelector("#InputField"));

By selector = inputField.By;
```

## Log
Get logger for the lazy element.
```csharp
LazyMobileElement inputField = new LazyMobileElement(this.TestObject, By.CssSelector("#InputField"));

 ILogger Log = inputField.Log;
```

## Clear
Find and clear the specified element.
```csharp
LazyMobileElement inputField = new LazyMobileElement(this.TestObject, By.CssSelector("#InputField"));

inputField.Clear();
```

## Click
Find and click the specified element.
```csharp
LazyMobileElement okButton = new LazyMobileElement(this.TestObject, By.CssSelector("#OkButton"));

okButton.Click();
```

## SendKeys
Find and send keys to the specified element.
```csharp
LazyMobileElement inputField = new LazyMobileElement(this.TestObject, By.CssSelector("#InputField"));

inputField.SendKeys("VALUE");
```

## SendSecretKeys
Find and send keys to the specified element, but suppress logging while doing so.
```csharp
string privateValue = GetPrivateValue();
LazyMobileElement inputField = new LazyMobileElement(this.TestObject, By.CssSelector("#InputField"));

inputField.SendSecretKeys(privateValue);
```

## Submit
Find and submit the specified element.
```csharp
LazyMobileElement submitButton = new LazyMobileElement(this.TestObject, By.CssSelector("#[type='submit']"));

submitButton.Submit();
```

## DeselectAllDropDownOptions
Find and deselect all drop down options for specified element.
```csharp
LazyMobileElement multiSelect = new LazyMobileElement(this.TestObject, By.CssSelector("#multiSelect"));

multiSelect.DeselectAllDropDownOptions();
```

## DeselectDropDownOption
Find and deselect option with specific text from specified drop down element.
```csharp
LazyMobileElement multiSelect = new LazyMobileElement(this.TestObject, By.CssSelector("#multiSelect"));

multiSelect.DeselectDropDownOption("OPTION1");
```

## DeselectDropDownOptionByValue
Find and deselect option with specific value from specified drop down element.
```csharp
LazyMobileElement multiSelect = new LazyMobileElement(this.TestObject, By.CssSelector("#multiSelect"));

multiSelect.DeselectDropDownOptionByValue("one");
```

## SelectDropDownOption
Find and select option with specific text from specified drop down element.
```csharp
LazyMobileElement multiSelect = new LazyMobileElement(this.TestObject, By.CssSelector("#multiSelect"));

multiSelect.SelectDropDownOption("OPTION1");
```

## SelectDropDownOptionByValue
Find and select option with specific value from specified drop down element.
```csharp
LazyMobileElement multiSelect = new LazyMobileElement(this.TestObject, By.CssSelector("#multiSelect"));

multiSelect.SelectDropDownOptionByValue("one");
```

## GetSelectedOptionFromDropdown
Find and get selected option from specified drop down element.
```csharp
LazyMobileElement multiSelect = new LazyMobileElement(this.TestObject, By.CssSelector("#multiOrSingleSelect"));

string selectedValue = multiSelect.GetSelectedOptionFromDropdown();
```

## GetSelectedOptionsFromDropdown
Find and get selected options from specified drop down element.
```csharp
LazyMobileElement multiSelect = new LazyMobileElement(this.TestObject, By.CssSelector("#multiSelect"));

List<string>  selectedValues = multiSelect.GetSelectedOptionsFromDropdown();
```

## GetAttribute
Find and gets attribute value for the specified element.
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

string attributeValue = element.GetAttribute("id");
```

## GetValue
Find and gets value for the specified element.
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

string elementValue = element.GetValue();
```

## GetCssValue
Find and gets CSS value for the specified element.
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

string elementCssValue = element.GetCssValue("max-width");
```

## GetProperty
Find and gets property value for the specified element.
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

string elementCssValue = element.GetProperty("id");
```

## GetRawVisibleElement
Find and gets the raw (non-lazy) visible element.  
**Wait for element to be visible*
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

IWebElement rawNoneLazyElement = element.GetRawVisibleElement();
```

## GetRawClickableElement
Find and gets the raw (non-lazy) visible element.  
**Wait for element to be clickable*
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

IWebElement rawNoneLazyElement = element.GetRawClickableElement();
```

## GetRawExistingElement
Find and gets the raw (non-lazy) visible element.  
**Wait for element to exist*
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

IWebElement rawNoneLazyElement = element.GetRawExistingElement();
```


## FindElement
Find and returns a child (lazy) element.  
**Wait for element to exist*
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

IWebElement subLazyElement = element.FindElement(By.CssSelector("#SubElement"));
```


## FindRawElement
Find and returns a child (non-lazy) element.  
**Wait for element to exist*
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

IWebElement subLazyElement = element.FindRawElement(By.CssSelector("#SubElement"));
```

## FindElements
Find and returns child (lazy) elements.  
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

ReadOnlyCollection<IWebElement> subLazyElements = element.FindElement(By.CssSelector("#SubElement"));
```

## FindRawElements
Find and returns child (non-lazy) elements.  
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

ReadOnlyCollection<IWebElement> subLazyElements = element.FindRawElements(By.CssSelector("#SubElement"));
```

## Text
Find and get element Text.
**Wait for element to be visible*   
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

string text = element.Text();
```

## Location
Find and get element Location.  
**Wait for element to be visible* 
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

Point point = element.Location();
```

## Size
Find and get element Size.  
**Wait for element to be visible* 
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

Size size = element.Size();
```

## TagName
Find and get element TagName.  
**Wait for element to exist*
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

string tagName = element.TagName();
```


## Displayed
Find and get element Displayed.  
**Wait for element to be visible*
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

bool displayed = element.Displayed();
```

## Selected
Find and get element Selected.  
**Wait for element to be visible*
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

bool selected = element.Selected();
```

## Enabled
Find and get element Enabled.  
**Wait for element to be visible*
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

bool enabled = element.Enabled();
```

## Exists
Find and get element Exists.  
**Does not wait for element to exist*
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

bool exists = element.Exists();
```

## ExistsNow
Find and get element Exists right now.  
**Wait for element to exist*
```csharp
LazyMobileElement element = new LazyMobileElement(this.TestObject, By.CssSelector("#testElement"));

bool exists = element.ExistsNow();
```