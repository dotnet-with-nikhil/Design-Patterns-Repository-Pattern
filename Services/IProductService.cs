using DotNet8_Enterprise_CRUD.DTOs;
using DotNet8_Enterprise_CRUD.Models;
namespace DotNet8_Enterprise_CRUD.Services;

public interface IProductService
{
    Task<List<Product>> GetAllAsync(CancellationToken ct);
    Task<Product?> GetByIdAsync(int id, CancellationToken ct);
    Task<Product> CreateAsync(ProductCreateDto dto, CancellationToken ct);
    Task<bool> UpdateAsync(int id, ProductUpdateDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<IEnumerable<Product>> GetProductsAsEnumerableAsync(CancellationToken cancellationToken);
    IQueryable<Product> GetProductsQueryable();
}
