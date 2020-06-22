Feature: EmailFeature
	Email Tests

@MAQS_Email
Scenario: EmailDriver Available BaseEmailTestSteps
	Given class BaseEmailTestSteps
	Then BaseEmailTestSteps TestObject is not null
	And TestObject is type EmailTestObject
	And BaseEmailTestSteps ScenarioContext is not null
