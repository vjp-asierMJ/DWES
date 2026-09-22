using System.Diagnostics.CodeAnalysis;
using OrderFactoryPattern;

List<Order> _orders = [
    // Ojo, como no uso el Factory he puesto los descuentos a mano;
    // lo ideal es usar el OrderFactory.Create para crear los pedidos.
    new StandarOrder("ORD-0001", CustomerType.Regular, 100.00m),
    new ExpressOrder("ORD-0002", CustomerType.Premium, 200.00m),
    new StandarOrder("ORD-0003", CustomerType.Vip, 300.00m),
];

Console.WriteLine("=== ORDER FACTORY ===");

while (true)
{
    Console.WriteLine("1. Crear pedido");
    Console.WriteLine("2. Listar pedidos");
    Console.WriteLine("3. Salir");
    Console.Write("Seleccione una opción: ");

    string? option = Console.ReadLine();
    switch (option)
    {
        case "1":
            Console.WriteLine("=== NUEVO PEDIDO ===");

            Console.Write("Introduzca el ID del pedido: ");
            string? id = Console.ReadLine();

            Console.Write("Introduzca el tipo de cliente (Regular, Premium, Vip): ");
            string? customerType = Console.ReadLine();

            Console.Write("Introduzca la cantidad total del pedido: ");
            string? totalInput = Console.ReadLine();

            if (!TryCreateOrder(id, customerType, totalInput, out Order? order, out string? error))
            {
                Console.WriteLine(error);
                continue;
            }

            _orders.Add(order);
            Console.WriteLine($"Pedido creado: {order.Id}, Descuento: {order.DiscountRate}");
            break;

        case "2":
            ListOrders();
            break;

        case "3":
            return;

        default:
            Console.WriteLine("Opción inválida.");
            break;
    }
}

void ListOrders()
{
    if (_orders.Count == 0)
    {
        Console.WriteLine("No hay pedidos para mostrar.");
        return;
    }

    Console.WriteLine("=== PEDIDOS ===");
    foreach (var order in _orders)
    {
        Console.WriteLine($"ID: {order.Id}, Descuento: {order.DiscountRate}");
    }
}

// C# tiene mecaniusmos como los llamados Attributes:
// NotNullWhen es un atributo que indica que el parámetro de salida "order" no será nulo cuando el método devuelva true.
bool TryCreateOrder(string? id, string? customerType, string? totalInput,
    [NotNullWhen(true)] out Order? order, out string? error)
{
    order = null;
    error = null;

    if (string.IsNullOrWhiteSpace(id))
    {
        error = "ID del pedido inválido.";
        return false;
    }

    if (!Enum.TryParse(customerType, out CustomerType type))
    {
        error = "Tipo de cliente inválido.";
        return false;
    }

    if (!decimal.TryParse(totalInput, out var total))
    {
        error = "Cantidad total inválida.";
        return false;
    }

    order = OrderFactory.Create(id, type, total);

    return true;
}
