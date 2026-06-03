using Microsoft.AspNetCore.Mvc;
using ReusePlusApi.Data;
using ReusePlusApi.Models;

namespace ReusePlusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovimentacaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Registrar([FromBody] Movimentacao mov)
        {
            _context.Movimentacoes.Add(mov);
            _context.SaveChanges();
            return Ok(mov);
        }
    }
}
