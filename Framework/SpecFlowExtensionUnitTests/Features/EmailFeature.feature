Feature: EmailFeature
	Email Tests

@MAQS_Email
Scenario: EmailWrapper Available BaseEmailTestSteps
	Given class BaseEmailTestSteps
	Then BaseEmailTestSteps EmailWrapper is not null
	And EmailWrapper is type EventFiringEmailConnectionWrapper
