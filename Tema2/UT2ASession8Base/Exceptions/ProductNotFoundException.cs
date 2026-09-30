namespace NovaWarehouse.Exceptions;

/// <summary>
/// Thrown when an order refers to a product that does not exist in the warehouse.
/// </summary>
internal class ProductNotFoundException : Exception
{
    public string ProductName { get; }

    public ProductNotFoundException(string productName)
        : base($"Product '{productName}' not found.")
    {
        ProductName = productName;
    }
}
