using BankMore.Tariff.Consumer.Interfaces;
using BankMore.Tariff.Consumer.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BankMore.Tariff.Consumer;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITariffRepository, TariffRepository>();

        return services;
    }
}
