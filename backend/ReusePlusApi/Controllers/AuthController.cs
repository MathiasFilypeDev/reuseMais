
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
        public static List<User> Users = new()
        {
            new User
            {
                Id = 1,
                Username = "admin",
                Password = "1234",
                Role = "admin"
            }
        };

        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            var user = Users.FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);
            if (user is null)
                return Unauthorized(new { message = "Usuário ou senha inválidos" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("sua_chave_secreta_super_segura");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return Ok(new { message = "Login realizado com sucesso", token = jwt });
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

        [HttpGet("users")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(Users);
        }
    }
}
