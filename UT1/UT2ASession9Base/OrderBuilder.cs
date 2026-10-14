namespace NovaWarehouse;
using NovaWarehouse;
using NovaWarehouse.Models;

class OrderBuilder
{
    private CustomerType _customer = CustomerType.Regular;
    private ShippingType _shippingType = ShippingType.Standard;

    private List<OrderLine> _lines = []; 

    public  OrderBuilder ForCustomer(CustomerType customerType)
    {
        _customer = customerType;
        return this;
    }

    public OrderBuilder WithShipping(ShippingType shippingType)
    {
        _shippingType = shippingType;
        return this;
    } 

    public OrderBuilder AddLine(Product product, int quantity)
    {
        _lines.Add(new OrderLine(product.Name, quantity, product.Price));

        return this;

    }

    public Order Build(string id)
    {
        return OrderFactory.Create(id, _customer, _lines, _shippingType);
    }
}
