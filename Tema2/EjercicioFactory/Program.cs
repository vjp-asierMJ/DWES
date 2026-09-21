using EjercicioFactory;
using EjercicioFactory.Models;


Order order = new Order("hola",CustomerType.Regular,50.0m,0.0m);
Order order2 = new Order("hola2",CustomerType.Vip,50.0m,0.2m);

Order orderFromFactory = OrderFactory.Create("hola3", CustomerType.Premium, 100.00m);

Console.WriteLine($"El descuento de {orderFromFactory.Id} es de {orderFromFactory.DiscountRate}");