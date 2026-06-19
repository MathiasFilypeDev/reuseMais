using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReuseMaisApi.Models;
using ReuseMaisApi.Services;  

namespace ReuseMaisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static List<User> users = new List<User>
        {
            new User { Id = 1, Nome = "João", Email = "joao@email.com", Senha = "123456" },
            new User { Id = 2, Nome = "Maria", Email = "maria@email.com", Senha = "123456" },
            new User { Id = 3, Nome = "Admin", Email = "admin@email.com", Senha = "admin123" }
        };

        private readonly JwtService _jwtService = new JwtService();

        [HttpGet]
        [Authorize]  // ← Protege a rota
        public IActionResult GetUsers()
        {
            return Ok(users.Select(u => new { u.Id, u.Nome, u.Email }));
        }

        // ✅ Login - gera JWT
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return BadRequest(new { message = "Username e password são obrigatórios" });

            var user = users.FirstOrDefault(u =>
                u.Nome == request.Username &&
                u.Senha == request.Password);

            if (user == null)
                return Unauthorized(new { message = "Usuário ou senha inválidos" });

            // ✅ Gera o JWT
            var token = _jwtService.GenerateToken(user.Id, user.Nome, user.Email);

            return Ok(new
            {
                id = user.Id,
                nome = user.Nome,
                email = user.Email,
                token = token  // ← Retorna o token
            });
        }
    }

    public class LoginRequest
    {
        public string? Username { get; set; }  // ← Adicione o ?
        public string? Password { get; set; }   // ← Adicione o ?
    }
}