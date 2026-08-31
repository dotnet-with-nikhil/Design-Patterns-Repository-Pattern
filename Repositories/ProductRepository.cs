using DotNet8_Enterprise_CRUD.Data;
using DotNet8_Enterprise_CRUD.Models;
using Microsoft.EntityFrameworkCore;
namespace DotNet8_Enterprise_CRUD.Repositories;

public class ProductRepository(AppDbContext db) : IProductRepository
{

    //Here the method promises: I will return a List<Product>. The caller gets all List<T> capabilities:
    public async Task<List<Product>> GetAllAsync(CancellationToken ct) =>
        await db.Products.AsNoTracking()
        .OrderBy(x => x.Id)
        .ToListAsync(ct);

    public async Task<Product?> GetByIdAsync(int id, CancellationToken ct) =>
        await db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<Product> AddAsync(Product p, CancellationToken ct)
    {
        await db.Products.AddAsync(p, ct); await db.SaveChangesAsync(ct); return p;
    }
    public async Task<bool> UpdateAsync(Product p, CancellationToken ct)
    {
        var x = await db.Products.FirstOrDefaultAsync(x => x.Id == p.Id, ct); if (x is null) return false; x.Name = p.Name; x.Price = p.Price; x.Stock = p.Stock;
        await db.SaveChangesAsync(ct); return true;
    }
    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var x = await db.Products.FirstOrDefaultAsync(x => x.Id == id, ct); if (x is null)
            return false;
        db.Products.Remove(x);
        await db.SaveChangesAsync(ct); return true;
    }

    //Here the method promises:I will return something that can be enumerated.
    //The caller can do: to iterate the iEnumerable collection in the loop
    //The caller can do:
    //    var products =
    //    await repository.GetProductsAsEnumerableAsync(token);

    //foreach (var product in products)
    //{
    //    // ...
    //}

    public async Task<IEnumerable<Product>> GetProductsAsEnumerableAsync(
    CancellationToken cancellationToken)
    {
        var products = await db.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return products;
    }
    public IQueryable<Product> GetProductsQueryable()
    {
        return db.Products.AsQueryable();
    }
}
