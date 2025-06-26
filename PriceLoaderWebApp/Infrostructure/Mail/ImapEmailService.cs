using System.Net.Mail;
using System.Text;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Options;
using MimeKit;
using PriceLoaderWebApp.Application.Services;
using PriceLoaderWebApp.Domain.Entities;
using PriceLoaderWebApp.Domain.Exceptions;
using PriceLoaderWebApp.Infrastructure.Configuration;

namespace PriceLoaderWebApp.Infrastructure.Mail
{
    public class ImapEmailService : IEmailService
    {
        private readonly ImapSettings _imapSettings;

        public ImapEmailService(IOptions<ImapSettings> imapSettings)
        {
            _imapSettings = imapSettings.Value;
        }

        public async Task<List<EmailMessage>> GetUnreadEmailsFromSupplierAsync(string supplierName)
        {
            using var client = new ImapClient();

            await client.ConnectAsync(_imapSettings.Host, _imapSettings.Port, true);
            await client.AuthenticateAsync(_imapSettings.Username, _imapSettings.Password);

            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadWrite);

            // Ищем все непрочитанные письма
            var uids = await inbox.SearchAsync(SearchQuery.NotSeen.And(SearchQuery.FromContains(supplierName)));

            if (!uids.Any())
                throw new EmailNotFoundException($"Нет непрочитанных письма от '{supplierName}'.");

            var messages = new List<EmailMessage>();

            foreach (var uid in uids)
            {
                var message = await inbox.GetMessageAsync(uid);

                var emailMessage = new EmailMessage
                {
                    From = message.From.ToString(),
                    Subject = message.Subject,
                    Date = message.Date.DateTime,
                    Attachments = message.Attachments
                        .Where(a => a.ContentType.MimeType == "application/octet-stream" ||
                                    a.ContentType.MimeType == "text/csv")
                        .Select(a => a.ContentDisposition?.FileName ?? a.ContentType.Name)
                        .ToList()
                };

                messages.Add(emailMessage);
            }

            return messages;
        }

        public async Task<byte[]> DownloadAttachmentAsync(string filename)
        {
            using var client = new ImapClient();

            await client.ConnectAsync(_imapSettings.Host, _imapSettings.Port, true);
            await client.AuthenticateAsync(_imapSettings.Username, _imapSettings.Password);

            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadWrite);

            var uids = await inbox.SearchAsync(SearchQuery.NotSeen);
            if (!uids.Any())
                throw new EmailNotFoundException("Новых писем нет");

            var latestUid = uids.Last();
            var message = await inbox.GetMessageAsync(latestUid);

            foreach (var attachment in message.Attachments)
            {
                if (attachment is MimePart mimePart)
                {
                    string attachmentName = mimePart.FileName;

                    if (string.Equals(attachmentName, filename, StringComparison.OrdinalIgnoreCase))
                    {
                        using var memoryStream = new MemoryStream();
                        await mimePart.Content.DecodeToAsync(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
            }

            throw new AttachmentNotFoundException($"Вложение '{filename}' не найдено.");
        }
    }
}