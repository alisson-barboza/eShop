using eShop.Core.Communication.Mediator;
using eShop.Core.DomainObjects;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.Pagamentos.Data
{
    public static class MediatorExtension
    {
        public static async Task PublishEvents(this IMediatrHandler mediator, PagamentoContext context)
        {
            var domainEntities = context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Events is not null && x.Entity.Events.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Events)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.PublishEvent(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}