using PriceLoaderWebApp.Domain.Entities;

namespace PriceLoaderWebApp.Application.Services
{
    public interface IPriceItemRepository
    {
        Task SaveAsync(IEnumerable<PriceItem> items, CancellationToken ct = default);
    }
}
