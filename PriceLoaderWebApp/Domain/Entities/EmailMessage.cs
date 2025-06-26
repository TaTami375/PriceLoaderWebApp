namespace PriceLoaderWebApp.Domain.Entities
{
    public class EmailMessage
    {
        public string From { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public string Body { get; set; }
        public List<string> Attachments { get; set; } = new();
        public bool IsRead { get; set; }
    }
}
