using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Data.Interceptors
{
    public class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await DispatchDomainEvents(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        public async Task DispatchDomainEvents(DbContext? context)
        {
            if (context is null) return;

            var aggregates = context.ChangeTracker.Entries<IAggregate>()
                .Where(a => a.Entity.DomainEvents.Any())
                .Select(c => c.Entity);

            var domainEvents = aggregates.SelectMany(m => m.DomainEvents)
                .ToList();

            aggregates.ToList().ForEach(c => c.ClearDomainEvent());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);

        }
    }
    
}
