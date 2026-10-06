namespace NovaWarehouse.Models;

/// <summary>
/// Represents the shipping method of an order. The <see cref="OrderFactory"/> uses it
/// to decide which <see cref="Order"/> subclass to create.
/// </summary>
internal enum ShippingType
{
    Standard,
    Express,
    Bulk,
}
