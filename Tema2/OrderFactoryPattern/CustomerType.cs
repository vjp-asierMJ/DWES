namespace OrderFactoryPattern;

/// <summary>
/// Represents the type of customer placing an order.
/// </summary>
internal enum CustomerType
{
    /// <summary>
    /// Represents a regular customer with no discount applied to their orders.
    /// </summary>
    Regular,
    /// <summary>
    /// Represents a premium customer with a 10% discount applied to their orders.
    /// </summary>
    Premium,
    /// <summary>
    /// Represents a VIP customer with a 20% discount applied to their orders.
    /// </summary>
    Vip,
}
