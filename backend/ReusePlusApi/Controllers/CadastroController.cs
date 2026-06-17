using Microsoft.AspNetCore.Mvc;
using ReusePlusApi.Models;

namespace ReusePlusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CadastroController : ControllerBase
    {
        private static readonly List<User> _users = new()
        {
            new User { Id = 1, Username = "admin", Password = "1234", Role = "admin" }
        };

        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            if (_users.Any(u => u.Username == user.Username))
                return BadRequest(new { message = "Usuário já existe" });

            user.Id = _users.Count + 1;
            user.Role = "user";
            _users.Add(user);

            return Ok(new { message = "Usuário cadastrado com sucesso", user });
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return Ok(_users);
        }
    }
}
