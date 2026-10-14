using NovaWarehouse.Models;

namespace NovaWarehouse.Repositories;

// Queda abierto para UT6 (EF Core + base de datos en Docker).
// Cuando esté implementado, Warehouse no cambia: solo la línea de Program que elige el repositorio

/// <summary>
/// Will keep the orders in a database table.
/// </summary>
internal class DbOrderRepository : IRepository<Order>
{
    // INSERT INTO Orders ...
    public void Add(Order item) => throw new NotImplementedException();

    // UPDATE Orders SET ... WHERE Id = @id
    public void Update(Order item) => throw new NotImplementedException();

    // SELECT ... FROM Orders WHERE Id = @id
    public Order? GetById(string id) => throw new NotImplementedException();

    // SELECT ... FROM Orders
    public IReadOnlyList<Order> GetAll() => throw new NotImplementedException();
}
