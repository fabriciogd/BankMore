using BankMore.Account.Domain.Services.Interfaces;
using BankMore.Core.Domain.Primitives;
using MediatR;

namespace BankMore.Account.Application.UseCases.Account.GetActive;

public sealed class AccountGetActiveRequestHandler(ICheckingAccountService service) : IRequestHandler<AccountGetActiveRequest, Result<AccountGetActiveResponse>>
{
    public async Task<Result<AccountGetActiveResponse>> Handle(AccountGetActiveRequest request, CancellationToken cancellationToken)
    {
        var checkingAccountResponse = await service.GetValidCheckingAccountAsync(request.NumberAccount);

        if (!checkingAccountResponse.IsSuccess)
            return checkingAccountResponse.Error;

        return new AccountGetActiveResponse()
        {
            CheckingAccountId = checkingAccountResponse.Value.Id,
            NumberAccount = checkingAccountResponse.Value.NumberAccount,
            Name = checkingAccountResponse.Value.Name,
            NationalDocument = checkingAccountResponse.Value.NationalDocument,
        };
    }
}
