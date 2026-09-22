namespace OrderFactoryPattern;

class StandarOrder : Order
{
    public StandarOrder(string id, CustomerType customer, decimal total) : base(id, customer, total)
    {
    }

    public override decimal CalculateShippingCost(double weightKg)
    {
        return 3.50m + (decimal)weightKg * 0.80m;
    }
}