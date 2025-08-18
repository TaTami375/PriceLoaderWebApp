using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql.Bulk;
using PriceLoaderWebApp.Application.Services;
using PriceLoaderWebApp.Domain.Entities;
using PriceLoaderWebApp.Domain.Exceptions;

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

            if (_context.Database.GetDbConnection().State != System.Data.ConnectionState.Open)
                await _context.Database.OpenConnectionAsync(ct);

            var bulk = new NpgsqlBulkUploader(_context, true);

            await bulk.InsertAsync(
                items                
            );
            
        }
    }
}