using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDto?> RegisterAsync(UserRequestDto request);
        Task<string?> LoginAsync(UserRequestDto request);
    }
}
