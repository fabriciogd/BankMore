using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BankMore.Core.API.Base;

public abstract class BaseController: ControllerBase
{
    protected static async Task ValidateAndThrowAsync<TValidator, TRequest>(TRequest request)
        where TRequest : class
        where TValidator : AbstractValidator<TRequest>, new()
    {
        var validator = new TValidator();

        var result = await validator.ValidateAsync(request);

        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }
}
