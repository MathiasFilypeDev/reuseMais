using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReusePlusApi.Data;
using ReusePlusApi.Models;

namespace ReusePlusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
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
            if (!SenhaValidator.ValidarSenha(user.Senha))
            {
                return BadRequest("A senha deve conter pelo menos uma letra maiúscula, uma minúscula, um número e ter no mínimo 6 caracteres.");
            }
            return Ok(user);
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
