namespace NovaWarehouse.Models;

internal class StandardOrder : Order, ITrackable
{
    public StandardOrder(string id, CustomerType customer, List<OrderLine> lines) : base(id, customer, lines) { }

    public override decimal CalculateShippingCost(double weightKg) =>
        3.50m + (decimal)weightKg * 0.80m;

    public string GetTrackingUrl() => $"https://novawarehouse.example/track/{Id}";
}
