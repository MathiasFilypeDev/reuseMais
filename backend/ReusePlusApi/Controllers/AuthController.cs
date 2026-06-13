using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using ReusePlusApi.Models;
using ReusePlusApi.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ReusePlusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // Método auxiliar para gerar token
        private string GerarToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            var adminUser = _config["AdminCredentials:User"];
            var adminPass = _config["AdminCredentials:Password"];

            // Validação de Admin
            if (login.Tipo == "admin")
            {
                if (login.Email.Equals(adminUser, StringComparison.OrdinalIgnoreCase)
                    && login.Senha == adminPass)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, adminUser),
                        new Claim(ClaimTypes.Role, "admin")
                    };

                    var token = GerarToken(claims);

                    return Ok(new
                    {
                        token,
                        role = "admin"
                    });
                }
                return Unauthorized("Credenciais inválidas.");
            }

            // Validação de Usuário comum
            var user = _context.Users.FirstOrDefault(u => u.Email == login.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Senha, user.Senha))
                return Unauthorized("Credenciais inválidas.");

            var userClaims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Tipo),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var tokenUser = GerarToken(userClaims);

            return Ok(new
            {
                token = tokenUser,
                role = user.Tipo
            });
        }
    }
}
