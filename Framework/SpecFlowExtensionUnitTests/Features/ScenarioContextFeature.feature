Feature: ScenarioContextFeature
	ScenarioContext tests

@MAQS_General
Scenario: Context in BaseTestSteps
	Given class BaseTestSteps
	Then BaseTestSteps ScenarioContext is not null
	And ScenarioContext is type ScenarioContext

@MAQS_Selenium
Scenario: Context in BaseSeleniumTestSteps
	Given class BaseSeleniumTestSteps
	Then BaseSeleniumTestSteps ScenarioContext is not null
	And BaseSeleniumTestSteps ScenarioContext is type ScenarioContext

@MAQS_WebService
Scenario: Context in BaseWebServiceTestSteps
	Given class BaseWebServiceTestSteps
	Then BaseWebServiceTestSteps ScenarioContext is not null
	And BaseWebServiceTestSteps ScenarioContext is type ScenarioContext

@MAQS_Email
Scenario: Context in BaseEmailTestSteps
	Given class BaseEmailTestSteps
	Then BaseEmailTestSteps ScenarioContext is not null
	And BaseEmailTestSteps ScenarioContext is type ScenarioContext

@MAQS_Database
Scenario: Context in BaseDatabaseTestSteps
	Given class BaseDatabaseTestSteps
	Then BaseDatabaseTestSteps ScenarioContext is not null
	And BaseDatabaseTestSteps ScenarioContext is type ScenarioContext