using BankMore.Core.Application.Iterfaces;
using BankMore.Core.Application.Services;
using BankMore.Core.Infraestructure.Auth;
using BankMore.Core.Infraestructure.Constants;
using BankMore.Core.Infraestructure.Contracts;
using BankMore.Core.Infraestructure.Database;
using BankMore.Core.Infraestructure.HttpClients.Handlers;
using BankMore.Core.Infraestructure.Models;
using BankMore.Core.Infraestructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankMore.Core.Infraestructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSqliteContext(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString(ConnectionString.Default)!;

        services.AddScoped<IDbConnectionFactory>(provider =>
            new SqliteConnectionFactory(connectionString));

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<AuthenticatedHttpClientHandler>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IUserIdentity, UserIdentity>();

        return services;
    }

    public static IServiceCollection AddIdempotencyService(this IServiceCollection services)
    {
        services.AddScoped<IIdempotencyService, RedisIdempotencyService>();

        return services;
    }
}
