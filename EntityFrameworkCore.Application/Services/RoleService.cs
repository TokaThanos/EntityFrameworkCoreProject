using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly FootballLeagueDbContext _context;

        public RoleService(FootballLeagueDbContext context)
        {
            _context = context;
        }
        public async Task<RoleResponseDto?> GetRolesAsync(string username)
        {
            var user = await _context.Users
                .Include(user => user.UserRoles)
                .ThenInclude(userRole => userRole.Role)
                .FirstOrDefaultAsync(user => user.UserNameNormalized == username.ToLower());

            if (user is null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var response = new RoleResponseDto
            {
                UserName = user.UserName,
                Roles = user.UserRoles.Select(userRole => userRole.Role.Name).ToList()
            };

            return response;
        }

        public async Task<string?> RegisterRoleAsync(string username, string roleName)
        {
            var user = await _context.Users
                .Include(user => user.UserRoles)
                .FirstOrDefaultAsync(user => user.UserNameNormalized == username.ToLower());

            if (user is null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var role = await _context.Roles.FirstOrDefaultAsync(role => role.Name == roleName);

            if (role == null)
            {
                throw new KeyNotFoundException("Role not found.");
            }

            if (user.UserRoles.Any(userRole => userRole.RoleId == role.Id))
            {
                throw new ArgumentException("User already has this role.");
            }

            _context.Users.Attach(user);

            user.UserRoles.Add(new UserRole
            {
                RoleId = role.Id,
            });

            await _context.SaveChangesAsync();
            return $"Role {roleName} assigned to user {username}";
        }

        public async Task<string?> RemoveRoleAsync(string username, string roleName)
        {
            var user = await _context.Users
                .Include(user => user.UserRoles)
                .FirstOrDefaultAsync(user => user.UserNameNormalized == username.ToLower());

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var role = await _context.Roles.FirstOrDefaultAsync(role => role.Name == roleName);

            if (role == null)
            {
                throw new KeyNotFoundException("Role not found.");
            }

            var userRole = user.UserRoles.FirstOrDefault(userRole => userRole.RoleId == role.Id);
            if (userRole is null)
            {
                throw new ArgumentException("User does not have this role.");
            }

            _context.Users.Attach(user);

            user.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();

            return $"Role {roleName} removed from user {username}";
        }
    }
}
