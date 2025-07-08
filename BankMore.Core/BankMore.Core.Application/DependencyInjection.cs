using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BankMore.Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services, Assembly assembly)
    {
        services
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
            });

        return services;
    }
}
