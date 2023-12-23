using CatsApp.Domain;
using CatsApp.Domain.Aggregates;
using CatsApp.Domain.Persistence;
using Dapper;
using System.Data;

namespace CatsApp.Infrastructure.Persistence;

public class DapperCatContext : ICatContext
{
    protected IDbConnection _connection;

    public DapperCatContext(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<int> CreateAsync(Cat cat, CancellationToken cancellationToken)
    {
        string commandText = $"INSERT INTO cats (name, weight, age) VALUES (@name, @weight, @age) RETURNING id";
        var queryArguments = new
        {
            name = cat.Name,
            weight = cat.Weight,
            age = cat.Age
        };

        return await _connection.QueryFirstOrDefaultAsync<int>(commandText, queryArguments);
    }

    public async Task DeleteAsync(Cat cat, CancellationToken cancellationToken)
    {
        string commandText = $"DELETE FROM cats WHERE id=@id";
        var queryArguments = new { id = cat.Id };
        await _connection.ExecuteAsync(commandText, queryArguments);
    }

    public async ValueTask<Cat?> ReadAsync(int catId, CancellationToken cancellationToken)
    {
        string commandText = $"SELECT * FROM cats WHERE id=@id";
        var queryArguments = new { id = catId };
        return await _connection.QueryFirstOrDefaultAsync<Cat>(commandText, queryArguments);
    }

    public async Task SaveAsync(Cat cat, CancellationToken cancellationToken)
    {
        string commandText = $"UPDATE cats SET name=@name, weight=@weight, age=@age WHERE id=@id";
        var queryArguments = new
        {
            name = cat.Name,
            weight = cat.Weight,
            age = cat.Age,
            id = cat.Id
        };

        await _connection.ExecuteAsync(commandText, queryArguments);
    }

    public async Task<Page<Cat?>> SearchAsync(string searchText, int pageNum, int pageSize, CancellationToken cancellationToken)
    {
        string commandText = $"SELECT * FROM cats WHERE LOWER(name) " +
            $"LIKE '%{searchText}%' {GetAdditionalCondition(searchText)} " +
            $"ORDER BY id LIMIT {pageSize + 1} OFFSET {pageSize * (pageNum - 1)}";

        var cats = await _connection.QueryAsync<Cat?>(commandText);
        var count = cats.Count();
        Page<Cat?> catPage = new()
        {
            IsLast = count <= pageSize,
            Content = count <= pageSize ? cats : cats.SkipLast(1)
        };

        return catPage;
    }

    private string GetAdditionalCondition(string searchText)
    {
        string condition = string.Empty;
        if (int.TryParse(searchText, out int intValue))
        {
            condition = $"OR age={intValue} OR weight={intValue}";
        }
        else if (double.TryParse(searchText, out double doubleValue))
        {
            condition = $"OR weight={doubleValue}";
        }

        return condition;
    }
}
