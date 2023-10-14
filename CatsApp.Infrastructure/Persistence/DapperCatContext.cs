using CatsApp.Domain;
using CatsApp.Domain.Aggregates;
using CatsApp.Domain.Persistence;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CatsApp.Infrastructure.Persistence;

public class DapperCatContext : ICatContext
{
    private NpgsqlConnection connection;

    public DapperCatContext(IConfiguration configuration)
    {
        connection = new NpgsqlConnection(configuration.GetConnectionString("PostgeSQLConnection"));
        connection.Open();
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

        return await connection.QueryFirstOrDefaultAsync<int>(commandText, queryArguments);
    }

    public async Task DeleteAsync(Cat cat, CancellationToken cancellationToken)
    {
        string commandText = $"DELETE FROM cats WHERE id=@id";
        var queryArguments = new { id = cat.Id };
        await connection.ExecuteAsync(commandText, queryArguments);
    }

    public async ValueTask<Cat?> ReadAsync(int catId, CancellationToken cancellationToken)
    {
        string commandText = $"SELECT * FROM cats WHERE id=@id";
        var queryArguments = new { id = catId };
        return await connection.QueryFirstOrDefaultAsync<Cat>(commandText, queryArguments);
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

        await connection.ExecuteAsync(commandText, queryArguments);
    }

    public async Task<Page<Cat>> SearchAsync(string searchText, int pageNum, int pageSize, CancellationToken cancellationToken)
    {
        string commandText = $"SELECT * FROM cats WHERE LOWER(name) " +
            $"LIKE '%@substring%' {GetAdditionalCondition(searchText)} " +
            $"ORDER BY id LIMIT @limitVal OFFSET @offsetVal";

        var queryArguments = new
        {
            substring = searchText,
            limitVal = pageSize + 1,
            offsetVal = pageSize * (pageNum - 1)
        };
        var cats = await connection.QueryAsync<Cat?>(commandText, queryArguments);
        var count = cats.Count();
        Page<Cat> catPage = new()
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
