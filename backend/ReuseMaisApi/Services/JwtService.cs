using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ReuseMaisApi.Services
{
    public class JwtService
    {
        private readonly string _jwtSecret = "sua-chave-super-secreta-com-mais-de-32-caracteres-aqui!";

        public string GenerateToken(int userId, string nome, string email, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("userId", userId.ToString()),
                new Claim("nome", nome),
                new Claim("email", email)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}