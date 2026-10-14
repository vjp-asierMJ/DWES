using System.Globalization;
using NovaWarehouse.Models;


namespace NovaWarehouse.Repositories;

internal class InFileProductRepository(string path) : IRepository<Product>
{
   private readonly string _path = path;
    public void Add(Product item) => File.AppendAllLines(_path, [ToLine(item)]);
    public Product? GetById(string id) => GetAllItems().FirstOrDefault(o => o.Name == id);
    public IReadOnlyList<Product> GetAllItems()
    {
        if (!File.Exists(_path)) // la primera vez todavía no existe
        {
            return [];
        }
        return File.ReadAllLines(_path).Select(FromLine).ToList();
    }

    private static string ToLine(Product product)
    {
        return string.Join(';', product.Name, product.Price, product.Stock);
    }

    private static Product FromLine(string line)
    {

  string[] fields = line.Split(';');
        Product product = new Product(
        name: fields[0],
        customer: Enum.Parse<CustomerType>(fields[1]),
        total: decimal.Parse(fields[3], CultureInfo.InvariantCulture),
        shipping: Enum.Parse<ShippingType>(fields[2]));
        order.Status = fields[4];
        return order;
    }

}
