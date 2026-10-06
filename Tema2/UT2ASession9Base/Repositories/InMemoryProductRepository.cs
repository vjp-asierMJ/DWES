using NovaWarehouse.Models;

namespace NovaWarehouse.Repositories;

// La misma interfaz genérica sirve para otra entidad

/// <summary>
/// Keeps the products in a list in memory. A product is identified by its name.
/// </summary>
internal class InMemoryProductRepository : IRepository<Product>
{
    private readonly List<Product> _products = [];

    public void Add(Product item) => _products.Add(item);

    // En memoria, GetById devuelve el mismo objeto que guarda la lista (tipo referencia):
    // el stock ya se ha modificado en la propia lista y aquí solo queda sustituirlo por sí mismo
    public void Update(Product item)
    {
        int index = _products.FindIndex(p => p.Name == item.Name);

        if (index >= 0)
        {
            _products[index] = item;
        }
    }

    public Product? GetById(string id) => _products.FirstOrDefault(p => p.Name == id);

    public IReadOnlyList<Product> GetAll() => _products;
}
