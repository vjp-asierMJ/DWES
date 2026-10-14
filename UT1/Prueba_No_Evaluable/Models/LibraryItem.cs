namespace Prueba_No_Evaluable;

abstract class LibraryItem
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }

    public LibraryItem(string title, string id, int year)
    {
        Title = title;
        Id = id;
        Year = year;
    }

    public string GetDescription()
    {
        return $"Id: {Id}, Título: {Title}, Año: {Year}";
    }

    public class Book()
    {

    }
}