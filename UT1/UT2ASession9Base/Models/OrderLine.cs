namespace NovaWarehouse.Models;

internal record OrderLine(string ProductName, int Quantity, decimal UnitPrice)
{
public decimal Subtotal => Quantity * UnitPrice;
}