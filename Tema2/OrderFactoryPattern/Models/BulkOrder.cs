namespace NovaWarehouse.Models;

internal class BulkOrder : Order
{
    public BulkOrder(string id, CustomerType customer, decimal total) : base(id, customer, total) { }

    public override decimal CalculateShippingCost(double weightKg) => 15.00m;
}
