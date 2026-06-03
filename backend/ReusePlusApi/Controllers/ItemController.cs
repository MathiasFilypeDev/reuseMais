using Microsoft.AspNetCore.Mvc;
using ReusePlusApi.Data;
using ReusePlusApi.Models;

namespace ReusePlusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CriarItem([FromBody] Item item)
        {
            _context.Items.Add(item);
            _context.SaveChanges();
            return Ok(item);
        }
    }
}
