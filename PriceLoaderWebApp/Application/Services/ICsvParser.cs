using PriceLoaderWebApp.Domain.Entities;
using PriceLoaderWebApp.Infrastructure.Configuration;

namespace PriceLoaderWebApp.Application.Services
{
    public interface ICsvParser
    {
        IEnumerable<PriceItem> Parse(byte[] csvData, string supplierName);
    }
}
