using BankMore.Account.Application.Errors;
using BankMore.Account.Domain.Repositories;
using BankMore.Core.Domain.Primitives;
using BankMore.Core.Infraestructure.Contracts;
using MediatR;

namespace BankMore.Account.Application.UseCases.Movement.Balance;

internal sealed class MovementBalanceRequestHandler(
    ICheckingAccountRepository accountRepository,
    IMovementRepository movementRepository,
    IUserIdentity userIdentity
) : IRequestHandler<MovementBalanceRequest, Result<MovementBalanceResponse>>
{
    public async Task<Result<MovementBalanceResponse>> Handle(MovementBalanceRequest request, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetByNumberAccountAsync(userIdentity.NumberAccount);

        if (account is null)
            return AccountErrors.NotFound;

        if (!account.IsActive)
            return AccountErrors.IsInactive;

        var balance = await movementRepository.GetBalanceAsync(account.Id);

        return new MovementBalanceResponse()
        {
            NumberAccount = account.NumberAccount,
            Name = account.Name,
            Date = DateTime.Now,
            Balance = balance
        };
    }
}
