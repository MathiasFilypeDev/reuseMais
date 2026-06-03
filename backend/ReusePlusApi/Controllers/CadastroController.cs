using Microsoft.AspNetCore.Mvc;
using ReusePlusApi.Data;
using ReusePlusApi.Models;

namespace ReusePlusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CadastroController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CadastroController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CriarUsuario([FromBody] User user)
        {
            user.Senha = BCrypt.Net.BCrypt.HashPassword(user.Senha);
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }
    }
}
