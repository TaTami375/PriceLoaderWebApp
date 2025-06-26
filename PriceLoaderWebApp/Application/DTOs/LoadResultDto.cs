namespace PriceLoaderWebApp.Application.DTOs
{
    public class LoadResultDto
    {
        public int TotalItemsLoaded { get; set; }
        public DateTime LoadTime { get; set; }
        public string Status { get; set; } // "Success", "Partial", "Failed"
    }
}
