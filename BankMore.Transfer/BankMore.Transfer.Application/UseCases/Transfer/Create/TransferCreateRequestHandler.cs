using BankMore.Core.Domain.Primitives;
using BankMore.Core.EventBus.Contracts;
using BankMore.Core.EventBus.Models;
using BankMore.Core.Infraestructure.Contracts;
using BankMore.Transfer.Application.Errors;
using BankMore.Transfer.Domain.Constants;
using BankMore.Transfer.Domain.Events;
using BankMore.Transfer.Domain.External.Account;
using BankMore.Transfer.Domain.External.Account.Request;
using BankMore.Transfer.Domain.Repostories;
using MediatR;

namespace BankMore.Transfer.Application.UseCases.Transfer.Create;

internal sealed class TransferCreateRequestHandler(
    IAccountMovementApiService accountMovementApiService,
    ITransferRepository transferRepository,
    IUserIdentity userIdentity,
    IPublisherEndpoint publisherEndpoint
) : IRequestHandler<TransferCreateRequest, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(TransferCreateRequest request, CancellationToken cancellationToken)
    {
        if (userIdentity.NumberAccount == request.DestinationNumberAccount)
            return TransferErrors.InvalidDestinationAccount;

        var registerDebitResponse = await accountMovementApiService.RegisterMovementAsync(new AccountMovementRequestApi()
        {
            NumberAccount = userIdentity.NumberAccount,
            Type = MovementType.Debit,
            Value = request.Value
        });

        if (!registerDebitResponse.IsSuccess)
            return Unit.Value;

        var registerCreditResponse = await accountMovementApiService.RegisterMovementAsync(new AccountMovementRequestApi()
        {
            NumberAccount = request.DestinationNumberAccount,
            Type = MovementType.Credit,
            Value = request.Value
        });

        if (!registerDebitResponse.IsSuccess)
        {
            await accountMovementApiService.RegisterMovementAsync(new AccountMovementRequestApi()
            {
                NumberAccount = userIdentity.NumberAccount,
                Type = MovementType.Debit,
                Value = request.Value
            });
        }

        var transfer = Domain.Entities.Transfer.Create(
            registerDebitResponse.Value.CheckingAccountId, 
            registerCreditResponse.Value.CheckingAccountId,
            DateTime.Now,
            request.Value);

        await transferRepository.AddAsync(transfer, cancellationToken);

        var transferEvent = new TransferEvent()
        {
            CheckingAccountId = 1,
            Date = DateTime.Now,
            Value = 1
        };

        await publisherEndpoint.PublishAsync("tariff-queue", new Body(transferEvent), cancellationToken);

        return Unit.Value;
    }
}
