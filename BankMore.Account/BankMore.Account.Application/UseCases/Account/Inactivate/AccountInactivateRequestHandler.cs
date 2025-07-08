using BankMore.Account.Application.Errors;
using BankMore.Account.Domain.Repositories;
using BankMore.Core.Domain.Primitives;
using BankMore.Core.Infraestructure.Contracts;
using MediatR;

namespace BankMore.Account.Application.UseCases.Account.Inactivate;

internal sealed class AccountInactivateRequestHandler(ICheckingAccountRepository repository, IUserIdentity userIdentity) : IRequestHandler<AccountInactivateRequest, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(AccountInactivateRequest request, CancellationToken cancellationToken)
    {
        var account = await repository.GetByNumberAccountAsync(userIdentity.NumberAccount);

        if (account is null)
            return AccountErrors.NotFound;

        if (!account.IsActive)
            return AccountErrors.IsInactive;

        account.Inactivate();

        await repository.UpdateAsync(account);

        return Unit.Value;
    }
}
