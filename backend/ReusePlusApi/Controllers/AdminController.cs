using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReusePlusApi.Models;

namespace ReusePlusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")] // só quem tem role "admin" acessa
    public class AdminController : ControllerBase
    {
        private static readonly List<User> _users = new()
        {
            new User { Id = 1, Username = "admin", Password = "1234", Role = "admin" }
        };

        [HttpGet("users")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(_users);
        }

        [HttpDelete("users/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user is null) return NotFound();

            _users.Remove(user);
            return NoContent();
        }
    }
}

