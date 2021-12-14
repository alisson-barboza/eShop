using eShop.Core.Communication.Mediator;
using eShop.Vendas.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.Vendas.Data
{
    public class VendasContext : DbContext
    {
        private readonly IMediatrHandler _mediatrHandler;
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Item> PedidosItens { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }

        public VendasContext(DbContextOptions<VendasContext> options, IMediatrHandler mediatrHandler)
            : base(options)
        {
            _mediatrHandler = mediatrHandler;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
            //    e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            //    property.Relational().ColumnType = "varchar(100)";
            //modelBuilder.Ignore<Event>();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VendasContext).Assembly);
            modelBuilder.HasSequence<int>("MinhaSequencia").StartsAt(1000).IncrementsBy(1);
            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }

            var success = await base.SaveChangesAsync() > 0;
            if (success)
                await _mediatrHandler.PublishEvents(this);

            return success;
        }
    }
}