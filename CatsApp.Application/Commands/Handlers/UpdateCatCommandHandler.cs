using CatsApp.Application.Exceptions;
using CatsApp.Domain.Persistence;
using MediatR;

namespace CatsApp.Application.Commands.Handlers;

public class UpdateCatCommandHandler : IRequestHandler<UpdateCatCommand>
{
    private readonly ICatContext _catContext;

    public UpdateCatCommandHandler(ICatContext catsRepository)
    {
        _catContext = catsRepository;
    }

    public async Task Handle(UpdateCatCommand request, CancellationToken cancellationToken)
    {
        var cat = await _catContext.ReadAsync(request.Id, cancellationToken);
        if (cat == null)
        {
            throw new CatNotFoundException();
        }

        cat.Name = request.Name;
        cat.Age = request.Age;
        cat.Weight = request.Weight;
        await _catContext.SaveAsync(cat, cancellationToken);
    }
}
