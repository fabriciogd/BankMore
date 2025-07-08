using BankMore.Account.Domain.Entities;
using BankMore.Account.Domain.Repositories;
using BankMore.Core.Domain.Primitives;
using BankMore.Core.Infraestructure.Security;
using MediatR;

namespace BankMore.Account.Application.UseCases.Account.Create;

internal sealed class AccountCreateRequestHandler(ICheckingAccountRepository repository)
    : IRequestHandler<AccountCreateRequest, Result<AccountCreateResponse>>
{
    public async Task<Result<AccountCreateResponse>> Handle(AccountCreateRequest request, CancellationToken cancellationToken)
    {
        var hashedPassword = PasswordHasher.HashPassword(request.Password, out string salt);

        var account = CheckingAccount.Create(request.NationalDocument, request.Name, hashedPassword, salt);

        await repository.AddAsync(account, cancellationToken);

        return new AccountCreateResponse()
        {
            NationalDocument = account.NationalDocument,
            Name = account.Name,
            NumberAccount = account.NumberAccount,
        };
    }
}
