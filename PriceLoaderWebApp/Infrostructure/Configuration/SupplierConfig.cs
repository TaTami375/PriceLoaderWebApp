namespace PriceLoaderWebApp.Infrastructure.Configuration
{
    public class SupplierConfig
    {
        public string Name { get; set; }
        public Dictionary<string, string> ColumnMapping { get; set; } = new();
    }
}
