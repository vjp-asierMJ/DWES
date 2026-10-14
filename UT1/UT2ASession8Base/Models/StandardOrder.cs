namespace NovaWarehouse.Models;

internal class StandardOrder : Order, ITrackable
{
    public StandardOrder(string id, CustomerType customer, decimal total) : base(id, customer, total) { }

    public override decimal CalculateShippingCost(double weightKg) =>
        3.50m + (decimal)weightKg * 0.80m;

    public string GetTrackingUrl() => $"https://novawarehouse.example/track/{Id}";
}
