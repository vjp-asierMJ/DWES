using System;

Console.WriteLine("Hello, World!");

string nombre = "Aimar";

Console.WriteLine($"Hola mi nombre es{nombre}");

var quantity = 120;

Persona persona = new();

var persona2 = new Persona();

// Casa 

bool isExpress = false;

Console.Write("Package weight (kg): ");
string? input = Console.ReadLine();
double weightKg = double.Parse(input!);

const double baseCost = 3.50;
const double costPerKg = 0.80;
double totalCost = baseCost + (weightKg * costPerKg);

Console.WriteLine("¿Pedido prioritario por 5 € s/n?");
input = Console.ReadLine();

if (input == "s")
{
    isExpress = true;
}

if (isExpress)
{
    totalCost += 5;
}

Console.WriteLine($"Shipping cost: {totalCost:C2}");


//Sesion 2 Guard Clause

decimal CalculateDiscount(Customer customer, Order order)
{
    if (customer == null)
    {
        throw new ArgumentNullException(nameof(customer), "Customer is null");
    }

    if (order == null)
    {
        throw new ArgumentNullException(nameof(order), "Order is null");
    }

    decimal discount = 0;

    if (!customer.Active)
    {
        Console.WriteLine("Inactive customer");
        return discount;
    }

    if (order.Total <= 0)
    {
        Console.WriteLine("Order has no total");
        return discount;
    }

    if (customer.IsVip)
    {
        if (order.Total > 1000)
        {
            discount = order.Total * 0.20m;
        }
        else
        {
            discount = order.Total * 0.10m;
        }
    }
    else
    {
        discount = order.Total * 0.05m;
    }

    return discount;
}

//Ejemplo validar cantidad de producto + practicar en casa

Console.Write("Enter quantity for SKU-4471: ");
string? rawQuantity = Console.ReadLine();
string? rawPeso;

if (!int.TryParse(rawQuantity, out quantity) || quantity <= 0 || quantity > 500)
{
    Console.WriteLine("Invalid quantity. Order line rejected.");
    return;
}

if (quantity > 500)
{
    Console.WriteLine("Invalid quantity. Order line rejected.");
    return;
}

Console.Write("Enter weight for SKU-4471: ");
string? rawWeight = Console.ReadLine();


Console.Write("Peso del articulo");
rawPeso = Console.ReadLine();

if (!double.TryParse(rawPeso, out double peso) || peso <= 0)
{
    Console.WriteLine("Peso invalido");
    return;
}
Console.WriteLine($"Added {quantity} units of SKU-4471 to the order.");


// Casa

//1.Añade un caso para pedidos con Total == 0 , y clasifícalo como "Free order"
//  if/else clásico: equivalente al switch expression anterior
string Classify(Order order)
{
    if (order.Status == "Cancelled") return "Ignore";
    if (order.Total > 1000 && order.Status == "Pending") return "Priority review";
    if (order.Total > 1000) return "High value";
    if (order.Status == "Pending") return "Awaiting confirmation";
    if (order.Total < 100) return "Free order";
    return "Standard";
}

//2.Reescribe el descuento por CustomerType añadiendo un 5% extra si Total > 500 (usa un patrón detupla (CustomerType, decimal) )
decimal GetDiscount(Order order)
{
    return (order.CustomerType, order.Total) switch
    {
        ("VIP", > 500) => 0.25m,
        ("VIP", _) => 0.20m,

        ("Regular", > 500) => 0.15m,
        ("Regular", _) => 0.10m,

        ("New", > 500) => 0.10m,
        ("New", _) => 0.05m,

        _ => 0m
    };
}

//Sesion 1
class Persona { }

//Sesion 2

class Customer
{
    public bool Active { get; internal set; }
    public bool IsVip { get; internal set; }
}
class Order
{
    public int Total { get; internal set; }
    public string Status { get; internal set; }
    public string CustomerType { get; set; }
}