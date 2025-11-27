using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Padoka.Models;

namespace Padoka.Infraestrutura
{
    public class PadokaContext(DbContextOptions<PadokaContext> options) : DbContext(options)
    {
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<ItemCardapio> ItensCardapio { get; set; }
        public DbSet<OpcaoAdicional> OpcoesAdicionais { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<ItemPedidoOpcao> ItensPedidoOpcao { get; set; }
        public DbSet<HistoricoStatusPedido> HistoricoStatusPedidos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Conversão de DateTime para UTC
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(
                            new ValueConverter<DateTime, DateTime>(
                                v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                            )
                        );
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(
                            new ValueConverter<DateTime?, DateTime?>(
                                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null,
                                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null
                            )
                        );
                    }
                }
            }

            modelBuilder.Entity<ItemCardapio>()
                .HasOne(e => e.Categoria)
                .WithMany(c => c.Itens)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasIndex(e => e.NumeroPedido).IsUnique();
                entity.HasOne(e => e.Usuario)
                    .WithMany(u => u.Pedidos)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ItemPedido>(entity =>
            {
                entity.HasOne(e => e.ItemCardapio)
                    .WithMany(i => i.ItensPedido)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ItemPedidoOpcao>()
                .HasOne(e => e.OpcaoAdicional)
                .WithMany(o => o.ItensPedidoOpcao)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HistoricoStatusPedido>()
                .HasOne(e => e.AlteradoPor)
                .WithMany(u => u.AlteracoesStatus)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Usuario>()
                .HasIndex(e => e.Email).IsUnique();
        }
    }
}
