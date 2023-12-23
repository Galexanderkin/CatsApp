using CatsApp.Domain.Persistence;
using EasyNetQ;
using EasyNetQ.Topology;

namespace CatsApp.Infrastructure.Persistence;

public class MqRabbitMessageProducer : IMessageProducer
{
    private readonly IBus _bus;
    public MqRabbitMessageProducer(IBus bus)
    {
        _bus = bus;   
    }

    public Task PublishAsync<T>(T message)
    {
        return _bus.Advanced.PublishAsync(new Exchange("CatExchange"), "cat.routing.key", false, new Message<T>(message));
    }
}
