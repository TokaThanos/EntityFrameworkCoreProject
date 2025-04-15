using Microsoft.EntityFrameworkCore;
using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCore.Data;

public class FootballLeagueDbContext : DbContext
{
    public DbSet<Team> Teams { get; set; }
    public DbSet<Coach> Coaches { get; set; }

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
        modelBuilder.Entity<Team>().HasData(
            new Team { Id = 1, Name = "Manchester United", CreatedDate = DateTimeOffset.UtcNow.DateTime },
            new Team { Id = 2, Name = "F.C. Barcelona", CreatedDate = DateTimeOffset.UtcNow.DateTime },
            new Team { Id = 3, Name = "Juventus", CreatedDate = DateTimeOffset.UtcNow.DateTime }
        );
    }
}