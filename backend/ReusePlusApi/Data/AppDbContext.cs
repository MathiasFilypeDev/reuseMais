using Microsoft.EntityFrameworkCore;
using ReusePlusApi.Models;

namespace ReusePlusApi.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Movimentacao> Movimentacoes { get; set; }
    }
}
