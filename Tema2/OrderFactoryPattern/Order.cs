namespace OrderFactoryPattern;
abstract class Order
{
    public string Id { get; init; }
    public CustomerType Customer { get; init; }
    public string Status { get; set; } = "Pending";
    public decimal Total { get; init; }
    public decimal DiscountRate { get; init; }
    public Order(string id, CustomerType customer, decimal total)
    {
        Id = id;
        Customer = customer;
        Total = total;
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
