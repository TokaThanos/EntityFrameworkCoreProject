using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.Data.Configurations;
internal class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasMany(t => t.HomeMatches)
            .WithOne(m => m.HomeTeam)
            .HasForeignKey(m => m.HomeTeamId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.AwayMatches)
            .WithOne(m=> m.AwayTeam)
            .HasForeignKey(m => m.AwayTeamId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasData(
            new Team { Id = 101, Name = "Manchester United", CreatedDate = new DateTime(2000, 01, 23), CoachId = 1, LeagueId = 3 },
            new Team { Id = 102, Name = "F.C. Barcelona", CreatedDate = new DateTime(2000, 01, 23), CoachId = 3, LeagueId = 1 },
            new Team { Id = 103, Name = "Real Madrid", CreatedDate = new DateTime(2000, 01, 23), CoachId = 2, LeagueId = 1 }
        );
    }
}