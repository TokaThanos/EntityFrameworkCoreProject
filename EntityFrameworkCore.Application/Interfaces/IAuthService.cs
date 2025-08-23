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
        Task<Guid?> RegisterAsync(UserRequestDto request);
        Task<LoginResponseDto?> LoginAsync(UserRequestDto request);
        Task<TokenResponseDto?> TokenRefreshAsync(RefreshTokenRequestDto request);
    }
}
