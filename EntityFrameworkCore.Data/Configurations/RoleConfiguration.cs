using EntityFrameworkCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.Data.Configurations
{
    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "admin" },
                new Role { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "user" },
                new Role { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "mod" }
            );
        }
    }
}
