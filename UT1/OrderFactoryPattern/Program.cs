using NovaWarehouse;
using NovaWarehouse.Models;
using NovaWarehouse.Exceptions;
Warehouse warehouse = new("NovaWarehouse");
warehouse.AddProduct(new Product("Steel Bracket", 4.25m, 120));
warehouse.AddProduct(new Product("Cardboard Box", 0.80m, 500));
warehouse.AddProduct(new Product("Pallet Wrap", 12.50m, 30));
warehouse.AddProduct(new Product("Safety Helmet", 18.90m, 15));

// customer y shipping son parámetros opcionales: si no los pasamos, se usan sus valores por defecto
warehouse.PlaceOrder("Cardboard Box", quantity: 100, CustomerType.Premium, ShippingType.Express);
warehouse.PlaceOrder("Steel Bracket", quantity: 20);

Console.WriteLine($"=== {warehouse.Name.ToUpper()} ===");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1. Ver inventario");
    Console.WriteLine("2. Crear pedido");
    Console.WriteLine("3. Listar pedidos");
    Console.WriteLine("4. Salir");
    Console.Write("Seleccione una opción: ");

    switch (Console.ReadLine())
    {
        case "1":
            ListProducts();
            break;

        case "2":
            try
            {
                CreateOrder(); // pide los datos por consola y llama a warehouse.PlaceOrder(...)
            }
            catch (InsufficientStockException ex)
            {
                // Console.WriteLine($"Pedido rechazado: {ex.Message}");
                Console.WriteLine(@$"Producto: {ex.ProductName}, 
                solicitado: {ex.Requested}, 
                
                disponible: {ex.Available}");
            }
            catch (ProductNotFoundException)
            {
                Console.WriteLine("Producto no encontrado");
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
            return;

        default:
            Console.WriteLine("Opción inválida.");
            break;
    }
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

    using var log = new StreamWriter("orders.log", append: true);
    log.WriteLine($"{DateTime.Now:s} REQUEST {quantity} x {productName}");

    Order order = warehouse.PlaceOrder(productName, quantity, customer, shipping);
    log.WriteLine($"{DateTime.Now:s} ACCEPTED {order.Id}");
    // Dispose() automático al salir del método, también si PlaceOrder lanza
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
