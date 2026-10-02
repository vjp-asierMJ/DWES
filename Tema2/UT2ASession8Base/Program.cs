using NovaWarehouse;
using NovaWarehouse.Exceptions;
using NovaWarehouse.Models;
using NovaWarehouse.Repositories;



IRepository<Order> _repository = new InFileOrderRepository("orders.csv");

Warehouse warehouse = new("NovaWarehouse",_repository);
warehouse.AddProduct(new Product("Steel Bracket", 4.25m, 120));
warehouse.AddProduct(new Product("Cardboard Box", 0.80m, 500));
warehouse.AddProduct(new Product("Pallet Wrap", 12.50m, 30));
warehouse.AddProduct(new Product("Safety Helmet", 18.90m, 15));

// customer y shipping son parámetros opcionales: si no los pasamos, se usan sus valores por defecto
// warehouse.PlaceOrder("Cardboard Box", quantity: 100, CustomerType.Premium, ShippingType.Express);
// warehouse.PlaceOrder("Steel Bracket", quantity: 20);

Console.WriteLine($"=== {warehouse.Name.ToUpper()} ===");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1. Ver inventario");
    Console.WriteLine("2. Crear pedido");
    Console.WriteLine("3. Listar pedidos");
    Console.WriteLine("4. Buscar pedido");
    Console.WriteLine("5. Estadisticas");
    Console.WriteLine("0. Salir");
    Console.Write("Seleccione una opción: ");

    switch (Console.ReadLine())
    {
        case "1":
            ListProducts();
            break;

        case "2":
            // Los catch van del más específico al más genérico: Exception siempre el último
            try
            {
                CreateOrder();
            }
            catch (InsufficientStockException ex)
            {
                Console.WriteLine($"Pedido rechazado: {ex.Requested} x '{ex.ProductName}', solo hay {ex.Available}.");
            }
            catch (ProductNotFoundException ex)
            {
                Console.WriteLine($"Pedido rechazado: no existe el producto '{ex.ProductName}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Procesamiento del pedido finalizado.");
            }
            break;

        case "3":
            ListOrders();
            break;

        case "4":
            FindOrder();
            break;

        case "5":
            ShowStats();
            break;

         case "0":
            return;

        default:
            Console.WriteLine("Opción inválida.");
            break;
    }
}

void ShowStats()
        //Numero de pedidos por tipo de cliente (GroupBy)
{       var ordersGrouped =  warehouse.Orders.GroupBy(o => o.Customer).Select(g => new {CustomerType = g.Key, Size = g.Count()});
       
       foreach (var group in ordersGrouped)
    {
        group.CustomerType
        group.Size
    }
       
       //order de mayor importe
        var order = warehouse.Orders.OrderByDescending(o => o.Total).First();
    
}


void ListProducts()
{
    Console.WriteLine("=== INVENTARIO ===");
    Console.WriteLine($"{"",1} {"Nombre",-15} {"Precio",8} {"Stock",6}");
    Console.WriteLine(new string('-', 33));

    foreach (Product product in warehouse.Products)
    {
        string flag = product.Stock < 50 ? "⚠" : " ";
        Console.WriteLine($"{flag} {product.Name,-15} {product.Price,8:C2} {product.Stock,6}");
    }

    Console.WriteLine($"\nProductos con poco stock: {warehouse.LowStockCount}");
    Console.WriteLine($"Valor total del inventario: {warehouse.TotalInventoryValue():C2}");
}

void CreateOrder()
{
    Console.WriteLine("=== NUEVO PEDIDO ===");

    Console.Write("Producto: ");
    string productName = Console.ReadLine() ?? "";

    Console.Write("Cantidad: ");
    if (!int.TryParse(Console.ReadLine(), out int quantity))
    {
        Console.WriteLine("Cantidad inválida.");
        return;
    }

    Console.Write("Tipo de cliente (Regular, Premium, Vip): ");
    if (!Enum.TryParse(Console.ReadLine(), out CustomerType customer))
    {
        Console.WriteLine("Tipo de cliente inválido.");
        return;
    }

    Console.Write("Tipo de envío (Standard, Express, Bulk): ");
    if (!Enum.TryParse(Console.ReadLine(), out ShippingType shipping))
    {
        Console.WriteLine("Tipo de envío inválido.");
        return;
    }

    // using: Dispose() cierra el fichero al salir del método, también si PlaceOrder lanza una excepción
    using var log = new StreamWriter("orders.log", append: true);
    log.WriteLine($"{DateTime.Now:s} REQUEST  {quantity} x {productName}");

    Order order = warehouse.PlaceOrder(productName, quantity, customer, shipping);
    log.WriteLine($"{DateTime.Now:s} ACCEPTED {order.Id}");

    Console.WriteLine($"Pedido {order.Id} creado. Total: {order.Total:C2}");
}

void ListOrders()
{
    if (warehouse.Orders.Count == 0)
    {
        Console.WriteLine("No hay pedidos para mostrar.");
        return;
    }

    Console.WriteLine("=== PEDIDOS ===");
    foreach (Order order in warehouse.Orders)
    {
        Console.WriteLine($"{order.Id} ({order.GetType().Name}) Cliente: {order.Customer}, Total: {order.Total:C2}, Descuento: {order.DiscountRate:P0}, Estado: {order.Status}");

        if (order is ITrackable trackable)
        {
            Console.WriteLine($"    Seguimiento: {trackable.GetTrackingUrl()}");
        }
    }
}


void FindOrder()
{
    Console.WriteLine("Introduce le id del programa");

    string? idOrder = Console.ReadLine();

    if(idOrder is null)
    {
        Console.Write($"La orden con id {idOrder} no existe");
        return;
    }
    

    Order? order = warehouse.FindOrder(idOrder);

    if (order is null)
    {
        Console.Write($"La orden con el id {idOrder} no existe");
        return;
    }

    Console.WriteLine($"{order.Id} ({order.GetType().Name}) Cliente: {order.Customer}, Total: {order.Total:C2}, Descuento: {order.DiscountRate:P0}, Estado: {order.Status}");

}
