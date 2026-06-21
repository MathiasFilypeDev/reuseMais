using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ReuseMaisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FavoritosController : ControllerBase
    {
        // Lista em memória (depois usar banco de dados)
        private static List<dynamic> favoritos = new List<dynamic>();
        private static int nextId = 1;

        // ✅ GET - Meus favoritos
        [HttpGet]
        public IActionResult GetMeusFavoritos()
        {
            var userId = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int userIdInt))
                return Unauthorized();

            var meusFavoritos = favoritos
                .Where(f => f["userId"] == userIdInt)
                .ToList();

            return Ok(meusFavoritos);
        }

        // ✅ POST - Adicionar aos favoritos
        [HttpPost("{produtoId}")]
        public IActionResult AdicionarFavorito(int produtoId)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            // Verificar se já existe
            if (favoritos.Any(f => f["userId"] == userId && f["produtoId"] == produtoId))
                return BadRequest(new { message = "Já está nos favoritos" });

            favoritos.Add(new
            {
                id = nextId++,
                userId,
                produtoId,
                dataCriacao = DateTime.Now
            });

            return Ok(new { message = "Adicionado aos favoritos" });
        }

        // ✅ DELETE - Remover dos favoritos
        [HttpDelete("{produtoId}")]
        public IActionResult RemoverFavorito(int produtoId)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var favorito = favoritos.FirstOrDefault(f => f["userId"] == userId && f["produtoId"] == produtoId);

            if (favorito == null)
                return NotFound();

            favoritos.Remove(favorito);
            return Ok(new { message = "Removido dos favoritos" });
        }
    }
}