using Microsoft.EntityFrameworkCore;
using ReusePlusApi.Models;

namespace ReusePlusApi
{
    public class ReusePlusContext : DbContext
    {
        public ReusePlusContext(DbContextOptions<ReusePlusContext> options) : base(options) { }

        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Movimentacao> Movimentacoes { get; set; } = null!;

    }
}
