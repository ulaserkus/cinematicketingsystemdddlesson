namespace CinemaTicketingSystem.Application.Test.core;

[CollectionDefinition(Name)]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestWebAppFactory>
{
    public const string Name = "Integration";
}
