namespace NovaWarehouse.Models;

internal class ExpressOrder : Order, ITrackable
{
    public ExpressOrder(string id, CustomerType customer, List<OrderLine> lines) : base(id, customer,lines) { }

    public override decimal CalculateShippingCost(double weightKg) =>
        8.00m + (decimal)weightKg * 1.20m;

    public string GetTrackingUrl() => $"https://novawarehouse.example/track/{Id}";
}
