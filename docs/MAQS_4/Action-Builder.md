# <img src="resources/maqslogo.ico" height="32" width="32"> Action Builder
The action builder class contains many advanced methods using interactions with selenium actions class.  This includes actions that need to be preformed synchronously.

## Hover Over an Element
Hovers the mouse over an element.

### Written as
```csharp
void HoverOver(By bySelector)
```
### Examples
```csharp
private static By javascriptAlertButton = By.CssSelector(".javaScriptAlertButton");

this.webDriver.HoverOver(javascriptAlertButton);
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
Locates an element, and right clicks it.

### Written as
```csharp
// Drags an element left 20 pixels and drops it
this.webDriver.RightClick(By bySelector);

private static By titleImage = By.CssSelector(".title>img");

// Right clicks the title image
this.webDriver.SlideElement(titleImage);
```
