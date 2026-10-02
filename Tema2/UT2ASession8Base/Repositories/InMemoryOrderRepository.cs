using NovaWarehouse.Models;
namespace NovaWarehouse.Repositories;


internal class InMemoryOrderRepository : IRepository<Order>
{

    private readonly List<Order> _orders = [];
    public void Add(Order item)
    {
        _orders.Add(item);
    }

    public IReadOnlyList<Order> GetAllItems()
    {
        return _orders;
    }
    }

    public Order? GetById(string id)
    {
        Order? orderFound = null;
        foreach (var order in _orders)
        {
            if (order.Id == id)
            {
               return order;
            }
        }
        return null;
    }
}