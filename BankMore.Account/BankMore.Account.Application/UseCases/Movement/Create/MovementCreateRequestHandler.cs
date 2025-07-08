using BankMore.Account.Application.Errors;
using BankMore.Account.Domain.Constants;
using BankMore.Account.Domain.Entities;
using BankMore.Account.Domain.Repositories;
using BankMore.Core.Domain.Primitives;
using BankMore.Core.Infraestructure.Contracts;
using MediatR;

namespace BankMore.Account.Application.UseCases.Movement.Create;

internal sealed class MovementCreateRequestHandler(
    ICheckingAccountRepository accountRepository,
    IMovementRepository movementRepository,
    IUserIdentity userIdentity) : IRequestHandler<MovementCreateRequest, Result<MovementCreateResponse>>
{
    public async Task<Result<MovementCreateResponse>> Handle(MovementCreateRequest request, CancellationToken cancellationToken)
    {
        var numberAccount = request.NumberAccount ?? userIdentity.NumberAccount;

        if (request.Type is not (MovementType.Credit or MovementType.Debit))
            return MovementErrors.InvalidType;

        if (numberAccount != userIdentity.NumberAccount && request.Type == MovementType.Debit)
            return MovementErrors.InvalidType;

        var account = await accountRepository.GetByNumberAccountAsync(numberAccount);

        if (account is null)
            return AccountErrors.NotFound;

        if (!account.IsActive)
            return AccountErrors.IsInactive;

        if (request.Value <= 0)
            return MovementErrors.InvalidValue;

        var movement = MovementAccount.Create(account.Id, request.Type, request.Value);

        await movementRepository.AddAsync(movement, cancellationToken);

        return new MovementCreateResponse()
        {
            CheckingAccountId = account.Id,
            NumberAccount = account.NumberAccount,
            Type = request.Type,
            Value = request.Value,
        };
    }
}
