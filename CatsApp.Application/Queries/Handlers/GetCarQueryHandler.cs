using CatsApp.Application.Exceptions;
using CatsApp.Domain.Aggregates;
using CatsApp.Domain.Persistence;
using MediatR;

namespace CatsApp.Application.Queries.Handlers;

public class GetCarQueryHandler : IRequestHandler<GetCatQuery, Cat>
{
    private readonly ICatContext _catsContext;

    public GetCarQueryHandler(ICatContext catsContext)
    {
        _catsContext = catsContext;
    }

    public async Task<Cat> Handle(GetCatQuery request, CancellationToken cancellationToken)
    {
        var cat = await _catsContext.ReadAsync(request.Id, cancellationToken);
        if (cat == null)
        {
            throw new CatNotFoundException();
        }

        return cat;
    }
}
