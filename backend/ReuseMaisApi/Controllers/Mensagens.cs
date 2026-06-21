using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ReuseMaisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MensagensController : ControllerBase
    {
        private static List<dynamic> mensagens = new List<dynamic>();
        private static int nextId = 1;

        // ✅ POST - Enviar mensagem
        [HttpPost]
        public IActionResult EnviarMensagem([FromBody] EnviarMensagemRequest request)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            var userName = User.FindFirst("nome")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            if (string.IsNullOrEmpty(request.Conteudo) || request.ParaUsuarioId <= 0)
                return BadRequest(new { message = "Conteúdo e usuário destinatário são obrigatórios" });

            mensagens.Add(new
            {
                id = nextId++,
                deUsuarioId = userId,
                deUsuarioNome = userName,
                paraUsuarioId = request.ParaUsuarioId,
                conteudo = request.Conteudo,
                produtoId = request.ProdutoId,
                dataEnvio = DateTime.Now,
                lida = false
            });

            return Ok(new { message = "Mensagem enviada com sucesso" });
        }

        // ✅ GET - Minhas mensagens
        [HttpGet]
        public IActionResult GetMinhasMensagens()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var minhasMensagens = mensagens
                .Where(m => m["paraUsuarioId"] == userId)
                .OrderByDescending(m => m["dataEnvio"])
                .ToList();

            return Ok(minhasMensagens);
        }

        // ✅ GET - Conversa com usuário
        [HttpGet("conversa/{outroUsuarioId}")]
        public IActionResult GetConversa(int outroUsuarioId)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var conversa = mensagens
                .Where(m =>
                    (m["deUsuarioId"] == userId && m["paraUsuarioId"] == outroUsuarioId) ||
                    (m["deUsuarioId"] == outroUsuarioId && m["paraUsuarioId"] == userId)
                )
                .OrderBy(m => m["dataEnvio"])
                .ToList();

            return Ok(conversa);
        }
    }

    public class EnviarMensagemRequest
    {
        public int ParaUsuarioId { get; set; }
        public string? Conteudo { get; set; }
        public int? ProdutoId { get; set; }
    }
}