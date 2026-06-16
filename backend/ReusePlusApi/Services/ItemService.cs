using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReusePlusApi.Constants;
using ReusePlusApi.DTOs;
using ReusePlusApi.Models;
using ReusePlusApi.Repositories;

namespace ReusePlusApi.Services
{
    /// <summary>
    /// Interface para serviço de itens
    /// </summary>
    public interface IItemService
    {
        Task<ItemResponseDto> CreateItemAsync(CreateItemRequestDto request);
        Task<ItemResponseDto> GetItemByIdAsync(int id);
        Task<IEnumerable<ItemResponseDto>> GetAllItemsAsync();
        Task<ItemResponseDto> UpdateItemAsync(UpdateItemRequestDto request);
        Task<bool> DeleteItemAsync(int id);
    }

    /// <summary>
    /// Implementação do serviço de itens
    /// </summary>
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<ItemResponseDto> CreateItemAsync(CreateItemRequestDto request)
        {
            var item = new Item
            {
                Nome = request.Nome,
                Quantidade = request.Quantidade,
                Valor = request.Valor,
                Descricao = request.Descricao,
                DataCadastro = DateTime.UtcNow
            };

            var createdItem = await _itemRepository.AddAsync(item);
            return MapToDto(createdItem);
        }

        public async Task<ItemResponseDto> GetItemByIdAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException(ErrorMessages.NotFound);

            return MapToDto(item);
        }

        public async Task<IEnumerable<ItemResponseDto>> GetAllItemsAsync()
        {
            var items = await _itemRepository.GetAllAsync();
            var result = new List<ItemResponseDto>();

            foreach (var item in items)
            {
                result.Add(MapToDto(item));
            }

            return result;
        }

        public async Task<ItemResponseDto> UpdateItemAsync(UpdateItemRequestDto request)
        {
            var item = await _itemRepository.GetByIdAsync(request.Id);
            if (item == null)
                throw new KeyNotFoundException(ErrorMessages.NotFound);

            if (!string.IsNullOrEmpty(request.Nome))
                item.Nome = request.Nome;

            if (request.Quantidade.HasValue)
                item.Quantidade = request.Quantidade.Value;

            if (request.Valor.HasValue)
                item.Valor = request.Valor.Value;

            if (!string.IsNullOrEmpty(request.Descricao))
                item.Descricao = request.Descricao;

            item.DataAtualizacao = DateTime.UtcNow;

            var updatedItem = await _itemRepository.UpdateAsync(item);
            return MapToDto(updatedItem);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            return await _itemRepository.DeleteAsync(id);
        }

        private static ItemResponseDto MapToDto(Item item)
        {
            return new ItemResponseDto
            {
                Id = item.Id,
                Nome = item.Nome,
                Quantidade = item.Quantidade,
                Valor = item.Valor,
                Descricao = item.Descricao,
                DataCadastro = item.DataCadastro,
                DataAtualizacao = item.DataAtualizacao
            };
        }
    }
}
