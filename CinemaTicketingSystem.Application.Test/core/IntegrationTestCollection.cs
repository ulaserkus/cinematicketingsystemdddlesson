namespace CinemaTicketingSystem.Application.Test;

[CollectionDefinition(Name)]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestWebAppFactory>
{
    public const string Name = "Integration";
}
