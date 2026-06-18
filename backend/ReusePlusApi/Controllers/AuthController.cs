using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReusePlusApi.Models;

namespace ReusePlusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ReusePlusContext _context;

        public AuthController(IConfiguration configuration, ReusePlusContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            if (login?.Username == null || login.Password == null)
                return BadRequest(new { message = "Username e password são obrigatórios" });

            var user = _context.Users.FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);
            if (user is null)
                return Unauthorized(new { message = "Usuário ou senha inválidos" });

            try
            {
                var token = GenerateJwtToken(user.Username, user.Role);
                return Ok(new { message = "Login realizado com sucesso", token, role = user.Role });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao gerar token JWT", error = ex.Message });
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User newUser)
        {
            if (newUser?.Username == null || newUser.Password == null)
                return BadRequest(new { message = "Username e password são obrigatórios" });

            if (_context.Users.Any(u => u.Username == newUser.Username))
                return BadRequest(new { message = "Usuário já existe" });

            newUser.Role = "user";
            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(new { message = "Usuário registrado com sucesso", user = new { newUser.Id, newUser.Username, newUser.Role } });
        }

        private string GenerateJwtToken(string username, string role)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
                throw new Exception("Configurações JWT incompletas");

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
