namespace NovaWarehouse.Exceptions;

internal class ProductNotFoundException : Exception
{
    public string ProductName { get; }

    public ProductNotFoundException(string productName) 
    : base($"El producto '{productName}' no existe")
    {
        ProductName = productName;

    }
}

