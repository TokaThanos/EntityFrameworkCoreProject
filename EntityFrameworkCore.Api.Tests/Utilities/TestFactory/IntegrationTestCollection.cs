
namespace EntityFrameworkCore.Api.Tests.Utilities.TestFactory
{
    [CollectionDefinition("IntegrationTestCollection")]
    public class IntegrationTestCollection : ICollectionFixture<DockerWebApplicationTestFactory>
    {
    }
}
