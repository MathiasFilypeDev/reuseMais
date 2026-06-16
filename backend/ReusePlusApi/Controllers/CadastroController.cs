using System.Diagnostics;
using System.Linq;
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
            if (user == null || string.IsNullOrWhiteSpace(user.Senha))
            {
                return BadRequest("Usuário ou senha inválidos.");
            }

            if (!ValidarSenha(user.Senha))
            {
                return BadRequest("A senha deve conter pelo menos uma letra maiúscula, uma minúscula, um número e ter no mínimo 6 caracteres.");
            }

            user.Senha = BCrypt.Net.BCrypt.HashPassword(user.Senha);
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }

        private bool ValidarSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
                return false;

            return senha.Any(char.IsUpper)
                && senha.Any(char.IsLower)
                && senha.Any(char.IsDigit)
                && senha.Length >= 6;
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
