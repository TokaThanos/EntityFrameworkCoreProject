using EntityFrameworkCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.Data.Configurations
{
    internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

            builder.HasKey(userRole => new { userRole.UserId, userRole.RoleId });

            // A UserRole record is connected to one specific user
            // and that user can have many UserRole entries.
            builder.HasOne(userRole => userRole.User)
                .WithMany(user => user.UserRoles)
                .HasForeignKey(userRole => userRole.UserId);

            // A UserRole record is connected to one specific role
            // and that role could be linked to many UserRole entries.
            builder.HasOne(userRole => userRole.Role)
                .WithMany() // This is empty because UserRole collection is not stored in Role class.
                .HasForeignKey(userRole => userRole.RoleId);
        }
    }
}
