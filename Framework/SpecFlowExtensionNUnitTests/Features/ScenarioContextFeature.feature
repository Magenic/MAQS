Feature: ScenarioContextFeature
	ScenarioContext tests

@MAQS_General
Scenario: ScenarioContext Available BaseTestSteps
	Given class BaseTestSteps
	Then BaseTestSteps ScenarioContext is not null
	And ScenarioContext is type ScenarioContext

@MAQS_Selenium
Scenario: ScenarioContext Available BaseSeleniumTestSteps
	Given class BaseSeleniumTestSteps
	Then BaseSeleniumTestSteps ScenarioContext is not null
	And BaseSeleniumTestSteps ScenarioContext is type ScenarioContext

@MAQS_WebService
Scenario: ScenarioContext Available BaseWebServiceTestSteps
	Given class BaseWebServiceTestSteps
	Then BaseWebServiceTestSteps ScenarioContext is not null
	And BaseWebServiceTestSteps ScenarioContext is type ScenarioContext

@MAQS_Email
Scenario: ScenarioContext Available BaseEmailTestSteps
	Given class BaseEmailTestSteps
	Then BaseEmailTestSteps ScenarioContext is not null
	And BaseEmailTestSteps ScenarioContext is type ScenarioContext

@MAQS_Database
Scenario: ScenarioContext Available BaseDatabaseTestSteps
	Given class BaseDatabaseTestSteps
	Then BaseDatabaseTestSteps ScenarioContext is not null
	And BaseDatabaseTestSteps ScenarioContext is type ScenarioContext