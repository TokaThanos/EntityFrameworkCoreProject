using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly FootballLeagueDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(FootballLeagueDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<TokenResponseDto?> LoginAsync(UserRequestDto request)
        {
            var normalizedUserName = request.UserName.ToLowerInvariant();
            var user = await _context.Users
                .Include(user => user.UserRoles)
                .ThenInclude(userRole => userRole.Role)
                .FirstOrDefaultAsync(user => user.UserNameNormalized == normalizedUserName);
            if (user is null)
            {
                return null;
            }
            if (new PasswordHasher<User>()
                .VerifyHashedPassword(user, user.PasswordHash, request.Password)
                == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var response = new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user),
            };

            return response;
        }

        public async Task<UserResponseDto?> RegisterAsync(UserRequestDto request)
        {
            var normalizedUserName = request.UserName.ToLowerInvariant();

            if (await _context.Users.AnyAsync(user => user.UserNameNormalized == normalizedUserName))
            {
                return null;
            }

            var user = new User
            {
                UserName = request.UserName,
                UserNameNormalized = normalizedUserName,
            };

            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password);
            user.PasswordHash = hashedPassword;

            var defaultRole = await _context.Roles.FirstOrDefaultAsync(role => role.Name == "user");
            if (defaultRole is not null)
            {
                user.UserRoles = new List<UserRole>
                {
                    new UserRole { RoleId = defaultRole.Id },
                };
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userInfo = new UserResponseDto
            {
                Id = user.Id,
                UserName = request.UserName
            };

            return userInfo;
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roles = user.UserRoles.Select(userRole => userRole.Role.Name);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                audience: _configuration.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(_configuration.GetValue<string>("Jwt:ExpireMinutes")!, CultureInfo.InvariantCulture)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
