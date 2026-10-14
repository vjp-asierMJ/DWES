namespace NovaWarehouse.Models;

internal class BulkOrder : Order
{
    public BulkOrder(string id, CustomerType customer, List<OrderLine> lines) : base(id, customer,lines) { }

    public override decimal CalculateShippingCost(double weightKg) => 15.00m;
}
