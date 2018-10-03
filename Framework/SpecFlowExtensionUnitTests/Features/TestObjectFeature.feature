Feature: TestObjectFeature
	TestObject is available

@MAQS_General
Scenario: TestObject in BaseTestSteps
	Given class BaseTestSteps
	Then BaseTestSteps TestObject is not null
	And TestObject is type BaseTestSteps

@MAQS_Selenium
Scenario: TestObject in BaseSeleniumTestSteps
	Given class BaseSeleniumTestSteps
	Then BaseSeleniumTestSteps TestObject is not null
	And TestObject is type SeleniumTestObject

@MAQS_WebService
Scenario: TestObject in BaseWebServiceTestSteps
	Given class BaseWebServiceTestSteps
	Then BaseWebServiceTestSteps TestObject is not null
	And TestObject is type WebServiceTestObject

@MAQS_Email
Scenario: TestObject in BaseEmailTestSteps
	Given class BaseEmailTestSteps
	Then BaseEmailTestSteps TestObject is not null
	And TestObject is type EmailTestObject

@MAQS_Database
Scenario: TestObject in BaseDatabaseTestSteps
	Given class BaseDatabaseTestSteps
	Then BaseDatabaseTestSteps TestObject is not null
	And TestObject is type DatabaseTestObject