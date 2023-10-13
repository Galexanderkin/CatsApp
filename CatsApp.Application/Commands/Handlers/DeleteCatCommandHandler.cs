using CatsApp.Application.Exceptions;
using CatsApp.Domain.Persistence;
using MediatR;

namespace CatsApp.Application.Commands.Handlers;

public class DeleteCatCommandHandler : IRequestHandler<DeleteCatCommand>
{
    private readonly ICatContext _catContext;

    public DeleteCatCommandHandler(ICatContext catsContext)
    {
        _catContext = catsContext;
    }

    public async Task Handle(DeleteCatCommand request, CancellationToken cancellationToken)
    {
        var cat = await _catContext.ReadAsync(request.Id, cancellationToken);
        if (cat == null)
        {
            throw new CatNotFoundException();
        }

        await _catContext.DeleteAsync(cat, cancellationToken);
    }
}
