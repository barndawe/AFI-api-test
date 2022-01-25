# AFI-api-test

##Introduction
This API has been written as a response to a coding test specification.
The methodologies and frameworks chosen are (intentionally) overkill for an API with a single endpoint, but were chosen to show understanding of the following:

- .NET 6
- Domain-driven design
- Asynchronous API
- OpenAPI (Swagger) documentation
- Validation (entity and API call)
- Entity Framework Core (Code-First)
- Mediator pattern
- Integration and unit testing
- IoC/DI

This was written using the Rider IDE, but has also been tested in Visual Studio 2022 to ensure the tests pass. Storage is provided by Windows LocalDB.

Further improvements that could be made to make this API 'production ready':~~~~

- Adding authentication - perhaps using OAuth2 and a suitable provider
- Adding a permission model and restricting access to the endpoint
- Adding a GET endpoint for the Customer model
- Adding logging
- Adding an events model to the domain object(s) to allow for alerting other services to (for now just) the creation of a Customer 
- Adding more integration tests for all the above!
