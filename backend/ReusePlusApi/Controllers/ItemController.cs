using Microsoft.AspNetCore.Mvc;
using ReusePlusApi.Models;

namespace ReusePlusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private static readonly List<Item> _items = new();
        private static int _nextId = 1;

        [HttpGet]
        public ActionResult<IEnumerable<Item>> GetAll()
        {
            return Ok(_items);
        }

        [HttpGet("{id}")]
        public ActionResult<Item> GetById(int id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item is null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<Item> Create(Item item)
        {
            item.Id = _nextId++;
            item.DataCadastro = DateTime.UtcNow;
            _items.Add(item);

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Item item)
        {
            var existing = _items.FirstOrDefault(i => i.Id == id);
            if (existing is null) return NotFound();

            existing.Nome = item.Nome;
            existing.Quantidade = item.Quantidade;
            existing.Valor = item.Valor;
            existing.Descricao = item.Descricao;
            existing.DataAtualizacao = DateTime.UtcNow;

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item is null) return NotFound();

            _items.Remove(item);
            return NoContent();
        }
    }
}
