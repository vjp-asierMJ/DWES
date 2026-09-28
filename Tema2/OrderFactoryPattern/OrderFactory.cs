using NovaWarehouse.Models;

namespace NovaWarehouse;

/// <summary>
/// Creates the concrete <see cref="Order"/> subclass for each shipping type.
/// </summary>
internal static class OrderFactory
{
    public static Order Create(string id, CustomerType customer, decimal total, ShippingType shipping) => shipping switch
    {
        ShippingType.Standard => new StandardOrder(id, customer, total),
        ShippingType.Express => new ExpressOrder(id, customer, total),
        ShippingType.Bulk => new BulkOrder(id, customer, total),
        _ => throw new ArgumentOutOfRangeException(nameof(shipping))
    };
}
