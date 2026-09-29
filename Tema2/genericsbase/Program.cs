using Generics;
using Generics.Models;

Product bracket = new("Steel Bracket", 4.25m);
Product box = new("Cardboard Box", 0.80m);
Product wrap = new("Pallet Wrap", 12.50m);
Product helmet = new("Safety Helmet", 18.90m);

// Un cliente navega por la tienda: guardamos los 3 últimos productos que ha visto...
RecentProducts recentProducts = new(capacity: 3);
recentProducts.Add(bracket);
recentProducts.Add(box);
recentProducts.Add(wrap);
recentProducts.Add(bracket); // lo vuelve a ver: sube arriba, sin duplicarse
recentProducts.Add(helmet); // ya no cabe: sale el más antiguo (box)

// ...y sus 3 últimas búsquedas
RecentSearches recentSearches = new(capacity: 3);
recentSearches.Add("helmet");
recentSearches.Add("box");
recentSearches.Add("helmet");
recentSearches.Add("wrap");
recentSearches.Add("bracket");

PrintProducts(recentProducts);
PrintSearches(recentSearches);

void PrintProducts(RecentProducts recent)
{
    Console.WriteLine($"Vistos recientemente ({recent.Items.Count}/{recent.Capacity}):");
    foreach (Product item in recent.Items)
    {
        Console.WriteLine($"  - {item}");
    }
}

void PrintSearches(RecentSearches recent)
{
    Console.WriteLine($"Búsquedas recientes ({recent.Items.Count}/{recent.Capacity}):");
    foreach (string item in recent.Items)
    {
        Console.WriteLine($"  - {item}");
    }
}
