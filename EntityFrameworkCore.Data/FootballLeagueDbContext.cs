using Microsoft.EntityFrameworkCore;
using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EntityFrameworkCore.Data;

public class FootballLeagueDbContext : DbContext
{
    public DbSet<Team> Teams { get; set; }
    public DbSet<Coach> Coaches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source=FootballLeague_EfCore.db");
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning)); // this line is added to suppress a bug present in ef core 9.0
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