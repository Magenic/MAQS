Feature: WebServiceFeature
	Web Service Tests

@MAQS_WebService
Scenario: WebServiceWrapper Available BaseWebServiceTestSteps
	Given class BaseWebServiceTestSteps
	Then BaseWebServiceTestSteps WebServiceWrapper is not null
	And BaseWebServiceTestSteps WebServiceWrapper is type EventFiringWebServiceDriver
