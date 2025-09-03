using Microsoft.EntityFrameworkCore;
using System.Reflection;
using EntityFrameworkCore.Domain.Entities;

namespace EntityFrameworkCore.Data;

public class FootballLeagueDbContext : DbContext
{
    public FootballLeagueDbContext() { }

    public FootballLeagueDbContext(DbContextOptions<FootballLeagueDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<League> Leagues { get; set; }
    public DbSet<Match> Matches { get; set; }
    //public DbSet<TeamsAndLeaguesView> TeamsAndLeaguesView { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        // modelBuilder.ApplyConfiguration(new TeamConfiguration);
        //modelBuilder.Entity<TeamsAndLeaguesView>().HasNoKey().ToView("vw_TeamsAndLeagues");
        //modelBuilder
        //    .HasDbFunction(typeof(UserDefinedFunctions)
        //    .GetMethod(nameof(UserDefinedFunctions.GetCoachNameByTeamId))!)
        //    .HasName("fn_GetCoachNameByTeamId");
    }
}