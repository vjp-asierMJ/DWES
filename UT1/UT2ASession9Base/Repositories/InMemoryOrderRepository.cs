using NovaWarehouse.Models;

namespace NovaWarehouse.Repositories;

/// <summary>
/// Keeps the orders in a list in memory: fast and simple, but they are lost when the application closes.
/// </summary>
internal class InMemoryOrderRepository : IRepository<Order>
{
    // Es la misma lista que antes tenía Warehouse: la hemos movido aquí, no reescrito
    private readonly List<Order> _orders = [];

    public void Add(Order item) => _orders.Add(item);

    public void Update(Order item)
    {
        int index = _orders.FindIndex(o => o.Id == item.Id);

        if (index >= 0)
        {
            _orders[index] = item;
        }
    }

    public Order? GetById(string id) => _orders.FirstOrDefault(o => o.Id == id);

    public IReadOnlyList<Order> GetAll() => _orders;
}
