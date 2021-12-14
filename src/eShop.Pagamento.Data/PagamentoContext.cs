using eShop.Core.Communication.Mediator;
using eShop.Core.Data;
using eShop.Pagamentos.Business;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.Pagamentos.Data
{
    public class PagamentoContext : DbContext, IUnitOfWork
    {
        private readonly IMediatrHandler _mediatrHandler;

        public PagamentoContext(DbContextOptions<PagamentoContext> options, IMediatrHandler mediatrHandler)
            : base(options)
        {
            _mediatrHandler = mediatrHandler;
        }

        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

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