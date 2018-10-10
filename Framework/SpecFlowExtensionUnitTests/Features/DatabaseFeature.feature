Feature: DatabaseFeature
	Database Tests

@MAQS_Database
Scenario: Driver in BaseDatabaseTestSteps
	Given class BaseDatabaseTestSteps
	Then DatabaseDriver is not null
	And DatabaseDriver is type DatabaseConnectionDriver
