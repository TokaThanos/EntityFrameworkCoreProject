using EntityFrameworkCore.Api.Tests.Utilities.TestServices;
using EntityFrameworkCore.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace EntityFrameworkCore.Api.Tests.Utilities.TestFactory
{
    public class DockerWebApplicationTestFactory : WebApplicationFactory<EntityFrameworkCore.Api.Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer;

        public DockerWebApplicationTestFactory()
        {
            var password = Environment.GetEnvironmentVariable("POSTGRES_TEST_PASSWORD");
            _dbContainer = new PostgreSqlBuilder()
                .WithImage("postgres:15-alpine")
                .WithDatabase("FootballLeague_EfCore_Test")
                .WithUsername("postgres")
                .WithPassword(password)
                // .WithPassword("YourStrong!Passw0rd")
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var connectionString = _dbContainer.GetConnectionString();
            base.ConfigureWebHost(builder);
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<FootballLeagueDbContext>));
                services.AddDbContext<FootballLeagueDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                });

                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, FakeJwtAuthHandler>("Test", options => { });
            });
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            await using (var scope = Services.CreateAsyncScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<FootballLeagueDbContext>();
                await dbContext.Database.MigrateAsync();
            }
        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
        }
    }
}
