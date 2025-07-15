using BankMore.Account.Domain.Repositories;
using BankMore.Account.Domain.Services.Interfaces;
using BankMore.Core.Domain.Primitives;
using BankMore.Core.Infraestructure.Contracts;
using MediatR;

namespace BankMore.Account.Application.UseCases.Movement.Balance;

public sealed class MovementBalanceRequestHandler(
    ICheckingAccountService service,
    IMovementRepository movementRepository,
    IUserIdentity userIdentity
) : IRequestHandler<MovementBalanceRequest, Result<MovementBalanceResponse>>
{
    public async Task<Result<MovementBalanceResponse>> Handle(MovementBalanceRequest request, CancellationToken cancellationToken)
    {
        var checkingAccountResponse = await service.GetValidCheckingAccountAsync(userIdentity.NumberAccount);

        if (!checkingAccountResponse.IsSuccess)
            return checkingAccountResponse.Error;

        var checkingAccount = checkingAccountResponse.Value;

        var balance = await movementRepository.GetBalanceAsync(checkingAccount.Id);

        return new MovementBalanceResponse()
        {
            NumberAccount = checkingAccount.NumberAccount,
            Name = checkingAccount.Name,
            Date = DateTime.Now,
            Balance = balance
        };
    }
}
