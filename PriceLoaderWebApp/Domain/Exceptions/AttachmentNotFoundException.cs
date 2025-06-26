namespace PriceLoaderWebApp.Domain.Exceptions
{
    public class AttachmentNotFoundException : PriceLoaderException
    {
        public AttachmentNotFoundException(string message) : base(message) { }
    }
}
