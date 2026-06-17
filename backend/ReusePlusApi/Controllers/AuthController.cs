using Microsoft.AspNetCore.Mvc;
using ReusePlusApi.Models;

namespace ReusePlusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private static readonly List<User> _users = new()
        {
            new User { Id = 1, Username = "admin", Password = "1234", Role = "admin" }
        };

        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            var user = _users.FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);
            if (user is null) return Unauthorized(new { message = "Usuário ou senha inválidos" });

            return Ok(new { message = "Login realizado com sucesso", user });
        }
    }

    internal class User
    {
        public int Id { get; internal set; }
        public required string Username { get; internal set; }
        public required string Password { get; internal set; }
        public required string Role { get; internal set; }
    }
}
