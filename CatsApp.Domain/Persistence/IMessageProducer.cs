
namespace CatsApp.Domain.Persistence;

public interface IMessageProducer
{
    Task PublishAsync<T>(T message);
}
