using NovaWarehouse.Models;
namespace NovaWarehouse.Repositories;


internal class InMemoryProductRepository : IRepository<Product>
{

    private readonly List<Product> _products = [];
    
    public void Add(Product item)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<Product> GetAllItems()
    {
        throw new NotImplementedException();
    }

    public Product? GetById(string id)
    {
        throw new NotImplementedException();
    }
}