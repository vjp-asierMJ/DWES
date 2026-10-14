namespace NovaWarehouse.Exceptions;

/// <summary>
/// Thrown when an order requests more units of a product than are available in stock.
/// </summary>
internal class InsufficientStockException : Exception
{
    public string ProductName { get; }
    public int Requested { get; }
    public int Available { get; }

    public InsufficientStockException(string productName, int requested, int available)
        : base($"Not enough stock for '{productName}': requested {requested}, available {available}.")
    {
        ProductName = productName;
        Requested = requested;
        Available = available;
    }
}
