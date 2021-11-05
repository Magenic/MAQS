# <img src="resources/maqslogo.ico" height="32" width="32"> Action Builder

## Overview
The action builder class contains many advanced methods using interactions with selenium actions class.  This includes actions that need to be preformed synchronously.

## Hover Over an Element
Hovers the mouse over an element
### Written as
```csharp
void HoverOver()
```
### Examples
```csharp
var hoverElement = new LazyElement(this.TestObject, bySelector)
hoverElement.HoverOver();
```
Find an element and hover over it

### Written as
```csharp
void HoverOver(By bySelector)
```
### Examples
```csharp
private static By javascriptAlertButton = By.CssSelector(".javaScriptAlertButton");

this.webDriver.HoverOver(javascriptAlertButton);
```






## Drag and drop an Element
Drag and drop an element onto another element
### Written as
```csharp
void DragAndDrop(IWebElement destination)
```
### Examples
```csharp
var dragElement = new LazyElement(this.TestObject, bySelectorSource)
var dropElement = new LazyElement(this.TestObject, bySelectorDestination)
dragElement.DragAndDrop(dropElement);
```
Find a source element, destination element, then drag the source to the destination
### Written as
```csharp
void DragAndDrop(By source, By destination)
```
### Examples
```csharp
var draggable = By.Id("draggable");
var droppable = By.Id("droppable");

this.webDriver.DragAndDrop(draggable, droppable);
```


## Drag and drop an Element with offset
Drag and drop an element onto another element with an offset
### Written as
```csharp
void DragAndDropToOffset(IWebElement destination, int pixelsXOffset, int pixelsYOffset)
```
### Examples
```csharp
var dragElement = new LazyElement(this.TestObject, bySelectorSource)
var dropElement = new LazyElement(this.TestObject, bySelectorDestination)
dragElement.DragAndDropToOffset(dropElement, 20, 0);
```
Find a source element, destination element, then drag the source to the destination with an offset
### Written as
```csharp
void DragAndDropToOffset(By source, By destination, int pixelsXOffset, int pixelsYOffset)
```
### Examples
```csharp
var draggable = By.Id("draggable");
var droppable = By.Id("droppable");

this.webDriver.DragAndDropToOffset(draggable, droppable, 20, 0);
```














## Press Modifier Keys
Press modifier keys synchronously.  Each key will be pressed at the same time.  The Keys class is used to quickly write keys.

### Written as
```csharp
void PressModifierKey(string Keys)
```
### Examples
```csharp
// Presses the arrow down, backspace, control, and divide keys all at once
this.webDriver.PressModifierKey(Keys.ArrowDown + Keys.Backspace + Keys.Control + Keys.Divide);
```
```csharp
// Presses the arrow down key
this.webDriver.PressModifierKey(Key.ArrowDown);
```

## Slide Element by X offset
Drags and drops an element by an X pixel offset.

### Written as
```csharp
void SlideElement(int pixelsOffset)
```
### Examples
```csharp
var element = new LazyElement(this.TestObject, bySelector)

// Drags an element left 20 pixels and drops it
element.SlideElement(-20);
```
```csharp
var element = new LazyElement(this.TestObject, bySelector)

// Drags an element right 20 pixels and drops it
element.SlideElement(20)
```

Find an element and drags and drops it by an X pixel offset.
### Written as
```csharp
void SlideElement(By bySelector, int pixelsOffset)
```
### Examples
```csharp
// Drags an element left 20 pixels and drops it
this.webDriver.SlideElement(element, -20);
```
```csharp
// Drags an element right 20 pixels and drops it
this.webDriver.SlideElement(element, 20)
```

## Right Click an Element
Right clicks on an element.
### Written as
```csharp
void RightClick()
```
### Examples
```csharp
// Get the image element
By titleImage = By.CssSelector(".title>img");
var titleImageElement = new LazyElement(this.TestObject, titleImage)

// Right clicks the title image
titleImageElement.RightClick();
```
Locates an element, and right click it.
### Written as
```csharp
void RightClick(By bySelector)
```
### Examples
```csharp
// Right clicks the that matches the by selector
this.webDriver.RightClick(By bySelector);
```

## Double Click an Element
Double clicks on an element.
### Written as
```csharp
void DoubleClick()
```
### Examples
```csharp
// Get the image element
By titleImage = By.CssSelector(".title>img");
var titleImageElement = new LazyElement(this.TestObject, titleImage)

// Double clicks the title image
titleImageElement.DoubleClick();
```
Locates an element, and double click it.
### Written as
```csharp
void DoubleClick(By bySelector)
```
### Examples
```csharp
// Double clicks the that matches the by selector
this.webDriver.DoubleClick(By bySelector);
```