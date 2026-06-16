using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReusePlusApi.Constants;
using ReusePlusApi.DTOs;
using ReusePlusApi.Services;

namespace ReusePlusApi.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar itens de inventário
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// Obtém todos os itens
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ItemResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var items = await _itemService.GetAllItemsAsync();
                return Ok(items);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto
                {
                    Message = ErrorMessages.InternalServerError,
                    ErrorCode = "INTERNAL_ERROR"
                });
            }
        }

        /// <summary>
        /// Obtém um item específico
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ItemResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetItemById(int id)
        {
            try
            {
                var item = await _itemService.GetItemByIdAsync(id);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponseDto
                {
                    Message = ex.Message,
                    ErrorCode = "ITEM_NOT_FOUND"
                });
            }
        }

        /// <summary>
        /// Cria um novo item
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ItemResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var item = await _itemService.CreateItemAsync(request);
                return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
            }
            catch
            {
                return BadRequest(new ErrorResponseDto
                {
                    Message = ErrorMessages.BadRequest,
                    ErrorCode = "ITEM_CREATION_FAILED"
                });
            }
        }

        /// <summary>
        /// Atualiza um item
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ItemResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] UpdateItemRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            request.Id = id;

            try
            {
                var item = await _itemService.UpdateItemAsync(request);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponseDto
                {
                    Message = ex.Message,
                    ErrorCode = "ITEM_NOT_FOUND"
                });
            }
        }

        /// <summary>
        /// Deleta um item
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                var success = await _itemService.DeleteItemAsync(id);
                if (!success)
                    return NotFound(new ErrorResponseDto
                    {
                        Message = ErrorMessages.NotFound,
                        ErrorCode = "ITEM_NOT_FOUND"
                    });

                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto
                {
                    Message = ErrorMessages.InternalServerError,
                    ErrorCode = "INTERNAL_ERROR"
                });
            }
        }
    }
}
