# This tags scopes the feature so that only BaseDatabaseTestSteps classes are instantiated when any scenario in this feature is run
@MAQS_Database

Feature: DatabaseFeature
	In order to do something
	As a tester
	I want to do stuff

# This tags scopes the Scenario so that only BaseDatabaseTestSteps classes are instantiated when this scenario is run
@MAQS_Database
Scenario: DatabaseFeatureScenario
	Given condition
	When action
	Then verification
