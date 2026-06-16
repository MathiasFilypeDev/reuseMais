using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReusePlusApi.Data;
using ReusePlusApi.Models;

namespace ReusePlusApi.Repositories
{
    /// <summary>
    /// Implementação genérica do repositório
    /// </summary>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.FindAsync(id) != null;
        }
    }

    /// <summary>
    /// Implementação do repositório de Usuários
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }
    }

    /// <summary>
    /// Implementação do repositório de Itens
    /// </summary>
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Item>> GetByNameAsync(string name)
        {
            return await _dbSet.Where(i => i.Nome.Contains(name)).ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetLowStockAsync(int threshold)
        {
            return await _dbSet.Where(i => i.Quantidade <= threshold).ToListAsync();
        }
    }

    /// <summary>
    /// Implementação do repositório de Movimentações
    /// </summary>
    public class MovimentacaoRepository : Repository<Movimentacao>, IMovimentacaoRepository
    {
        public MovimentacaoRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Movimentacao>> GetByTypeAsync(string tipo)
        {
            return await _dbSet.Where(m => m.Tipo == tipo).ToListAsync();
        }

        public async Task<IEnumerable<Movimentacao>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(m => m.Data >= startDate && m.Data <= endDate).ToListAsync();
        }
    }
}
