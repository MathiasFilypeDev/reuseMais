using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using ReusePlusApi.Models;
using ReusePlusApi.Data;

namespace ReusePlusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RelatorioController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public IActionResult Get(string? tipo)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            var query = _context.Movimentacoes.AsQueryable();

            if (!string.IsNullOrEmpty(tipo))
                query = query.Where(m => m.Tipo == tipo);

            return Ok(query.ToList());
        }
    }
}
