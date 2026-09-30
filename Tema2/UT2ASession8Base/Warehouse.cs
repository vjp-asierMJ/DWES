using NovaWarehouse.Exceptions;
using NovaWarehouse.Models;

namespace NovaWarehouse;

/// <summary>
/// Manages the products in stock and the orders placed against them.
/// </summary>
internal class Warehouse(string name)
{
    public string Name { get; init; } = name;

    private readonly List<Product> _products = [];
    private readonly List<Order> _orders = [];

    public IReadOnlyList<Product> Products => _products;
    public IReadOnlyList<Order> Orders => _orders;

    public int LowStockCount => _products.Count(p => p.Stock < 50);

    public void AddProduct(Product product) => _products.Add(product);

    public decimal TotalInventoryValue() =>
        _products.Sum(p => p.Price * p.Stock);

    /// <summary>
    /// Creates an order for <paramref name="quantity"/> units of a product and takes them out of stock.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="quantity"/> is zero or negative.</exception>
    /// <exception cref="ProductNotFoundException">The product does not exist in the warehouse.</exception>
    /// <exception cref="InsufficientStockException">There are not enough units in stock.</exception>
    public Order PlaceOrder(string productName, int quantity,
        CustomerType customer = CustomerType.Regular, ShippingType shipping = ShippingType.Standard)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        }

        Product? product = _products.FirstOrDefault(p => p.Name == productName);

        if (product is null)
        {
            throw new ProductNotFoundException(productName);
        }

        if (product.Stock < quantity)
        {
            throw new InsufficientStockException(productName, quantity, product.Stock);
        }

        string id = $"ORD-{_orders.Count + 1:D4}";
        Order order = OrderFactory.Create(id, customer, product.Price * quantity, shipping);

        // Solo modificamos el estado cuando ya no puede fallar nada:
        // si algo lanza antes, el almacén no queda a medias (stock descontado sin pedido)
        product.Stock -= quantity;
        _orders.Add(order);

        return order;
    }
}
