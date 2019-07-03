# This tags scopes the feature so that only BaseSeleniumTestSteps classes are instantiated when any scenario in this feature is run
@MAQS_Selenium

Feature: Selenium
	In order to do something
	As a tester
	I want to do stuff

Scenario: Valid login
	Given I login with a valid user
	Then I am logged into the home page

Scenario: Invalid login
	Given I login with a invalid user
	Then An error message is displayed