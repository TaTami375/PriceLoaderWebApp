namespace PriceLoaderWebApp.Domain.Exceptions
{
    public class InvalidCsvFormatException : PriceLoaderException
    {
        public InvalidCsvFormatException(string message) : base(message) { }
    }
}
