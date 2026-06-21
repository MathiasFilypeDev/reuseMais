using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ReuseMaisApi.Models;

namespace ReuseMaisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private static List<Produto> produtos = new List<Produto>
        {
            new Produto { Id = 1, Nome = "Camiseta", Categoria = "Roupas", Quantidade = 10, Descricao = "Camiseta azul tamanho M", CriadoPorUserId = 1, CriadoPorNome = "João" },
            new Produto { Id = 2, Nome = "Calça Jeans", Categoria = "Roupas", Quantidade = 5, Descricao = "Calça jeans tamanho 40", CriadoPorUserId = 2, CriadoPorNome = "Maria" }
        };

        private static int nextId = 3;

        // ✅ GET - Listar todos os produtos
        [HttpGet]
        [Authorize]
        public IActionResult GetProdutos()
        {
            return Ok(produtos);
        }

        // ✅ GET - Meus produtos (apenas do usuário logado)
        [HttpGet("meus")]
        [Authorize]
        public IActionResult GetMeusProdutos()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Usuário não identificado" });

            var meusProdutos = produtos.Where(p => p.CriadoPorUserId == userId).ToList();
            return Ok(meusProdutos);
        }

        // ✅ POST - Criar novo produto
        [HttpPost]
        [Authorize]
        public IActionResult CreateProduto([FromBody] CreateProdutoRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Nome) || string.IsNullOrEmpty(request.Categoria))
                return BadRequest(new { message = "Nome e categoria são obrigatórios" });

            var userIdClaim = User.FindFirst("userId")?.Value;
            var userName = User.FindFirst("nome")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Usuário não identificado" });

            var novoProduto = new Produto
            {
                Id = nextId++,
                Nome = request.Nome,
                Descricao = request.Descricao,
                Categoria = request.Categoria,
                Quantidade = request.Quantidade,
                CriadoPorUserId = userId,
                CriadoPorNome = userName ?? "Desconhecido"
            };

            produtos.Add(novoProduto);
            return CreatedAtAction(nameof(GetProdutos), new { id = novoProduto.Id }, novoProduto);
        }

        // ✅ PUT - Editar produto
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateProduto(int id, [FromBody] UpdateProdutoRequest request)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Usuário não identificado" });

            var produto = produtos.FirstOrDefault(p => p.Id == id);

            if (produto == null)
                return NotFound(new { message = "Produto não encontrado" });

            if (produto.CriadoPorUserId != userId)
                return Forbid();

            if (!string.IsNullOrEmpty(request.Nome))
                produto.Nome = request.Nome;
            if (!string.IsNullOrEmpty(request.Descricao))
                produto.Descricao = request.Descricao;
            if (!string.IsNullOrEmpty(request.Categoria))
                produto.Categoria = request.Categoria;
            if (request.Quantidade >= 0)
                produto.Quantidade = request.Quantidade;

            return Ok(new { message = "Produto atualizado com sucesso", produto });
        }

        // ✅ GET - Buscar produtos
        [HttpGet("buscar")]
        [Authorize]
        public IActionResult BuscarProdutos([FromQuery] string termo = "", [FromQuery] string categoria = "")
        {
            var resultado = produtos.AsEnumerable();

            if (!string.IsNullOrEmpty(termo))
            {
                resultado = resultado.Where(p =>
                    p.Nome != null && p.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                    p.Descricao != null && p.Descricao.Contains(termo, StringComparison.OrdinalIgnoreCase)
                );
            }

            if (!string.IsNullOrEmpty(categoria))
            {
                resultado = resultado.Where(p => p.Categoria == categoria);
            }

            return Ok(resultado.ToList());
        }

        // ✅ GET - Listar categorias
        [HttpGet("categorias")]
        [Authorize]
        public IActionResult GetCategorias()
        {
            var categorias = produtos
                .Select(p => p.Categoria)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            return Ok(categorias);
        }

        // ✅ DELETE - Deletar produto
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteProduto(int id)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            var userRole = User.FindFirst("role")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Usuário não identificado" });

            var produto = produtos.FirstOrDefault(p => p.Id == id);

            if (produto == null)
                return NotFound(new { message = "Produto não encontrado" });

            bool podeDeltar = userRole == "admin" || produto.CriadoPorUserId == userId;

            if (!podeDeltar)
                return Forbid();

            produtos.Remove(produto);
            return Ok(new { message = "Produto deletado com sucesso" });
        }
    }

    public class CreateProdutoRequest
    {
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Categoria { get; set; }
        public int Quantidade { get; set; }
    }

    public class UpdateProdutoRequest
    {
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Categoria { get; set; }
        public int Quantidade { get; set; }
    }
}

