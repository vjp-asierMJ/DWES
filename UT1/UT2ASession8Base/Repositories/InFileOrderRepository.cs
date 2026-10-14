using System.Globalization;
using NovaWarehouse.Models;


namespace NovaWarehouse.Repositories;

internal class InFileOrderRepository(string path) : IRepository<Order>
{
    private readonly string _path = path;
    public void Add(Order item) => File.AppendAllLines(_path, [ToLine(item)]);
    public Order? GetById(string id) => GetAllItems().FirstOrDefault(o => o.Id == id);
    public IReadOnlyList<Order> GetAllItems()
    {
        if (!File.Exists(_path)) // la primera vez todavía no existe
        {
            return [];
        }
        return File.ReadAllLines(_path).Select(FromLine).ToList();
    }

    private static string ToLine(Order order)
    {
        ShippingType shipping = order switch
        {
            StandardOrder => ShippingType.Standard,
            ExpressOrder => ShippingType.Express,
            BulkOrder => ShippingType.Bulk,
            _ => throw new ArgumentOutOfRangeException(nameof(order))
        };
        return string.Join(';', order.Id, order.Customer, shipping,
        order.Total.ToString(CultureInfo.InvariantCulture), order.Status);
    }

    private static Order FromLine(string line)
    {
        string[] fields = line.Split(';');
        Order order = OrderFactory.Create(
        id: fields[0],
        customer: Enum.Parse<CustomerType>(fields[1]),
        total: decimal.Parse(fields[3], CultureInfo.InvariantCulture),
        shipping: Enum.Parse<ShippingType>(fields[2]));
        order.Status = fields[4];
        return order;
    }

}
