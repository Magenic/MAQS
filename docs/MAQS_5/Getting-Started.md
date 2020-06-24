# <img src="resources/maqslogo.ico" height="32" width="32"> Getting Started 

## Installing and Building Your First Project
After the MAQS Visual Studio Extension has been installed, you're ready to start testing.

# Visual Studio Professional (or Enterprise) 2017 or above.

## Installation

### There are 2 ways to install MAQS Templates:
    Option 1. Install via Visual Studio 
    Option 2. Install via Marketplace

## Option 1. Install MAQS Templates Using Visual Studio
1. Open Visual Studio and open "Extensions and Updates"  

### ![Extensions and updates](resources/ExtensionsAndUpdates.PNG)

2. Find and download MAQS  

### ![From store](resources/FromStore.PNG)

### Option 2. Install MAQS Templates Using The VS Marketplace
The MAQS Visual Studio Extension contains a collection of templates for NUnit, VSTest, Test Classes, and Page Models.
It can be downloaded from the [Microsoft Marketplace](https://marketplace.visualstudio.com/items?itemName=vs-publisher-1465771.MAQSOpenFramework)

&nbsp;

## Creating a Project

1. Go To File > New > Project  
![New Project](resources/NewProject1.png)  

2. Under Templates in the side panel, select "Magenic's Open Test" 
![New Project](resources/NewProject2.png)  

3. Finally, name the solution whatever you'd like and click "ok".
### Adding a Page Model
To add a new Page Model from a template

1. Right click PageModel project.
2. Click "Add>New Item". This will open a list of templates.  
![New Page Model](resources/NewPageModel1.png)  
3. Under "Magenic's Open Test" select the "MAQS Selenium Page Model Class"
3. Name your page model class
4. Click the "Add" Button  
![New Page Model](resources/NewPageModel2.png)  

This will add a template for a Page Object Model.  The template is filled with sample code and methods.  The methods included are common methods used on every page.

### Adding a Test
#### VSTest Test
1. Right click PageModel project.
2. Click "Add>New Item". This will open a list of templates.  
![New Test](resources/NewTest1.png)  
3. Under "Magenic's Open Test" select the "MAQS Selenium VS Test Class"
4. Name your test class
5. Click the "Add" Button  
![New Test](resources/NewTest2.png)  

The VSTest class will generate a template for a VSTest TestClass and include a sample VSTest TestMethod. Visual Studio will use VSTest test runner to execute tests when the solution is run.
#### NUnit Test
1. Right click PageModel project.
2. Click "Add>New Item". This will open a list of templates.  
![New NUnit](resources/NewTest1.png)
3. Expand the "Magenic's Open Test"
4. Select "Maqs Selenium NUnit Test Class"
5. Name your test class
6. Click the "Add" Button  
![New NUnit](resources/NewNUnitTest2.png)


This will generate a template for the NUnit TestFixture with a sample NUnit Test.
## Running Tests
Tests can be run through the command line using MSTest or NUnit. You can also run through Visual Studio using the test explorer.

### Enable Test Explorer
To enable the Test Explorer window
1. Go to "Test>Windows>Test Explorer"  
![Test Explorer](resources/TestExplorer1.png)

### Running Tests from Test Explorer
There are 3 options to running tests from test explorer
1. Tests can be run individually by right clicking a test and clicking "Debug Selected Tests" to run in debug mode
2. Tests can also be run individually or in multiples(based on selected tests) by right clicking and selecting "Run Selected Tests" to run the tests normally.
3. All tests can be run by clicking "Run All" 

### Organizing Tests

An easy way to group tests is to give those tests a trait attribute. These are written in the test classes. These traits can be used to group tests in the test explorer.

#### VSTest Traits
Above the test method, add an attribute with the TestCategory tag.

Written As
```csharp
[TestCategory(string testCategoryName)]
```

Examples
```csharp
[TestCategory("Smoke Tests")]
```

```csharp
string testCategory = "Login Tests"

[TestCategory(testCategory)]
```
Test methods with multiple TestCategory attributes will add that test case to each attribute group, but the test will only be run once when running all tests.

#### Grouping Tests
Tests can be organized to filter through what could potentially be a test solution with thousands of tests. There are two ways to change the grouping:

1. Right-click inside the Test Explorer window and select "Group By".  
![Test Grouping](resources/Groupin2.png)

2. Left-click the Group by icon button in the top left corner of the Test Explorer, to the left of the Search field.   
![Test Explorer](resources/Groupin1.png)
 
#### NUnit Traits
Above the test method, add an attribute with the Category tag.

Written As
```csharp
[Category(string testCategoryName)]
```

Examples
```csharp
[Category("Smoke Tests")]
```

```csharp
string testCategory = "Login Tests"

[Category(testCategory)]
```
Test methods with multiple Category attributes will add that test case to each attribute group, but the test will only be run once when running all tests.

# VS Code

## Template install


2. In the VS Code terminal type the following to get the most current 5 version.

```dotnet new --install Magenic.Maqs.Templates::5.8.2```

3. You should then see the MAQS templates available

### ![MAQS templates in template](resources/DotNetTemplates.png)

## Creating a Project

1. Go to the VS code terminal

2. type  ```dotnet new Maqs.Selenium```

3. type ```dotnet build```

### Adding a Page Model
### Adding a Test

Unfortunatly no item templates are available for .NET core, only project templates. All new classes will need to be manually created

## Running Tests
Tests can be run through the dotnet cli. You can also run through VS code using the "C# for Visual Studio Code" extension.

### Running Tests using C# for Visual Studio Code
There are 3 options to running tests from VS code
1. Tests can be run individually by clicking the "Run Test" or "Debug Test" buttons above the test method
3. All tests can be run on a page by clicking the "Run All Tests" or "Debug All Tests" buttons above the test class

### Organizing Tests

An easy way to group tests is to give those tests a trait attribute. These are written in the test classes. These traits can be used to filter test for running.

#### VSTest Traits
Above the test method, add an attribute with the TestCategory tag.

Written As
```csharp
[TestCategory(string testCategoryName)]
```

Examples
```csharp
[TestCategory("SmokeTests")]
```

```csharp
string testCategory = "Login Tests"

[TestCategory(testCategory)]
```
Test methods with multiple TestCategory attributes will add that test case to each attribute group, but the test will only be run once when running all tests.
 
#### NUnit Traits
Above the test method, add an attribute with the Category tag.

Written As
```csharp
[Category(string testCategoryName)]
```

Examples
```csharp
[Category("SmokeTests")]
```

```csharp
string testCategory = "Login Tests"

[Category(testCategory)]
```

### Running Tests using dotnet cli
1. Run all tests by typing ```dotnet test```

2. Run filtered tests by typing ```dotnet test --filter TestCategory==SmokeTests```

 - learn more about dotnet test --filter at [docs.microsoft.com](https://docs.microsoft.com/en-us/dotnet/core/testing/selective-unit-tests)

## Configurations
There are a number of configurations included with MAQS.  Checkout the [MAQS Configurations](MAQS_6/General/EnterpriseConfiguration.md) guide.