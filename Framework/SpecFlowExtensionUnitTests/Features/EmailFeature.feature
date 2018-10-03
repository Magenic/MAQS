Feature: EmailFeature
	Email Tests

@MAQS_Email
Scenario: Driver in BaseEmailTestSteps
	Given class BaseEmailTestSteps
	Then BaseEmailTestSteps EmailDriver is not null
	And EmailDriver is type EventFiringEmailConnectionDriver
