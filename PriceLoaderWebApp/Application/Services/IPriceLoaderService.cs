using PriceLoaderWebApp.Domain.Entities;

namespace PriceLoaderWebApp.Application.Services
{
    public interface IPriceLoaderService
    {
        Task<IEnumerable<PriceItem>> LoadLatestPriceAsync(string supplierName);
    }
}
