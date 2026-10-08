using NovaWarehouse;
using NovaWarehouse.Exceptions;
using NovaWarehouse.Models;
using NovaWarehouse.Repositories;

// Program decide DÓNDE se guardan los pedidos; Warehouse solo conoce la interfaz IRepository<Order>.
// Cambiar de almacenamiento es cambiar esta línea:
IRepository<Order> orderRepository = new InFileOrderRepository("orders.csv");
// IRepository<Order> orderRepository = new InMemoryOrderRepository();
// IRepository<Order> orderRepository = new DbOrderRepository(); // UT6

// Productos y pedidos, en el mismo sitio. Si solo se guardan los pedidos,
// el stock vuelve a su valor inicial en cada ejecución
IRepository<Product> productRepository = new InFileProductRepository("products.csv");
// IRepository<Product> productRepository = new InMemoryProductRepository();

Warehouse warehouse = new("NovaWarehouse", productRepository, orderRepository);

// Con fichero, el catálogo y los pedidos de ejemplo ya estarán guardados desde la primera ejecución
if (warehouse.Products.Count == 0)
{
    warehouse.AddProduct(new Product("Steel Bracket", 4.25m, 120));
    warehouse.AddProduct(new Product("Cardboard Box", 0.80m, 500));
    warehouse.AddProduct(new Product("Pallet Wrap", 12.50m, 30));
    warehouse.AddProduct(new Product("Safety Helmet", 18.90m, 15));
}

if (warehouse.Orders.Count == 0)
{
    // customer y shipping son parámetros opcionales: si no los pasamos, se usan sus valores por defecto
    warehouse.PlaceOrder("Cardboard Box", quantity: 100, CustomerType.Premium, ShippingType.Express);
    warehouse.PlaceOrder("Steel Bracket", quantity: 20);
}

Console.WriteLine($"=== {warehouse.Name.ToUpper()} ===");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1. Ver inventario");
    Console.WriteLine("2. Crear pedido");
    Console.WriteLine("3. Listar pedidos");
    Console.WriteLine("4. Buscar pedido");
    Console.WriteLine("5. Estadísticas");
    Console.WriteLine("6. Salir");
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
            SearchOrder();
            break;

        case "5":
            ShowStatistics();
            break;

        case "6":
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

    OrderBuilder orderBuilder = new();

    // Console.Write("Producto: ");
    // string productName = Console.ReadLine() ?? "";

    // Console.Write("Cantidad: ");
    // if (!int.TryParse(Console.ReadLine(), out int quantity))
    // {
    //     Console.WriteLine("Cantidad inválida.");
    //     return;
    // }

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

    orderBuilder
        .ForCustomer(customer)
        .WithShipping(shipping);

    string addMoreProducts = "";
    
    do
    {
        Console.Write("Producto: ");
        
        Product product = warehouse.GetProduct(Console.ReadLine() ?? "");

        Console.Write("Cantidad: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity))
        {
            Console.WriteLine("Cantidad inválida.");
            return;
        }

        orderBuilder.AddLine(product,quantity);


    } while (Console.ReadLine()?.ToLower() == "s");

    Order order = warehouse.PlaceOrder(orderBuilder);

    // using: Dispose() cierra el fichero al salir del método, también si PlaceOrder lanza una excepción
    // using var log = new StreamWriter("orders.log", append: true);
    // log.WriteLine($"{DateTime.Now:s} REQUEST  {quantity} x {productName}");

    

    // Order order = warehouse.PlaceOrder(productName, quantity, customer, shipping);
    // log.WriteLine($"{DateTime.Now:s} ACCEPTED {order.Id}");

    Console.WriteLine($"Pedido {order.Id} creado. Total: {order.Total:C2}");
}

void ListOrders()
{
    // Leemos los pedidos una sola vez: con InFileOrderRepository cada GetAll() vuelve a leer el fichero
    IReadOnlyList<Order> orders = warehouse.Orders;

    if (orders.Count == 0)
    {
        Console.WriteLine("No hay pedidos para mostrar.");
        return;
    }

    Console.WriteLine("=== PEDIDOS ===");
    foreach (Order order in orders)
    {
        PrintOrder(order);
    }
}

void SearchOrder()
{
    Console.Write("Id del pedido: ");
    string id = Console.ReadLine() ?? "";

    Order? order = warehouse.FindOrder(id);

    if (order is null)
    {
        Console.WriteLine($"No existe el pedido '{id}'.");
        return;
    }

    PrintOrder(order);
}

void ShowStatistics()
{
    IReadOnlyList<Order> orders = warehouse.Orders;

    // Sin este if, First() lanzaría InvalidOperationException con la lista vacía
    if (orders.Count == 0)
    {
        Console.WriteLine("No hay pedidos para mostrar.");
        return;
    }

    Console.WriteLine("=== ESTADÍSTICAS ===");

    var byCustomer = orders
        .GroupBy(o => o.Customer)
        .Select(g => new { Customer = g.Key, Count = g.Count() });

    Console.WriteLine("Pedidos por tipo de cliente:");
    foreach (var group in byCustomer)
    {
        Console.WriteLine($"    {group.Customer}: {group.Count}");
    }

    Order mostExpensive = orders
        .OrderByDescending(o => o.Total)
        .First();

    Console.WriteLine($"Pedido de mayor importe: {mostExpensive.Id} ({mostExpensive.Total:C2})");

    var byShipping = orders
        .GroupBy(o => o.GetType().Name)
        .Select(g => new { Shipping = g.Key, Total = g.Sum(o => o.Total) });

    Console.WriteLine("Total facturado por tipo de envío:");
    foreach (var group in byShipping)
    {
        Console.WriteLine($"    {group.Shipping}: {group.Total:C2}");
    }
}

void PrintOrder(Order order)
{
    Console.WriteLine($"{order.Id} ({order.GetType().Name}) Cliente: {order.Customer}, Total: {order.Total:C2}, Descuento: {order.DiscountRate:P0}, Estado: {order.Status}");

    if (order is ITrackable trackable)
    {
        Console.WriteLine($"    Seguimiento: {trackable.GetTrackingUrl()}");
    }
}
