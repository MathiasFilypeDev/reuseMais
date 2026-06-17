using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReusePlusApi.Models;

namespace ReusePlusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")] // só admin acessa relatórios
    public class RelatorioController : ControllerBase
    {
        private static readonly List<Movimentacao> _movimentacoes = new()
        {
            new Movimentacao { Id = 1, Tipo = "entrada", ItemId = 1, Quantidade = 10 },
            new Movimentacao { Id = 2, Tipo = "saida", ItemId = 1, Quantidade = 3 },
            new Movimentacao { Id = 3, Tipo = "entrada", ItemId = 2, Quantidade = 5 }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Movimentacao>> Consultar([FromQuery] string? tipo)
        {
            if (string.IsNullOrEmpty(tipo))
                return Ok(_movimentacoes);

            var filtrado = _movimentacoes
                .Where(m => m.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase));

            return Ok(filtrado);
        }

        [HttpGet("estatisticas")]
        public IActionResult Estatisticas()
        {
            var totalEntradas = _movimentacoes.Where(m => m.Tipo == "entrada").Sum(m => m.Quantidade);
            var totalSaidas = _movimentacoes.Where(m => m.Tipo == "saida").Sum(m => m.Quantidade);

            return Ok(new { totalEntradas, totalSaidas });
        }

        [HttpPost]
        public IActionResult Registrar([FromBody] Movimentacao mov)
        {
            mov.Id = _movimentacoes.Count + 1;
            _movimentacoes.Add(mov);
            return Ok(new { message = "Movimentação registrada com sucesso", mov });
        }
    }
}
