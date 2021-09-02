# Updating from MAQS 6 to MAQS 7

## NuGet package changes

In MAQS 6.0, the base projects have been split up into their own NuGet packages.  In MAQS 5.0, all the base libraries were in the Magenic.MAQS NuGet package.  In MAQS 6.0, there are now:

1. Magenic.Maqs - Base NuGet project that references all NuGet packages
2. Magenic.Maqs.Selenium
3. Magenic.Maqs.Base
4. Magenic.Maqs.Mongo
5. Magenic.Maqs.Appium
6. Magenic.Maqs.Database
7. Magenic.Maqs.Email
8. Magenic.Maqs.WebService

## Lazy Element changes

1. FindElement now returns a LazyElement.  
    * If you are looking for an IWebElement, then you will need to call FindRawElement
2. FindElements now returns a List<LazyElement>
    * If you are looking for an IWebElement, then you will need to call FindRawElements

### Examples

``` csharp
public void LazyElementFindElement()
{
    IWebElement firstElement = this.FlowerTableLazyElement.FindRawEleme(By.CssSelector("THEAD TH"));
    Assert.AreEqual("Flowers", firstElement.Text);
}

public void LazyElementFindElementRespectAction()
{
    IWebElement firstElement = this.DivRoot.FindElement(this.DisabledItem.By);
    firstElement.Click();
}

```
