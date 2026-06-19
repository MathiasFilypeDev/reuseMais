using Microsoft.AspNetCore.Mvc;
using ReuseMaisApi.Models;

namespace ReuseMaisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private static List<Item> items = new List<Item>();

        [HttpGet]
        public IActionResult GetItems() => Ok(items);

        [HttpPost]
        public IActionResult AddItem(Item item)
        {
            item.Id = items.Count + 1;
            items.Add(item);
            return Ok(item);
        }
    }
}
