namespace NovaWarehouse.Exceptions;

internal class InsufficientStockException : Exception
{
    public string ProductName { get; }
    public int Requested { get; }
    public int Available { get; }

    public InsufficientStockException(string productName, int requested, int available) 
    : base($"Insufficient stock for product '{productName}': requested {requested}, available {available}")
    {
        ProductName = productName;
        Requested = requested;
        Available = available;
    }
}