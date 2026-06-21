using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReuseMaisApi.Models;

namespace ReuseMaisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RelatoriosController : ControllerBase
    {
        // Dados simulados de logs
        private static List<LoginLog> loginLogs = new List<LoginLog>
        {
            new LoginLog { Id = 1, UserId = 1, NomeUsuario = "João", Role = "user", DataLogin = DateTime.Now.AddHours(-5) },
            new LoginLog { Id = 2, UserId = 2, NomeUsuario = "Maria", Role = "user", DataLogin = DateTime.Now.AddHours(-2) },
            new LoginLog { Id = 3, UserId = 3, NomeUsuario = "Admin", Role = "admin", DataLogin = DateTime.Now.AddMinutes(-30) }
        };

        // Dados simulados de movimentações
        private static List<Movimentacao> movimentacoes = new List<Movimentacao>
        {
            new Movimentacao { Id = 1, ProdutoId = 1, NomeProduto = "Camiseta", Categoria = "Roupas", Quantidade = 10, Tipo = "entrada", UsuarioId = 1, Data = DateTime.Now.AddDays(-1) },
            new Movimentacao { Id = 2, ProdutoId = 1, NomeProduto = "Camiseta", Categoria = "Roupas", Quantidade = 3, Tipo = "saida", UsuarioId = 2, Data = DateTime.Now.AddHours(-12) },
            new Movimentacao { Id = 3, ProdutoId = 2, NomeProduto = "Calça", Categoria = "Roupas", Quantidade = 5, Tipo = "entrada", UsuarioId = 1, Data = DateTime.Now.AddDays(-2) }
        };

        // ✅ GET - Total de usuários cadastrados
        [HttpGet("usuarios/total")]
        public IActionResult GetTotalUsuarios()
        {
            var totalUsuarios = loginLogs.Select(l => l.UserId).Distinct().Count();
            return Ok(new { total = totalUsuarios });
        }

        // ✅ GET - Logins por período
        [HttpGet("logins")]
        public IActionResult GetLogins([FromQuery] string periodo = "7d")
        {
            var dataInicio = ObterDataInicio(periodo);
            var logins = loginLogs.Where(l => l.DataLogin >= dataInicio).ToList();

            return Ok(new
            {
                periodo,
                total = logins.Count,
                logins = logins.OrderByDescending(l => l.DataLogin)
            });
        }

        // ✅ GET - Estatísticas de produtos por categoria
        [HttpGet("categorias/estatisticas")]
        public IActionResult GetEstatisticasCategorias([FromQuery] string periodo = "7d")
        {
            var dataInicio = ObterDataInicio(periodo);
            var movimentacoesPeriodo = movimentacoes.Where(m => m.Data >= dataInicio).ToList();

            var categorias = movimentacoesPeriodo
                .GroupBy(m => m.Categoria)
                .Select(g => new
                {
                    categoria = g.Key,
                    totalEntrada = g.Where(m => m.Tipo == "entrada").Sum(m => m.Quantidade),
                    totalSaida = g.Where(m => m.Tipo == "saida").Sum(m => m.Quantidade),
                    movimentacoes = g.Count()
                })
                .ToList();

            return Ok(new { periodo, categorias });
        }

        // ✅ GET - Produtos mais procurados (mais saídas)
        [HttpGet("produtos/mais-procurados")]
        public IActionResult GetProdutosMaisProcurados([FromQuery] string periodo = "7d")
        {
            var dataInicio = ObterDataInicio(periodo);
            var produtos = movimentacoes
                .Where(m => m.Data >= dataInicio && m.Tipo == "saida")
                .GroupBy(m => m.NomeProduto)
                .Select(g => new
                {
                    produto = g.Key,
                    categoria = g.First().Categoria,
                    totalSaidas = g.Sum(m => m.Quantidade),
                    movimentacoes = g.Count()
                })
                .OrderByDescending(p => p.totalSaidas)
                .Take(10)
                .ToList();

            return Ok(new { periodo, produtos });
        }

        // ✅ GET - Produtos mais cadastrados (mais entradas)
        [HttpGet("produtos/mais-cadastrados")]
        public IActionResult GetProdutosMaisCadastrados([FromQuery] string periodo = "7d")
        {
            var dataInicio = ObterDataInicio(periodo);
            var produtos = movimentacoes
                .Where(m => m.Data >= dataInicio && m.Tipo == "entrada")
                .GroupBy(m => m.NomeProduto)
                .Select(g => new
                {
                    produto = g.Key,
                    categoria = g.First().Categoria,
                    totalEntradas = g.Sum(m => m.Quantidade),
                    movimentacoes = g.Count()
                })
                .OrderByDescending(p => p.totalEntradas)
                .Take(10)
                .ToList();

            return Ok(new { periodo, produtos });
        }

        // ✅ GET - Resumo geral
        [HttpGet("resumo")]
        public IActionResult GetResumo([FromQuery] string periodo = "7d")
        {
            var dataInicio = ObterDataInicio(periodo);
            var movimentacoesPeriodo = movimentacoes.Where(m => m.Data >= dataInicio).ToList();
            var loginsPeriodo = loginLogs.Where(l => l.DataLogin >= dataInicio).ToList();

            var totalEntradas = movimentacoesPeriodo.Where(m => m.Tipo == "entrada").Sum(m => m.Quantidade);
            var totalSaidas = movimentacoesPeriodo.Where(m => m.Tipo == "saida").Sum(m => m.Quantidade);

            return Ok(new
            {
                periodo,
                totalUsuariosCadastrados = loginLogs.Select(l => l.UserId).Distinct().Count(),
                totalLogins = loginsPeriodo.Count,
                totalMovimentacoes = movimentacoesPeriodo.Count,
                totalEntradas,
                totalSaidas,
                saldo = totalEntradas - totalSaidas
            });
        }

        // ✅ Helper - Obter data de início baseado no período
        private DateTime ObterDataInicio(string periodo)
        {
            return periodo switch
            {
                "24h" => DateTime.Now.AddHours(-24),
                "7d" => DateTime.Now.AddDays(-7),
                "30d" => DateTime.Now.AddDays(-30),
                "1y" => DateTime.Now.AddYears(-1),
                "all" => DateTime.MinValue,
                _ => DateTime.Now.AddDays(-7)
            };
        }
    }
}