using CatsApp.Domain;
using CatsApp.Domain.Aggregates;

namespace CatsApp.Domain.Persistence
{
    public interface ICatContext
    {
        Task<int> CreateAsync(Cat cat, CancellationToken cancellationToken);
        Task DeleteAsync(Cat cat, CancellationToken cancellationToken);
        ValueTask<Cat?> ReadAsync(int id, CancellationToken cancellationToken);
        Task SaveAsync(Cat cat, CancellationToken cancellationToken);
        Task<Page<Cat?>> SearchAsync(string searchText, int pageNum, int pageSize, CancellationToken cancellationToken);
    }
}