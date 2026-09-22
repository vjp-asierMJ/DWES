namespace OrderFactoryPattern;

/// <summary>
/// Provides a factory for creating orders based on customer type.
/// </summary>
internal static class OrderFactory
{
    /// <summary>
    /// Creates an order based on the specified customer type and total amount.
    /// </summary>
    /// <param name="id">The unique identifier for the order.</param>
    /// <param name="type">The type of customer placing the order.</param>
    /// <param name="total">The total amount of the order.</param>
    /// <returns>The created order.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Order Create(string id, CustomerType type, decimal total) => type switch
    {
        CustomerType.Regular => new Order(id, type, total, DiscountRate: 0.00m),
        CustomerType.Premium => new Order(id, type, total, DiscountRate: 0.10m),
        CustomerType.Vip => new Order(id, type, total, DiscountRate: 0.20m),
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };
}
