namespace NovaWarehouse.Repositories;

public interface IRepository<T> where T : class
{
    void Add(T item);
    T? GetById(string id);
    IReadOnlyList<T> GetAllItems();
}