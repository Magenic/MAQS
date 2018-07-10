Feature: EmailFeature
	Email Tests

@MAQS_Email
Scenario: EmailDriver Available BaseEmailTestSteps
	Given class BaseEmailTestSteps
	Then BaseEmailTestSteps EmailDriver is not null
	And EmailDriver is type EventFiringEmailConnectionDriver
