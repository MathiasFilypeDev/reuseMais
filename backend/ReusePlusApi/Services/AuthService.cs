using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReusePlusApi.Constants;
using ReusePlusApi.DTOs;
using ReusePlusApi.Models;
using ReusePlusApi.Repositories;

namespace ReusePlusApi.Services
{
    /// <summary>
    /// Interface para serviço de autenticação
    /// </summary>
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
        Task<SuccessResponseDto> RegisterUserAsync(RegisterUserRequestDto request);
        Task<SuccessResponseDto> RegisterAdminAsync(RegisterAdminRequestDto request);
        string GenerateToken(IEnumerable<Claim> claims);
    }

    /// <summary>
    /// Implementação do serviço de autenticação
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            if (request.Tipo == UserType.Admin)
            {
                return await ValidateAdminLoginAsync(request);
            }

            return await ValidateUserLoginAsync(request);
        }

        public async Task<SuccessResponseDto> RegisterUserAsync(RegisterUserRequestDto request)
        {
            if (await _userRepository.EmailExistsAsync(request.Email))
            {
                throw new InvalidOperationException(ErrorMessages.EmailAlreadyRegistered);
            }

            var user = new User
            {
                Nome = request.Nome,
                Email = request.Email,
                Senha = BCrypt.Net.BCrypt.HashPassword(request.Senha),
                Tipo = UserType.Usuario,
                DataCadastro = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            return new SuccessResponseDto
            {
                Success = true,
                Message = SuccessMessages.UserRegistered,
                Data = new { userId = user.Id, email = user.Email }
            };
        }

        public async Task<SuccessResponseDto> RegisterAdminAsync(RegisterAdminRequestDto request)
        {
            var adminSecret = _configuration["AdminSecret"];
            if (request.SecretKey != adminSecret)
            {
                throw new UnauthorizedAccessException(ErrorMessages.InvalidSecretKey);
            }

            if (await _userRepository.EmailExistsAsync(request.Email))
            {
                throw new InvalidOperationException(ErrorMessages.EmailAlreadyRegistered);
            }

            var admin = new User
            {
                Nome = request.Nome,
                Email = request.Email,
                Senha = BCrypt.Net.BCrypt.HashPassword(request.Senha),
                Tipo = UserType.Admin,
                DataCadastro = DateTime.UtcNow
            };

            await _userRepository.AddAsync(admin);

            return new SuccessResponseDto
            {
                Success = true,
                Message = SuccessMessages.AdminRegistered,
                Data = new { adminId = admin.Id, email = admin.Email }
            };
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(AuthConstants.TokenExpirationHours),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<LoginResponseDto?> ValidateAdminLoginAsync(LoginRequestDto request)
        {
            var adminUser = _configuration["AdminCredentials:User"];
            var adminPass = _configuration["AdminCredentials:Password"];

            if (!request.Email.Equals(adminUser, StringComparison.OrdinalIgnoreCase) || request.Senha != adminPass)
            {
                return null;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, adminUser!),
                new Claim(ClaimTypes.Role, UserType.Admin)
            };

            var token = GenerateToken(claims);

            return new LoginResponseDto
            {
                Token = token,
                Role = UserType.Admin,
                Email = adminUser!
            };
        }

        private async Task<LoginResponseDto?> ValidateUserLoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Senha, user.Senha))
            {
                return null;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Tipo),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = GenerateToken(claims);

            return new LoginResponseDto
            {
                Token = token,
                Role = user.Tipo,
                Email = user.Email
            };
        }
    }
}
