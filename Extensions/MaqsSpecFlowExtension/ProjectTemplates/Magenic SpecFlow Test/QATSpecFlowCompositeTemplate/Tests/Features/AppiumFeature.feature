# This tags scopes the feature so that only BaseAppiumTestSteps classes are instantiated when any scenario in this feature is run
@MAQS_Appium

Feature: AppiumFeature
	In order to do something
	As a tester
	I want to do stuff

Scenario: AppiumFeatureScenario
	Given I launch the app
	When I login as a valid user 
	Then the homepage is loaded 
