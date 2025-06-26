using System.Collections.Generic;
using PriceLoaderWebApp.Domain.Entities;

namespace PriceLoaderWebApp.Application.Services
{
    public interface IDataProcessor
    {
        PriceItem Process(PriceItem item);
        IEnumerable<PriceItem> ProcessAll(IEnumerable<PriceItem> items);
    }
}