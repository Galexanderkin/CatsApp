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

    public Task<Page<Cat>> SearchAsync(string searchText, int pageNum, int pageSize, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
