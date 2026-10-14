namespace NovaWarehouse.Models;

/// <summary>
/// Represents an order with its details.
/// </summary>
internal abstract class Order
{
    public string Id { get; init; }
    public CustomerType Customer { get; init; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal Total => OrderLines.Sum(l => l.Subtotal);
    public decimal DiscountRate { get; init; }

    public List<OrderLine> OrderLines { get; init; }= [];

    public Order(string id, CustomerType customer, List<OrderLine> lines)
    {
        Id = id;
        Customer = customer;
        OrderLines = lines;
        DiscountRate = customer switch
        {
            CustomerType.Regular => 0.00m,
            CustomerType.Premium => 0.10m,
            CustomerType.Vip => 0.20m,
            _ => throw new ArgumentOutOfRangeException(nameof(customer))
        };
    }

    public abstract decimal CalculateShippingCost(double weightKg);
}
