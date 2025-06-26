using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PriceLoaderWebApp.Application.DTOs;
using PriceLoaderWebApp.Application.Services;
using PriceLoaderWebApp.Infrastructure.Configuration;

[ApiController]
[Route("api/[controller]")]
public class PriceController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPriceLoaderService _priceLoaderService;

    public PriceController(IMapper mapper, IPriceLoaderService priceLoaderService)
    {
        _mapper = mapper;
        _priceLoaderService = priceLoaderService;
    }

    [HttpPost("load")]
    public async Task<IActionResult> LoadLatestPrice([FromBody] string supplierName)
    {
        if (string.IsNullOrWhiteSpace(supplierName))
            return BadRequest("Supplier name is required");

        var items = await _priceLoaderService.LoadLatestPriceAsync(supplierName);
        var itemDtos = _mapper.Map<IEnumerable<PriceItemDto>>(items);

        return Ok(itemDtos);
    }
}