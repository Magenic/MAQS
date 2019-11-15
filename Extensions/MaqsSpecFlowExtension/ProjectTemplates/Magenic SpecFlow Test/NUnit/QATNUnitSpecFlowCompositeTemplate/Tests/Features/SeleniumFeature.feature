﻿# This tags scopes the feature so that only BaseSeleniumTestSteps classes are instantiated when any scenario in this feature is run
@MAQS_Selenium

Feature: SeleniumFeature
	In order to do something
	As a tester
	I want to do stuff

# This tags scopes the Scenario so that only BaseSeleniumTestSteps classes are instantiated when this scenario is run
@MAQS_Selenium
Scenario: SeleniumFeatureScenario
	Given I login as the 'standard' user
	Then The home page is loaded
