using NovaWarehouse.Models;
using NovaWarehouse.Exceptions;
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

    public Order PlaceOrder(string productName, int quantity,
        CustomerType customer = CustomerType.Regular, ShippingType shipping = ShippingType.Standard)
    {
        Product? product = _products.First(p => p.Name == productName);

        if (product.Stock < quantity)
        {
            throw new InsufficientStockException(productName, quantity, product.Stock);
        }

        if (product is null)
        {
            throw new ProductNotFoundException(productName);
        }
        product.Stock -= quantity;

        string id = $"ORD-{_orders.Count + 1:D4}";
        Order order = OrderFactory.Create(id, customer, product.Price * quantity, shipping);
        _orders.Add(order);

        return order;
    }
}
