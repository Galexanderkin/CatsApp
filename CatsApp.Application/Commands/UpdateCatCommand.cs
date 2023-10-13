using MediatR;

namespace CatsApp.Application.Commands;

public class UpdateCatCommand : IRequest
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public double Weight { get; init; }
    public int Age { get; init; }
}
