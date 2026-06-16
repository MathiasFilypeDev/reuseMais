using Microsoft.EntityFrameworkCore;
using ReusePlusApi.Models;

namespace ReusePlusApi.Data
{
    /// <summary>
    /// Contexto do banco de dados da aplicação
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Movimentacao> Movimentacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração de User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Senha).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Tipo).IsRequired().HasMaxLength(20).HasDefaultValue("usuario");
                entity.Property(e => e.DataCadastro).IsRequired().HasDefaultValue(DateTime.UtcNow);
            });

            // Configuração de Item
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Quantidade).IsRequired();
                entity.Property(e => e.Valor).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Descricao).HasMaxLength(500);
                entity.Property(e => e.DataCadastro).IsRequired().HasDefaultValue(DateTime.UtcNow);
            });

            // Configuração de Movimentacao
            modelBuilder.Entity<Movimentacao>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Produto).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Quantidade).IsRequired();
                entity.Property(e => e.Tipo).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Valor).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Observacoes).HasMaxLength(500);
                entity.Property(e => e.Data).IsRequired().HasDefaultValue(DateTime.UtcNow);
            });
        }
    }
}
