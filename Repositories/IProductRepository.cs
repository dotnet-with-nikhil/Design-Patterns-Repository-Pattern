using DotNet8_Enterprise_CRUD.Models;
namespace DotNet8_Enterprise_CRUD.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync(CancellationToken ct);
    Task<Product?> GetByIdAsync(int id, CancellationToken ct);
    Task<Product> AddAsync(Product p, CancellationToken ct);
    Task<bool> UpdateAsync(Product p, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<IEnumerable<Product>> GetProductsAsEnumerableAsync(
    CancellationToken cancellationToken);
    IQueryable<Product> GetProductsQueryable();
}
