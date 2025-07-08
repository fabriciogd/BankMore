using BankMore.Account.Domain.Repositories;
using BankMore.Account.Infraestructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BankMore.Account.Infraestructure;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICheckingAccountRepository, CheckingAccountRepository>();
        services.AddScoped<IMovementRepository, MovementRepository>();

        return services;
    }
}
