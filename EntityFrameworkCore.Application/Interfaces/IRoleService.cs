using EntityFrameworkCore.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Interfaces
{
    public interface IRoleService
    {
        Task<string?> RegisterRoleAsync(string username, string roleName);
        Task<string?> RemoveRoleAsync(string username, string roleName);
        Task<RoleResponseDto?> GetRolesAsync(string username);
    }
}
