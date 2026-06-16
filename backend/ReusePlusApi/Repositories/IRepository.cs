using System.Collections.Generic;
using System.Threading.Tasks;
using ReusePlusApi.Models;

namespace ReusePlusApi.Repositories
{
    /// <summary>
    /// Interface para o repositório genérico
    /// </summary>
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }

    /// <summary>
    /// Interface específica para repositório de Usuários
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
    }

    /// <summary>
    /// Interface específica para repositório de Itens
    /// </summary>
    public interface IItemRepository : IRepository<Item>
    {
        Task<IEnumerable<Item>> GetByNameAsync(string name);
        Task<IEnumerable<Item>> GetLowStockAsync(int threshold);
    }

    /// <summary>
    /// Interface específica para repositório de Movimentações
    /// </summary>
    public interface IMovimentacaoRepository : IRepository<Movimentacao>
    {
        Task<IEnumerable<Movimentacao>> GetByTypeAsync(string tipo);
        Task<IEnumerable<Movimentacao>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
