namespace PriceLoaderWebApp.Application.DTOs
{
    public class PriceItemDto
    {
        public string Vendor { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
    }
}
