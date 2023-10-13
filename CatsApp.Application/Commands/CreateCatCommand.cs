using CatsApp.Domain.Aggregates;
using MediatR;

namespace CatsApp.Application.Commands;

public class CreateCatCommand : IRequest<int>
{
    public string? Name { get; init; }
    public double Weight { get; init; }
    public int Age { get; init; }
}
