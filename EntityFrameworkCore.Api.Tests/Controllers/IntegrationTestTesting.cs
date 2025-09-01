using EntityFrameworkCore.Api.Tests.Utilities.Fixture;

namespace EntityFrameworkCore.Api.Tests.Controllers
{
    public class IntegrationTestTesting : IClassFixture<DockerWebApplicationTestFactoryFixture>
    {
        private readonly DockerWebApplicationTestFactoryFixture _factory;

        public IntegrationTestTesting(DockerWebApplicationTestFactoryFixture factory)
        {
            _factory = factory;
        }

        [Fact]
        public void Test1()
        {
            // Arrange
            // Act
            // Assert
        }
    }
}
