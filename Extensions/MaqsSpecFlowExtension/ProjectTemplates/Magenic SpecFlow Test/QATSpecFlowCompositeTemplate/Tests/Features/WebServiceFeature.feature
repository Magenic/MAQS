# This tags scopes the feature so that only BaseWebServiceTestSteps classes are instantiated when any scenario in this feature is run
@MAQS_WebService

Feature: WebServiceFeature
	In order to do something
	As a tester
	I want to do stuff

# This tags scopes the Scenario so that only BaseWebServiceTestSteps classes are instantiated when this scenario is run
Scenario: WebServiceXmlFeatureScenario
	Given I get product XML 1
	Then The result ID is 1

Scenario: WebServiceJsonFeatureScenario
	Given I get product JSON 2
	Then The result ID is 2
