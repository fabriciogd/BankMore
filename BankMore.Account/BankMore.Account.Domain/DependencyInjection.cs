using BankMore.Account.Domain.Services;
using BankMore.Account.Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BankMore.Account.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ICheckingAccountService, CheckingAccountService>();

        return services;
    }
}