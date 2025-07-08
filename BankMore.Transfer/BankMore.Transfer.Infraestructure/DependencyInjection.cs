using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using BankMore.Transfer.Domain.External.Account;
using BankMore.Transfer.Infraestructure.External;
using BankMore.Transfer.Domain.Repostories;
using BankMore.Transfer.Infraestructure.Repositories;
using BankMore.Core.Infraestructure.HttpClients.Handlers;

namespace BankMore.Transfer.Infraestructure;

public static class DependencyInjection
{
    public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IAccountMovementApiService, AccountMovementApiService>(cfg => 
            cfg.BaseAddress = new Uri(configuration["AccountApi:Url"])).AddHttpMessageHandler<AuthenticatedHttpClientHandler>();
        ;

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITransferRepository, TransferRepository>();

        return services;
    }
}
