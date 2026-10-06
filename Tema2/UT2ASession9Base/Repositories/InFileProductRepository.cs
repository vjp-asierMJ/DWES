using System.Globalization;
using NovaWarehouse.Models;

namespace NovaWarehouse.Repositories;

// Si solo guardamos los pedidos, el stock vuelve a su valor inicial
// en cada ejecución. Productos y pedidos tienen que guardarse en el mismo sitio

/// <summary>
/// Keeps the products in a text file, one per line, so the stock survives between executions.
/// A product is identified by its name.
/// </summary>
/// <example><c>Steel Bracket;4.25;120</c> (Name;Price;Stock)</example>
internal class InFileProductRepository(string path) : IRepository<Product>
{
    private const char Separator = ';';

    private readonly string _path = path;

    public void Add(Product item) => File.AppendAllLines(_path, [ToLine(item)]);

    // Reescribe el fichero entero, sustituyendo solo la línea de ese producto
    public void Update(Product item) =>
        File.WriteAllLines(_path, GetAll().Select(p => p.Name == item.Name ? item : p).Select(ToLine));

    public Product? GetById(string id) => GetAll().FirstOrDefault(p => p.Name == id);

    public IReadOnlyList<Product> GetAll()
    {
        if (!File.Exists(_path))
        {
            return [];
        }

        return File.ReadAllLines(_path).Select(FromLine).ToList();
    }

    private static string ToLine(Product product) =>
        string.Join(Separator,
            product.Name,
            product.Price.ToString(CultureInfo.InvariantCulture),
            product.Stock);

    private static Product FromLine(string line)
    {
        string[] fields = line.Split(Separator);

        return new Product(
            name: fields[0],
            price: decimal.Parse(fields[1], CultureInfo.InvariantCulture),
            stock: int.Parse(fields[2]));
    }
}
