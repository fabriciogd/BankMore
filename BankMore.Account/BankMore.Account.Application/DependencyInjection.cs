using BankMore.Account.Application.UseCases.Account.Create;
using BankMore.Account.Application.UseCases.Movement.Create;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BankMore.Account.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<AccountCreateRequest>, AccountCreateRequestValidator>();
        services.AddScoped<IValidator<MovementCreateRequest>, MovementCreateRequestValidator>();

        return services;
    }
}
