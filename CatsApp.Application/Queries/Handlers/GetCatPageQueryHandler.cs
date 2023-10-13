using CatsApp.Application.Exceptions;
using CatsApp.Domain;
using CatsApp.Domain.Aggregates;
using CatsApp.Domain.Persistence;
using MediatR;

namespace CatsApp.Application.Queries.Handlers;

public class GetCatPageQueryHandler : IRequestHandler<GetCatPageQuery, Page<Cat>>
{
    private readonly ICatContext _catContext;

    public GetCatPageQueryHandler(ICatContext catsContext)
    {
        _catContext = catsContext;
    }

    public async Task<Page<Cat>> Handle(GetCatPageQuery request, CancellationToken cancellationToken)
    {
        var catPage = await _catContext.SearchAsync(request.SearchText, request.PageNum, request.PageSize, cancellationToken);
        if (!catPage.Content.Any())
        {
            throw new CatNotFoundException();
        }

        return catPage;
    }
}
