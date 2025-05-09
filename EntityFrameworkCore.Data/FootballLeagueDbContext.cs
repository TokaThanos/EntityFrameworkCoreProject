using Microsoft.EntityFrameworkCore;
using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Reflection;
// using EntityFrameworkCore.Data.Configurations;

namespace EntityFrameworkCore.Data;

public class FootballLeagueDbContext : DbContext
{
    public DbSet<Team> Teams { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<League> Leagues { get; set; }
    public DbSet<Match> Matches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
        optionsBuilder.UseSqlServer($"Server=localhost,1433;Database=FootballLeague_EfCore;User Id=sa;Password={password};TrustServerCertificate=True;")
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
        optionsBuilder.ConfigureWarnings(warings =>
            warings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.ApplyConfiguration(new TeamConfiguration);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}