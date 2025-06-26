using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PriceLoaderWebApp.Domain.Entities;
using PriceLoaderWebApp.Domain.Exceptions;
using PriceLoaderWebApp.Infrastructure.Persistence;

namespace PriceLoaderWebApp.Infrastructure.Persistence
{
    public class PriceItemRepository : IPriceItemRepository
    {
        private readonly AppDbContext _context;

        public PriceItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(IEnumerable<PriceItem> items, CancellationToken ct = default)
        {
            if (items == null || !items.Any())
                return;

            await using var transaction = await _context.Database.BeginTransactionAsync(ct);

            try
            {
                await _context.PriceItems.AddRangeAsync(items, ct);
                await _context.SaveChangesAsync(ct);

                await transaction.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                throw new PriceLoaderException("Ошибка при сохранении данных в БД");
            }
        }
    }
}