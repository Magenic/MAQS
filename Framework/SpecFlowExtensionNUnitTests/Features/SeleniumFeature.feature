Feature: SeleniumFeature
	Selenium Tests

@MAQS_Selenium
Scenario: WebDriver Available BaseSeleniumTestSteps
	Given class BaseSeleniumTestSteps
	Then BaseSeleniumTestSteps WebDriver is not null
	And WebDriver is type EventFiringWebDriver