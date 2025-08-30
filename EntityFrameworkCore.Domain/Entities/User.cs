using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserNameNormalized { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
