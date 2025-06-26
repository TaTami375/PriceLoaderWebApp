using System.Net.Mail;
using PriceLoaderWebApp.Domain.Entities;

namespace PriceLoaderWebApp.Application.Services
{
    public interface IEmailService
    {
        /// <summary>
        /// Получает последнее письмо от нужного поставщика.
        /// </summary>
        Task<List<EmailMessage>> GetUnreadEmailsFromSupplierAsync(string supplierName);

        /// <summary>
        /// Скачивает вложение по имени.
        /// </summary>
        Task<byte[]> DownloadAttachmentAsync(string filename);
    }
}
