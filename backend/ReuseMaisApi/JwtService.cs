using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReuseMaisApi.Services
{
    public class JwtServiceV2
    {
        private readonly string _secret;

        public JwtServiceV2(string secret)
        {
            _secret = secret;
        }

        // Gera token simples com username
        public string GenerateToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Gera token completo com id, nome, email e role
        public string GenerateToken(int id,
                                    string? nome,
                                    string? email,
                                    string? role,
                                    bool includeRole = true)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);

            var claims = new List<Claim>
            {
                new Claim("Id", id.ToString()),
                new Claim(ClaimTypes.Name, nome ?? string.Empty),
                new Claim(ClaimTypes.Email, email ?? string.Empty)
            };

            if (includeRole)
            {
                claims.Add(new Claim(ClaimTypes.Role, role ?? string.Empty));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
