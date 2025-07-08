using BankMore.Core.Application.Iterfaces;
using DbUp;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Reflection;

namespace BankMore.Core.Infraestructure.Database;

public sealed class SqliteConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqliteConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> CreateOpenConnectionAsync()
    {
        var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(); // Abre a conexão de forma assíncrona
        return connection;
    }

    public async Task RunMigrationsAsync(Assembly assembly)
    {
        using var connection = await CreateOpenConnectionAsync();

        var upgrader = DeployChanges.To
            .SqliteDatabase(_connectionString)
            .WithScriptsEmbeddedInAssembly(assembly)
            .LogToConsole()
            .Build();

        upgrader.PerformUpgrade();
    }
}
