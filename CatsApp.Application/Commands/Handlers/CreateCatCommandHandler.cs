using CatsApp.Domain.Aggregates;
using CatsApp.Domain.Persistence;
using MediatR;

namespace CatsApp.Application.Commands.Handlers;

public class CreateCatCommandHandler : IRequestHandler<CreateCatCommand, int>
{
    private readonly ICatContext _catContext;
    private readonly IMessageProducer _messageProducer;

    public CreateCatCommandHandler(ICatContext catsContext, IMessageProducer messageProducer)
    {
        _catContext = catsContext;
        _messageProducer = messageProducer;
    }

    public async Task<int> Handle(CreateCatCommand request, CancellationToken cancellationToken)
    {
        var catId = await _catContext.CreateAsync(new Cat()
        { 
            Name = request.Name,
            Weight= request.Weight, 
            Age = request.Age
        }, cancellationToken);
        // await _messageProducer.PublishAsync<string>($"Cat created with {catId}");

        return catId;
    }
}
