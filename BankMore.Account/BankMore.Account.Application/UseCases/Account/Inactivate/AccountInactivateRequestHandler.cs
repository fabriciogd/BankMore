using BankMore.Account.Application.Errors;
using BankMore.Account.Domain.Repositories;
using BankMore.Core.Domain.Primitives;
using BankMore.Core.Domain.Resources;
using BankMore.Core.Infraestructure.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BankMore.Account.Application.UseCases.Account.Inactivate;

public sealed class AccountInactivateRequestHandler(
    ICheckingAccountRepository repository, 
    IUserIdentity userIdentity,
    ILogger<AccountInactivateRequestHandler> logger) : IRequestHandler<AccountInactivateRequest, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(AccountInactivateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation(LogTemplate.StartHandler, nameof(AccountInactivateRequestHandler));

        var account = await repository.GetByNumberAccountAsync(userIdentity.NumberAccount);

        if (account is null)
        {
            logger.LogWarning(LogTemplate.WarningHandler, nameof(AccountInactivateRequestHandler), AccountErrors.NotFound.Description);

            return AccountErrors.NotFound;
        }

        if (!account.IsActive)
        {
            logger.LogWarning(LogTemplate.WarningHandler, nameof(AccountInactivateRequestHandler), AccountErrors.IsInactive.Description);

            return AccountErrors.IsInactive;
        }

        account.Inactivate();

        await repository.UpdateAsync(account);

        logger.LogInformation(LogTemplate.EndHandler, nameof(AccountInactivateRequestHandler), string.Empty);

        return Unit.Value;
    }
}
