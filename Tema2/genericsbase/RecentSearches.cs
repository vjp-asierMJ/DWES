// using Generics.Models;

namespace Generics;

/// <summary>
/// Keeps the last searches made by a customer, most recent first.
/// </summary>
internal class RecentSearches
{
    private readonly List<string> _items = [];

    public RecentSearches(int capacity)
    {
        Capacity = capacity;
    }

    public int Capacity { get; }
    public IReadOnlyList<string> Items => _items;

    public void Add(string item)
    {
        _items.Remove(item); // si ya estaba, lo quitamos para no duplicarlo...
        _items.Insert(0, item); // ...y lo ponemos el primero: el más reciente

        if (_items.Count > Capacity)
        {
            _items.RemoveAt(_items.Count - 1); // si no cabe, sale el más antiguo
        }
    }
}
