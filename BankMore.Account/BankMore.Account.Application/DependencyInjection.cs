using BankMore.Account.Application.UseCases.Account.Create;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BankMore.Account.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<AccountCreateRequest>, AccountCreateRequestValidator>();

        return services;
    }
}
