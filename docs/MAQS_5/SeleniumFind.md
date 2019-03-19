# <img src="resources/maqslogo.ico" height="32" width="32"> Selenium Find

## Overview
Library for extending find capabilities

# Available methods
[Element](#Element)  
[ElementWithText](#ElementWithText)  
[IndexOfElementWithText](#IndexOfElementWithText)  


##  Element
Find an element 
*Waits timeout period if no element is found. Optional parameter decides if a missing element will throw an exception or just return null.*
```csharp
this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
IWebElement element = this.WebDriver.Find().Element(AutomationNamesLabel);

Assert.AreEqual(element.Text, "Names");
```
##  ElementWithText
Find an element that has a specific text value. 
*Ideally used with selectors that can match more than one element.  Will only find the first instance if there are multiples.* 
```csharp
this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
Assert.IsNotNull(this.WebDriver.Find().ElementWithText(By.CssSelector("BUTTON"), "Login"), "Element was not found");
```
##  IndexOfElementWithText
Find index of the element with specific text 
*Ideally used with selectors that can match more than one element.  Will only find the first instance if there are multiples.* 

### From a selector
```csharp
this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
Assert.AreEqual(this.WebDriver.Find().IndexOfElementWithText(By.CssSelector("#FlowerTable TD"), "Red"), 3);
```
### From a collection
```csharp
this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
ICollection<IWebElement> elemList = this.WebDriver.FindElements(By.CssSelector("#FlowerTable TD"));
Assert.AreEqual(this.WebDriver.Find().IndexOfElementWithText(elemList, "Red"), 3);
```