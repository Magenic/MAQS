# This tags scopes the feature so that only BaseEmailTestSteps classes are instantiated when any scenario in this feature is run
@MAQS_Email

Feature: EmailFeature
	In order to do something
	As a tester
	I want to do stuff

# This tags scopes the Scenario so that only BaseEmailTestSteps classes are instantiated when this scenario is run
@MAQS_Email
Scenario: EmailFeatureScenario
	Given condition
	When action
	Then verification
