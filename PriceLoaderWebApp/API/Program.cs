using Microsoft.EntityFrameworkCore;
using PriceLoaderWebApp.Application.Services;
using PriceLoaderWebApp.Infrastructure.Mail;
using PriceLoaderWebApp.Infrastructure.Persistence;
using PriceLoaderWebApp.Infrastructure.Configuration;
using PriceLoaderWebApp.Infrastructure.Persistence;
using PriceLoaderWebApp.Mappings;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Регистрация своих сервисов:
builder.Services.AddTransient<IEmailService, ImapEmailService>();
builder.Services.AddTransient<ICsvParser, CsvParser>();
builder.Services.AddTransient<IDataProcessor, DataProcessor>();
builder.Services.AddTransient<IPriceItemRepository, PriceItemRepository>();
builder.Services.AddTransient<IPriceLoaderService, PriceLoaderService>();

builder.Services.Configure<ImapSettings>(builder.Configuration.GetSection("ImapSettings"));

builder.Services.Configure<SupplierConfig>(builder.Configuration.GetSection("Supplier"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
