# This tags scopes the feature so that only BaseAppiumTestSteps classes are instantiated when any scenario in this feature is run
@MAQS_Appium

Feature: AppiumFeature
	In order to do something
	As a tester
	I want to do stuff

# This tags scopes the Scenario so that only BaseAppiumTestSteps classes are instantiated when this scenario is run
@MAQS_Appium
Scenario: AppiumFeatureScenario
	Given condition
	When action
	Then verification
