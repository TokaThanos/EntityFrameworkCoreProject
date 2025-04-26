using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.Data.Configurations;
internal class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasData(
            new Team { Id = 1, Name = "Manchester United", CreatedDate = DateTimeOffset.UtcNow.DateTime },
            new Team { Id = 2, Name = "F.C. Barcelona", CreatedDate = DateTimeOffset.UtcNow.DateTime },
            new Team { Id = 3, Name = "Juventus", CreatedDate = DateTimeOffset.UtcNow.DateTime }
        );
    }
}