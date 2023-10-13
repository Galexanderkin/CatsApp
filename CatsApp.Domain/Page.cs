using CatsApp.Domain.Aggregates;

namespace CatsApp.Domain;

public record class Page<T>
{
    public const int MinSize = 1;

    public const int MaxSize = 10;

    public bool IsLast { get; init; }

    public IEnumerable<T> Content { get; init; }
}
