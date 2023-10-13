using CatsApp.Domain.Aggregates;
using MediatR;

namespace CatsApp.Application.Queries;

public class GetCatQuery : IRequest<Cat>
{
    public int Id { get; init; }
}
