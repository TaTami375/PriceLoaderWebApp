using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using PriceLoaderWebApp.Domain.Entities;
using PriceLoaderWebApp.Application.Services;
using PriceLoaderWebApp.Domain.Exceptions;
using PriceLoaderWebApp.Infrastructure.Configuration;
using System.Text;

namespace PriceLoaderWebApp.Infrastructure.Persistence
{
    public class CsvParser : ICsvParser
    {
        private readonly IEnumerable<SupplierConfig> _supplierConfigs;

        public CsvParser(IEnumerable<SupplierConfig> supplierConfigs)
        {
            _supplierConfigs = supplierConfigs;
        }

        public IEnumerable<PriceItem> Parse(byte[] csvData, string supplierName)
        {
            if (csvData == null || csvData.Length == 0)
                throw new InvalidCsvFormatException("CSV-данные пусты.");

            var config = _supplierConfigs.FirstOrDefault(c => c.Name == supplierName)
                ?? throw new InvalidCsvFormatException($"Конфигурация для поставщика '{supplierName}' не найдена.");

            var result = new List<PriceItem>();

            using var memoryStream = new MemoryStream(csvData);
            using var reader = new StreamReader(memoryStream, Encoding.UTF8);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();

            var headerMap = new Dictionary<string, int>();
            for (int i = 0; i < csv.HeaderRecord.Length; i++)
            {
                var header = csv.HeaderRecord[i];
                headerMap[header] = i;
            }

            // Проверяем наличие нужных колонок
            foreach (var mapping in config.ColumnMapping)
            {
                if (!headerMap.ContainsKey(mapping.Value))
                    throw new InvalidCsvFormatException($"В CSV отсутствует необходимая колонка: {mapping.Value}");
            }

            while (csv.Read())
            {
                var values = csv.Parser.Record;

                var item = new PriceItem
                {
                    Vendor = GetValue(values, headerMap, config.ColumnMapping["Vendor"]),
                    Number = GetValue(values, headerMap, config.ColumnMapping["Number"]),
                    Description = GetValue(values, headerMap, config.ColumnMapping["Description"]),
                    Price = GetPrice(GetValue(values, headerMap, config.ColumnMapping["Price"])),
                    Count = GetCount(GetValue(values, headerMap, config.ColumnMapping["Count"]))
                };

                result.Add(item);
            }

            return result;
        }

        private string GetValue(string[] values, Dictionary<string, int> headers, string headerName)
        {
            if (!headers.TryGetValue(headerName, out int index))
                return null;

            return values.Length > index ? values[index]?.Trim() : null;
        }

        private decimal GetPrice(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
                return price;

            return 0;
        }

        private int GetCount(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            value = value.Trim();

            if (value.StartsWith(">"))
                return int.TryParse(value.Substring(1).Trim(), out var val) ? val : 0;

            if (value.StartsWith("<"))
                return int.TryParse(value.Substring(1).Trim(), out var val) ? val : 0;

            var parts = value.Split('-');
            if (parts.Length == 2 && int.TryParse(parts[1].Trim(), out var upper))
                return upper;

            return int.TryParse(value, out var result) ? result : 0;
        }
    }
}