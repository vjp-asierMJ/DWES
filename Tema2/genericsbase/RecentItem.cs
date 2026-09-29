// using Generics.Models;

namespace Generics;

internal class RecentItems<T>
{
    private readonly List<T> _items = [];

    public RecentItems(int capacity)
    {
        Capacity = capacity;
    }

    public int Capacity { get; }
    public IReadOnlyList<T> Items => _items;

    public void Add(T item)
    {
        _items.Remove(item); // si ya estaba, lo quitamos para no duplicarlo...
        _items.Insert(0, item); // ...y lo ponemos el primero: el más reciente

        if (_items.Count > Capacity)
        {
            _items.RemoveAt(_items.Count - 1); // si no cabe, sale el más antiguo
        }
    }
}
