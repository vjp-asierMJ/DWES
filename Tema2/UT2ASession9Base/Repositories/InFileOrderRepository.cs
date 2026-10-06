using System.Globalization;
using NovaWarehouse.Models;

namespace NovaWarehouse.Repositories;

/// <summary>
/// Keeps the orders in a text file, one per line, so they survive between executions.
/// </summary>
/// <example><c>ORD-0001;Premium;Express;80.00;Pending</c> (Id;Customer;Shipping;Total;Status)</example>
internal class InFileOrderRepository(string path) : IRepository<Order>
{
    private const char Separator = ';';

    private readonly string _path = path;

    // File.AppendAllLines y File.ReadAllLines abren y cierran el fichero en cada llamada: no hace falta using
    public void Add(Order item) => File.AppendAllLines(_path, [ToLine(item)]);

    // Reescribe el fichero entero, sustituyendo solo la línea de ese pedido
    public void Update(Order item) =>
        File.WriteAllLines(_path, GetAll().Select(o => o.Id == item.Id ? item : o).Select(ToLine));

    public Order? GetById(string id) => GetAll().FirstOrDefault(o => o.Id == id);

    public IReadOnlyList<Order> GetAll()
    {
        // La primera vez que se ejecuta la aplicación el fichero todavía no existe
        if (!File.Exists(_path))
        {
            return [];
        }

        return File.ReadAllLines(_path).Select(FromLine).ToList();
    }

    private static string ToLine(Order order)
    {
        // El fichero guarda el tipo de envío para poder recrear la subclase correcta al leer
        ShippingType shipping = order switch
        {
            StandardOrder => ShippingType.Standard,
            ExpressOrder => ShippingType.Express,
            BulkOrder => ShippingType.Bulk,
            _ => throw new ArgumentOutOfRangeException(nameof(order))
        };

        // InvariantCulture: el total se escribe siempre con punto (80.00), sin depender del idioma del equipo
        return string.Join(Separator,
            order.Id,
            order.Customer,
            shipping,
            order.Total.ToString(CultureInfo.InvariantCulture),
            order.Status);
    }

    private static Order FromLine(string line)
    {
        string[] fields = line.Split(Separator);

        // Reutilizamos la Factory de la Sesión 6: ella sabe qué subclase crear
        Order order = OrderFactory.Create(
            id: fields[0],
            customer: Enum.Parse<CustomerType>(fields[1]),
            total: decimal.Parse(fields[3], CultureInfo.InvariantCulture),
            shipping: Enum.Parse<ShippingType>(fields[2]));
        order.Status = Enum.Parse<OrderStatus>(fields[4]);

        return order;
    }
}
