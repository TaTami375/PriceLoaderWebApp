namespace PriceLoaderWebApp.Domain.Exceptions
{
    public class EmailNotFoundException : PriceLoaderException
    {
        public EmailNotFoundException(string message) : base(message) { }
    }
}
