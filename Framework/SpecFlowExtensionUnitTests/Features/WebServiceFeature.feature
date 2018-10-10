Feature: WebServiceFeature
	Web Service Tests

@MAQS_WebService
Scenario: Driver in BaseWebServiceTestSteps
	Given class BaseWebServiceTestSteps
	Then BaseWebServiceTestSteps WebServiceDriver is not null
	And BaseWebServiceTestSteps WebServiceDriver is type EventFiringWebServiceDriver
