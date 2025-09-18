using EntityFrameworkCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.Data.Configurations
{
    public class CoachConfiguration : IEntityTypeConfiguration<Coach>
    {
        public void Configure(EntityTypeBuilder<Coach> builder)
        {
            builder.HasData(
                new Coach { Id = 1, Name = "Ruben Amorim" },
                new Coach { Id = 2, Name = "Carlo Ancelotti" },
                new Coach { Id = 3, Name = "Hansi Flick" }
            );
        }
    }
}