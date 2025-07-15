using BankMore.Account.Domain.Errors;
using BankMore.Account.Domain.Repositories;
using BankMore.Account.Domain.Services.Interfaces;
using BankMore.Core.Domain.Primitives;
using BankMore.Core.Domain.Resources;
using BankMore.Core.Infraestructure.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BankMore.Account.Application.UseCases.Account.Inactivate;

public sealed class AccountInactivateRequestHandler(
    ICheckingAccountRepository repository,
    ICheckingAccountService service,
    IUserIdentity userIdentity,
    ILogger<AccountInactivateRequestHandler> logger) : IRequestHandler<AccountInactivateRequest, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(AccountInactivateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation(LogTemplate.StartHandler, nameof(AccountInactivateRequestHandler));

        var checkingAccountResponse = await service.GetValidCheckingAccountAsync(userIdentity.NumberAccount);

        if (!checkingAccountResponse.IsSuccess)
            return checkingAccountResponse.Error;

        var checkingAccount = checkingAccountResponse.Value;

        checkingAccount.Inactivate();

        await repository.UpdateAsync(checkingAccount);

        logger.LogInformation(LogTemplate.EndHandler, nameof(AccountInactivateRequestHandler), string.Empty);

        return Unit.Value;
    }
}
