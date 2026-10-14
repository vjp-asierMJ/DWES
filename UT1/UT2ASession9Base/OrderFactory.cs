using NovaWarehouse.Models;

namespace NovaWarehouse;

/// <summary>
/// Creates the concrete <see cref="Order"/> subclass for each shipping type.
/// </summary>
internal static class OrderFactory
{
    public static Order Create(string id, CustomerType customer, List<OrderLine> lines, ShippingType shipping) => shipping switch
    {
        ShippingType.Standard => new StandardOrder(id, customer, lines),
        ShippingType.Express => new ExpressOrder(id, customer, lines),
        ShippingType.Bulk => new BulkOrder(id, customer, lines),
        _ => throw new ArgumentOutOfRangeException(nameof(shipping))
    };
}
