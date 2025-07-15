using BankMore.Account.Domain.Errors;
using BankMore.Account.Domain.Constants;
using BankMore.Account.Domain.Entities;
using BankMore.Account.Domain.Repositories;
using BankMore.Core.Domain.Primitives;
using BankMore.Core.Infraestructure.Contracts;
using MediatR;
using BankMore.Account.Domain.Services.Interfaces;

namespace BankMore.Account.Application.UseCases.Movement.Create;

public sealed class MovementCreateRequestHandler(
    ICheckingAccountService service,
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

        var checkingAccountResponse = await service.GetValidCheckingAccountAsync(numberAccount);

        if (!checkingAccountResponse.IsSuccess)
            return checkingAccountResponse.Error;

        var checkingAccount = checkingAccountResponse.Value;

        var movement = MovementAccount.Create(checkingAccount.Id, request.Type, request.Value);

        await movementRepository.AddAsync(movement, cancellationToken);

        return new MovementCreateResponse()
        {
            CheckingAccountId = checkingAccount.Id,
            NumberAccount = checkingAccount.NumberAccount,
            Type = request.Type,
            Value = request.Value,
        };
    }
}
