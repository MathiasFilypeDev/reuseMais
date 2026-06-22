using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using ReuseMaisApi.Models;

namespace ReuseMaisApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public UsersController(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("login")]
        public ActionResult<LoginResponseDto> Login([FromBody] LoginRequestDto request)
    {
        // Validação
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))  // ✅ SIMPLIFICADO
        {
            return BadRequest(new { message = "Usuário e senha são obrigatórios." });
        }

        try
        {
            // Buscar usuário no banco de dados
            var user = _userService.GetUserByUsername(request.Username);  // ✅ USE request.Username direto

            if (user == null)
            {
                return Unauthorized(new { message = "Usuário ou senha inválidos." });
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash ?? ""))
            {
                return Unauthorized(new { message = "Usuário ou senha inválidos." });
            }

            var token = GenerateJwtToken(user);

            return Ok(new LoginResponseDto
            {
                Id = user.Id,
                Nome = user.Nome ?? "",
                Email = user.Email ?? "",
                Token = token,
                Role = user.Role ?? "user"
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro no login: {ex.Message}");
            return StatusCode(500, new { message = "Erro no servidor. Tente novamente mais tarde." });
        }
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSecret = _configuration["Jwt:Secret"];
        var jwtIssuer = _configuration["Jwt:Issuer"];
        var jwtAudience = _configuration["Jwt:Audience"];

        if (string.IsNullOrEmpty(jwtSecret))
        {
            throw new InvalidOperationException("Jwt:Secret não configurado");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Nome ?? ""),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim("role", user.Role ?? "user")
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

    public class UserService
    {
        internal User GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }
    }
}