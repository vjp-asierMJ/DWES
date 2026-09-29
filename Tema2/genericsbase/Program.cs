using Generics;
using Generics.Models;

Product bracket = new("Steel Bracket", 4.25m);
Product box = new("Cardboard Box", 0.80m);
Product wrap = new("Pallet Wrap", 12.50m);
Product helmet = new("Safety Helmet", 18.90m);

// Un cliente navega por la tienda: guardamos los 3 últimos productos que ha visto...
RecentItems<Product> recentProducts = new(capacity: 3);
recentProducts.Add(bracket);
recentProducts.Add(box);
recentProducts.Add(wrap);
recentProducts.Add(bracket); // lo vuelve a ver: sube arriba, sin duplicarse
recentProducts.Add(helmet); // ya no cabe: sale el más antiguo (box)

// ...y sus 3 últimas búsquedas
RecentItems<string> recentSearches = new(capacity: 3);
recentSearches.Add("helmet");
recentSearches.Add("box");
recentSearches.Add("helmet");
recentSearches.Add("wrap");
recentSearches.Add("bracket");

PrintAllRecentItems<Product>(recentProducts);
PrintAllRecentItems<string>(recentSearches);

//Ejercicio pedidos consultados
RecentItems<Order> recentOrders = new(capacity: 2);
recentOrders.Add(new Order("ORD-001", 125.50m));
recentOrders.Add(new Order("ORD-002", 89.90m));
recentOrders.Add(new Order("ORD-003", 210.00m));

PrintAllRecentItems<Order>(recentOrders);
//


void PrintAllRecentItems<T>(RecentItems<T> recent)
{
    Console.WriteLine($"Vistos recientemente ({recent.Items.Count}/{recent.Capacity}):");
    foreach (T item in recent.Items)
    {
        Console.WriteLine($"  - {item}");
    }
}
