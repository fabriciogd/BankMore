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

public sealed class TransferCreateRequestHandler(
    IAccountMovementApiService accountMovementApiService,
    IAccountApiService accountApiService,
    ITransferRepository transferRepository,
    IUserIdentity userIdentity,
    IIdempotency idempotency,
    IPublisherEndpoint publisherEndpoint
) : IRequestHandler<TransferCreateRequest, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(TransferCreateRequest request, CancellationToken cancellationToken)
    {
        if (userIdentity.NumberAccount == request.DestinationNumberAccount)
            return TransferErrors.InvalidDestinationAccount;

        var sourceAccount = await accountApiService.GetActiveAsync(userIdentity.NumberAccount, cancellationToken);

        if (!sourceAccount.IsSuccess)
            return sourceAccount.Error;

        var desinationAccount = await accountApiService.GetActiveAsync(request.DestinationNumberAccount, cancellationToken);

        if (!desinationAccount.IsSuccess)
            return desinationAccount.Error;

        var transfer = Domain.Entities.Transfer.Create(
            sourceAccount.Value.CheckingAccountId,
            desinationAccount.Value.CheckingAccountId,
            request.Value);

        await transferRepository.AddAsync(transfer, cancellationToken);

        var requestDebit = new AccountMovementRequestApi()
        {
            NumberAccount = userIdentity.NumberAccount,
            Type = MovementType.Debit,
            Value = request.Value
        };

        var registerDebitResponse = await accountMovementApiService.RegisterMovementAsync(
            requestDebit, 
            idempotency.Key + MovementType.Debit, 
            cancellationToken);

        if (!registerDebitResponse.IsSuccess)
        {
            transfer.SetStatus(TransferStatusType.Failed);
            await transferRepository.UpdateAsync(transfer);

            return Unit.Value;
        }

        var requestCredit = new AccountMovementRequestApi()
        {
            NumberAccount = request.DestinationNumberAccount,
            Type = MovementType.Credit,
            Value = request.Value
        };

        var registerCreditResponse = await accountMovementApiService.RegisterMovementAsync(
            requestCredit, 
            idempotency.Key + MovementType.Credit, 
            cancellationToken);

        if (!registerCreditResponse.IsSuccess)
        {
            var requestReversal = new AccountMovementRequestApi()
            {
                NumberAccount = userIdentity.NumberAccount,
                Type = MovementType.Credit,
                Value = request.Value
            };

            await accountMovementApiService.RegisterMovementAsync(
                requestReversal, 
                idempotency.Key + MovementType.Refund,
                cancellationToken);

            transfer.SetStatus(TransferStatusType.Refunded);
            await transferRepository.UpdateAsync(transfer);

            return Unit.Value;
        }

        transfer.SetStatus(TransferStatusType.Finished);
        await transferRepository.UpdateAsync(transfer);

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
