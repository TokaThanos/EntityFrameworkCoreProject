using EntityFrameworkCore.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;

namespace EntityFrameworkCore.Api.Tests.Utilities.Fixture
{
    public class DockerWebApplicationTestFactoryFixture : WebApplicationFactory<EntityFrameworkCore.Api.Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _dbContainer;
        private string _connectionString = string.Empty;

        public DockerWebApplicationTestFactoryFixture()
        {
            _dbContainer = new MsSqlBuilder().Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            _connectionString = _dbContainer.GetConnectionString();
            base.ConfigureWebHost(builder);
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<FootballLeagueDbContext>));
                services.AddDbContext<FootballLeagueDbContext>(options =>
                {
                    options.UseSqlServer(_connectionString);
                });
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
