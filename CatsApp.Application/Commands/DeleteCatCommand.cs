using MediatR;

namespace CatsApp.Application.Commands;

public class DeleteCatCommand : IRequest
{
    public int Id { get; init; }
}
