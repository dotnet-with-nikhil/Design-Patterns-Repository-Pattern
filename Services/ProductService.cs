using DotNet8_Enterprise_CRUD.DTOs;
using DotNet8_Enterprise_CRUD.Models;
using DotNet8_Enterprise_CRUD.Repositories;
using Microsoft.EntityFrameworkCore;
namespace DotNet8_Enterprise_CRUD.Services;

public class ProductService(IProductRepository repo) : IProductService
{
    public Task<List<Product>> GetAllAsync(CancellationToken ct) => repo.GetAllAsync(ct);
    public Task<Product?> GetByIdAsync(int id, CancellationToken ct) => repo.GetByIdAsync(id, ct);
    public async Task<Product> CreateAsync(ProductCreateDto d, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(d.Name)) throw new ArgumentException("Product name is required."); if (d.Price < 0 || d.Stock < 0) throw new ArgumentException("Price and stock cannot be negative."); return await repo.AddAsync(new Product { Name = d.Name.Trim(), Price = d.Price, Stock = d.Stock, CreatedAt = DateTime.UtcNow }, ct);
    }
    public async Task<bool> UpdateAsync(int id, ProductUpdateDto d, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(d.Name))
            throw new ArgumentException("Product name is required.");
        return await repo.UpdateAsync(new Product { Id = id, Name = d.Name.Trim(), Price = d.Price, Stock = d.Stock }, ct);
    }
    public Task<bool> DeleteAsync(int id, CancellationToken ct) =>
        repo.DeleteAsync(id, ct);

    public async Task<IEnumerable<Product>> GetProductsAsEnumerableAsync(CancellationToken cancellationToken)
    {
        return await repo.GetProductsAsEnumerableAsync(cancellationToken);
    }
    public IQueryable<Product> GetProductsQueryable()
    {
        return repo.GetProductsQueryable();
    }
}
