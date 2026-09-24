namespace OrderFactoryPattern;

interface ITrackeable
{
    string GetTrackingUrl();
    void PrintTrackingInfo() => Console.WriteLine($"Track at: {GetTrackingUrl()}");
}

