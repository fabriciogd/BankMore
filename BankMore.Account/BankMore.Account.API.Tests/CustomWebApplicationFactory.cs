using BankMore.Core.Application.Iterfaces;
using BankMore.Core.Infraestructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;
using Testcontainers.Redis;

namespace BankMore.Account.API.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly RedisContainer _redisContainer;

    public CustomWebApplicationFactory()
    {
        _redisContainer = new RedisBuilder()
              .WithImage("redis:7.0")
              .Build();
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(IConnectionMultiplexer));

            services.RemoveAll(typeof(IDbConnectionFactory));

            services.TryAddSingleton<IDbConnectionFactory>(new SqliteConnectionFactory("Data Source=Teste.db;Mode=ReadWriteCreate;Cache=Shared;Pooling=false;"));

            services.TryAddSingleton<IConnectionMultiplexer>(
               _ => ConnectionMultiplexer.Connect(_redisContainer.GetConnectionString())
           );
        });

        base.ConfigureWebHost(builder);
    }

    public async Task InitializeAsync()
    {
        await _redisContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _redisContainer.StopAsync();

        if (File.Exists("Teste.db"))
            File.Delete("Teste.db");
    }
}
