using PriceLoaderWebApp.Domain.Entities;

namespace PriceLoaderWebApp.Infrastructure.Persistence
{
    public interface IPriceItemRepository
    {
        Task SaveAsync(IEnumerable<PriceItem> items, CancellationToken ct);
    }
}
