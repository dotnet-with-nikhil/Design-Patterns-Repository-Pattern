using DotNet8_Enterprise_CRUD.DTOs;
using DotNet8_Enterprise_CRUD.Models;
using DotNet8_Enterprise_CRUD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
namespace DotNet8_Enterprise_CRUD.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("api")]
public class ProductsController(IProductService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) => Ok(await service.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var p = await service.GetByIdAsync(id, ct); return p is null ? NotFound() : Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateDto dto, CancellationToken ct)
    {
        var p = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ProductUpdateDto dto, CancellationToken ct) =>
        await service.UpdateAsync(id, dto, ct) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct) =>
        await service.DeleteAsync(id, ct) ? NoContent() : NotFound();

    [HttpGet("enumerable")]
    public async Task<IActionResult> GetAsEnumerable(
    CancellationToken cancellationToken)
    {
        var products =
            await service.GetProductsAsEnumerableAsync(cancellationToken);

        return Ok(products);
    }

    [HttpGet("queryable")]
    public IActionResult GetProducts()
    {
        var products = service
            .GetProductsQueryable()
            .Where(p => p.Price > 100)
            .OrderBy(p => p.Name)
            .ToList();

        return Ok(products);
    }
}
