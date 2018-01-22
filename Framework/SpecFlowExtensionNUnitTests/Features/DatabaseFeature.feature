Feature: DatabaseFeature
	Database Tests

@MAQS_Database
Scenario: DatabaseWrapper Available BaseDatabaseTestSteps
	Given class BaseDatabaseTestSteps
	Then DatabaseWrapper is not null
	And DatabaseWrapper is type DatabaseConnectionWrapper
