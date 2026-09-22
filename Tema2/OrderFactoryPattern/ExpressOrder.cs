namespace OrderFactoryPattern;

class ExpressOrder : Order
{
    public ExpressOrder(string id, CustomerType customer, decimal total) : base(id, customer, total)
    {
    }

    public override decimal CalculateShippingCost(double weightKg)
    {
        return 8.00m + (decimal)weightKg * 1.20m;
    }
}