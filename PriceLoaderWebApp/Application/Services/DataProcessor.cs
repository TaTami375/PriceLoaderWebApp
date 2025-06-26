using System;
using System.Collections.Generic;
using System.Text;
using PriceLoaderWebApp.Domain.Entities;
using PriceLoaderWebApp.Application.Services;

namespace PriceLoaderWebApp.Infrastructure.Persistence
{
    public class DataProcessor : IDataProcessor
    {
        public PriceItem Process(PriceItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return new PriceItem
            {
                Vendor = item.Vendor,
                Number = item.Number,
                SearchVendor = CleanStringForSearch(item.Vendor),
                SearchNumber = CleanStringForSearch(item.Number),
                Description = TruncateDescription(item.Description),
                Price = item.Price,
                Count = item.Count
            };
        }

        public IEnumerable<PriceItem> ProcessAll(IEnumerable<PriceItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
            {
                yield return Process(item);
            }
        }

        private string CleanStringForSearch(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var result = new StringBuilder();

            foreach (char c in input.ToUpper())
            {
                if (char.IsLetterOrDigit(c) || c == '_')
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        private string TruncateDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                return description;

            const int maxLength = 512;

            return description.Length > maxLength
                ? description.Substring(0, maxLength)
                : description;
        }
    }
}