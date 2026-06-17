using Microsoft.AspNetCore.Mvc;
using ReusePlusApi.Models;

namespace ReusePlusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public static List<User> Users = new()
        {
            new User
            {
                Id = 1,
                Username = "Admin",
                Password = "reusemaisadmin",
                Role = "admin"
            }
        };

        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            if (login.Username == "Admin" && login.Password == "reusemaisadmin")
            {
                var token = GenerateJwtToken("Admin", "admin");
                return Ok(new { message = "Login de administrador realizado com sucesso", token });
            }

            // Login para usuários cadastrados
            var user = Users.FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);
            if (user is null)
                return Unauthorized(new { message = "Usuário ou senha inválidos" });

            var userToken = GenerateJwtToken(user.Username, user.Role);
            return Ok(new { message = "Login realizado com sucesso", token = userToken });
        }

        private object GenerateJwtToken(string username, string role)
        {
            throw new NotImplementedException();
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User newUser)
        {
            if (Users.Any(u => u.Username == newUser.Username))
                return BadRequest(new { message = "Usuário já existe" });

            newUser.Id = Users.Count + 1;
            newUser.Role = "user";
            Users.Add(newUser);

            return Ok(new { message = "Usuário registrado com sucesso", user = newUser });
        }
    }
}
