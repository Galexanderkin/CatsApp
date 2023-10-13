using CatsApp.Domain.Aggregates;
using CatsApp.Domain.Persistence;
using MediatR;

namespace CatsApp.Application.Commands.Handlers;

public class CreateCatCommandHandler : IRequestHandler<CreateCatCommand, int>
{
    private readonly ICatContext _catContext;

    public CreateCatCommandHandler(ICatContext catsContext)
    {
        _catContext = catsContext;
    }

    public async Task<int> Handle(CreateCatCommand request, CancellationToken cancellationToken)
    {
        return await _catContext.CreateAsync(new Cat()
        { 
            Name = request.Name,
            Weight= request.Weight, 
            Age = request.Age
        }, cancellationToken);
    }
}
