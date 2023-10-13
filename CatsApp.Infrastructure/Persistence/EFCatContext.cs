using CatsApp.Domain;
using CatsApp.Domain.Aggregates;
using CatsApp.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CatsApp.Infrastructure.Persistence;

public class EFCatContext : DbContext, ICatContext
{
    protected DbSet<Cat> Cats { get; set; }

    public EFCatContext(DbContextOptions<EFCatContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public ValueTask<Cat?> ReadAsync(int id, CancellationToken cancellationToken)
    {
        return Cats.FindAsync(id, cancellationToken);
    }

    public async Task<Page<Cat>> SearchAsync(string searchText, int pageNum, int pageSize, CancellationToken cancellationToken)
    {       
        IQueryable<Cat> filter = Cats;
        if (!string.IsNullOrEmpty(searchText))
        {
            if (int.TryParse(searchText, out int intValue))
            {
                filter = Cats.Where(cat => cat.Weight == intValue
                || cat.Age == intValue
                || cat.Name.Contains(searchText));
            }
            else if (double.TryParse(searchText, out double doubleValue))
            {
                filter = Cats.Where(cat => cat.Weight == doubleValue
                || cat.Name.Contains(searchText));
            }
            else
            {
                filter = Cats.Where(cat => cat.Name.Contains(searchText));
            }
        }
        
        var cats = await filter.OrderBy(x => x.Id).Skip(pageSize * (pageNum - 1)).Take(pageSize + 1).ToListAsync(cancellationToken);
        Page<Cat> catPage = new() 
        { 
            IsLast = cats.Count <= pageSize,
            Content = cats.Count <= pageSize ? cats : cats.SkipLast(1)
        };

        return catPage;
    }

    public async Task<int> CreateAsync(Cat cat, CancellationToken cancellationToken)
    {
        Cats.Add(cat);
        await SaveChangesAsync(cancellationToken);

        return cat.Id;
    }

    public Task DeleteAsync(Cat cat, CancellationToken cancellationToken)
    {
        Cats.Remove(cat);

        return SaveChangesAsync(cancellationToken);
    }

    public Task SaveAsync(Cat cat, CancellationToken cancellationToken)
    {
        return SaveChangesAsync(cancellationToken);
    }
}
