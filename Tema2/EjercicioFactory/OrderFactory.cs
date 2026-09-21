using EjercicioFactory.Models;
namespace EjercicioFactory;

class OrderFactory
{
    public static Order Create(string id, CustomerType type, decimal total) => type switch
    {
        CustomerType.Regular => new Order(id, type, total, DiscountRate: 0.00m),
        CustomerType.Premium => new Order(id, type, total, DiscountRate: 0.10m),
        CustomerType.Vip => new Order(id, type, total, DiscountRate: 0.20m),
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };
}

