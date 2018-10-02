Feature: SeleniumFeature
	Selenium Tests

@MAQS_Selenium
Scenario: Driver in BaseSeleniumTestSteps
	Given class BaseSeleniumTestSteps
	Then BaseSeleniumTestSteps WebDriver is not null
	And WebDriver is type EventFiringWebDriver