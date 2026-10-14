using NovaWarehouse.Exceptions;
using NovaWarehouse.Models;
using NovaWarehouse.Repositories;

namespace NovaWarehouse;

/// <summary>
/// Manages the products in stock and the orders placed against them.
/// It does not know where they are stored: it works with any <see cref="IRepository{T}"/>.
/// </summary>
internal class Warehouse(string name, IRepository<Product> products, IRepository<Order> orders)
{
    public string Name { get; init; } = name;

    private readonly IRepository<Product> _products = products;
    private readonly IRepository<Order> _orders = orders;

    public IReadOnlyList<Product> Products => _products.GetAll();
    public IReadOnlyList<Order> Orders => _orders.GetAll();

    public int LowStockCount => Products.Count(p => p.Stock < 50);

    public Product GetProduct(string name) => 
        _products.GetById(name) ?? throw new ProductNotFoundException(name);

    public void AddProduct(Product product) => _products.Add(product);

    public decimal TotalInventoryValue() =>
        Products.Sum(p => p.Price * p.Stock);

    public Order? FindOrder(string id) => _orders.GetById(id);

    /// <summary>
    /// Creates an order for <paramref name="quantity"/> units of a product and takes them out of stock.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="quantity"/> is zero or negative.</exception>
    /// <exception cref="ProductNotFoundException">The product does not exist in the warehouse.</exception>
    /// <exception cref="InsufficientStockException">There are not enough units in stock.</exception>
    public Order PlaceOrder(OrderBuilder orderBuilder)
    {
        string id = $"ORD-{_orders.GetAll().Count + 1:D4}";
        Order order = orderBuilder.Build(id);

        // if (quantity <= 0)
        // {
        //     throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        // }

        // Product? product = _products.GetById(productName);

        // if (product is null)
        // {
        //     throw new ProductNotFoundException(productName);
        // }

        // if (product.Stock < quantity)
        // {
        //     throw new InsufficientStockException(productName, quantity, product.Stock);
        // }

        // // Con InFileOrderRepository el contador continúa desde los pedidos guardados en ejecuciones anteriores
        // string id = $"ORD-{_orders.GetAll().Count + 1:D4}";
        // Order order = OrderFactory.Create(id, customer, product.Price * quantity, shipping);

        // // Solo modificamos el estado cuando ya no puede fallar nada:
        // // si algo lanza antes, el almacén no queda a medias (stock descontado sin pedido)
        // product.Stock -= quantity;
        // _products.Update(product); // con fichero, product es una copia: hay que guardarlo
        // _orders.Add(order);

        return order;
    }
}
