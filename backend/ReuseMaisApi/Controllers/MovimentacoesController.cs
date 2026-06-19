using Microsoft.AspNetCore.Mvc;
using ReuseMaisApi.Models;

namespace ReuseMaisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentacoesController : ControllerBase
    {
        private static List<Movimentacao> movimentacoes = new List<Movimentacao>();

        [HttpGet]
        public IActionResult GetMovimentacoes() => Ok(movimentacoes);

        [HttpPost]
        public IActionResult AddMovimentacao(Movimentacao mov)
        {
            mov.Id = movimentacoes.Count + 1;
            movimentacoes.Add(mov);
            return Ok(mov);
        }
    }
}
