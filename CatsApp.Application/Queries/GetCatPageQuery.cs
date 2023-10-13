using CatsApp.Domain;
using CatsApp.Domain.Aggregates;
using MediatR;

namespace CatsApp.Application.Queries;

public class GetCatPageQuery : IRequest<Page<Cat>>
{
    public string? SearchText { get; init; }
    public int PageNum { get; init; }
    public int PageSize { get; init; }
}
