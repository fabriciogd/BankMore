using BankMore.Transfer.Application.UseCases.Transfer.Create;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BankMore.Transfer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<TransferCreateRequest>, TransferCreateRequestValidator>();

        return services;
    }
}
