# This tags scopes the feature so that only BaseTestSteps classes are instantiated when any scenario in this feature is run
@MAQS_General

Feature: GeneralFeature
	In order to do something
	As a tester
	I want to do stuff

# This tags scopes the Scenario so that only BaseTestSteps classes are instantiated when this scenario is run
@MAQS_General
Scenario: GeneralFeatureScenario
	Given condition
	When action
	Then verification
