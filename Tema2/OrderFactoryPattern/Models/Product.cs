namespace NovaWarehouse.Models;

/// <summary>
/// Represents a product stored in the warehouse.
/// </summary>
internal class Product(string name, decimal price, int stock)
{
    public string Name { get; init; } = name;
    public decimal Price { get; init; } = price;

    private int _stock = stock;
    public int Stock
    {
        get => _stock;
        set => _stock = value >= 0
            ? value
            : throw new ArgumentOutOfRangeException(nameof(value), "Stock cannot be negative.");
    }
}
