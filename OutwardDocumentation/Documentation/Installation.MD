#Getting Started
## Requirements
Visual Studio Professional (or Enterprise) 2012 and above
MSDN subscription

#Installing MAQS
## MAQS Visual Studio Extension
The MAQS Visual Studio Extension contains a collection of templates for NUnit, VSTest, Test Classes, and Page Models.

## MAQS NuGet Package
In the package manage console write:
```
install-package Magenic.Open.Maqs
```
Or install with the NuGet package manager:
```
nuget.exe install Magenic.Open.Maqs 
```

## Adding StyleCop to Visual Studio
StyleCop is advised to keep code consistent between multiple contributors, any branches will fail to merge if StyleCop throws an error on the gated build.

1. Open Package Manager Console (PMC) and check that Package Source drop down is set to All.
   PMC can be found at Tools > NuGet Package Manager > Package Manager Console
   the PMC window looks like a terminal window inside Visual Studio
2. Open a Solution file (.sln) from your Source Control Explorer
   A Solution Explorer window should open as well.
3. In the terminal like PMC window you should see PM>
   type
   ```
   install-package stylecop.msbuild
   ```
   and press Enter Key
4. PMC will post success or failure post install.
5. Common reason of a fail is likely due to zero solutions (.sln) being open.
   Avoid using -Version flag in PMC for stylecop
  (https://www.nuget.org/packages/StyleCop/) (https://github.com/StyleCop/StyleCop)

## Adding NUnit Extension
Templates for NUnit tests are included with MAQS, but the NUnit 3 Test Adapter extension is required to be installed before NUnit tests can be run.

1. Open Extensions and Updates under Tools in the toolbar.  
![Extensions and Updates](ReadMeScreenshots/NUnitSetup1.PNG)  

2. Search NUnit in Online  
![Download NUnit](ReadMeScreenshots/NUnitSetup2.PNG)  
3. Download NUnit 3 Test Adapter

## Building Your First Project
After the MAQS Visual Studio Extension and NuGet package have been installed, you're ready to start testing.

1. Go To File > New > Project
2. Under Templates (in the side panel)
3. Magenic's Open Test

There are two options, MAQS Framework - Selenium , that will start a project with tests run in VSTest, or MAQS Framework - NUnit - Selenium.  The only difference is the test adapter used with the included test templates.

Finally name the solution whatever you'd like to name, and click "ok".

For next steps, check our [Getting Started guide](Getting Started.MD).