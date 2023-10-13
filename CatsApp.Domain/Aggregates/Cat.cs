namespace CatsApp.Domain.Aggregates;

public record class Cat
{
    public const double MinWeight = 0.1;
    public const double MaxWeight = 20.0;
    public const int MinAge = 0;
    public const int MaxAge = 20;
    public const int MaxNameLength = 20;
    public int Id { get; set; }
    public string Name { get; set; }
    public double Weight { get; set; }
    public int Age { get; set; }
}
