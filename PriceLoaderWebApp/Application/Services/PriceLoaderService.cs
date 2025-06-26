using PriceLoaderWebApp.Domain.Entities;
using PriceLoaderWebApp.Domain.Exceptions;
using PriceLoaderWebApp.Infrastructure.Persistence;

namespace PriceLoaderWebApp.Application.Services
{
    public class PriceLoaderService : IPriceLoaderService
    {
        private readonly IEmailService _emailService;
        private readonly ICsvParser _csvParser;
        private readonly IDataProcessor _dataProcessor;
        private readonly IPriceItemRepository _repository;

        public PriceLoaderService(
            IEmailService emailService,
            ICsvParser csvParser,
            IDataProcessor dataProcessor,
            IPriceItemRepository repository)
        {
            _emailService = emailService;
            _csvParser = csvParser;
            _dataProcessor = dataProcessor;
            _repository = repository;
        }

        public async Task<IEnumerable<PriceItem>> LoadLatestPriceAsync(string supplierName)
        {
            var emails = await _emailService.GetUnreadEmailsFromSupplierAsync(supplierName);

            if (emails.Count == 0)
                throw new EmailNotFoundException($"Письма от поставщика '{supplierName}' не найдены.");

            var allProcessedItems = new List<PriceItem>();

            foreach (var email in emails)
            {
                foreach (var attachmentName in email.Attachments.Where(a => a.EndsWith(".csv")))
                {
                    var attachment = await _emailService.DownloadAttachmentAsync(attachmentName);
                    var items = _csvParser.Parse(attachment, supplierName);
                    var processedItems = items.Select(i => _dataProcessor.Process(i)).ToList();
                    allProcessedItems.AddRange(processedItems);
                }
            }

            await _repository.SaveAsync(allProcessedItems);

            return allProcessedItems;
        }
    }
}
