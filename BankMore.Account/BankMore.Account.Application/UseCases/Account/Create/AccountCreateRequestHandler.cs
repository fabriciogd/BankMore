using BankMore.Account.Domain.Entities;
using BankMore.Account.Domain.Repositories;
using BankMore.Core.Domain.Primitives;
using BankMore.Core.Domain.Resources;
using BankMore.Core.Infraestructure.Security;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BankMore.Account.Application.UseCases.Account.Create;

internal sealed class AccountCreateRequestHandler(ICheckingAccountRepository repository, ILogger<AccountCreateRequestHandler> logger)
    : IRequestHandler<AccountCreateRequest, Result<AccountCreateResponse>>
{
    public async Task<Result<AccountCreateResponse>> Handle(AccountCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation(LogTemplate.StartHandler, nameof(AccountCreateRequestHandler));

        var hashedPassword = PasswordHasher.HashPassword(request.Password, out string salt);

        var account = CheckingAccount.Create(request.NationalDocument, request.Name, hashedPassword, salt);

        await repository.AddAsync(account, cancellationToken);

        logger.LogInformation(LogTemplate.EndHandler, nameof(AccountCreateRequestHandler), string.Empty);

        return new AccountCreateResponse()
        {
            NationalDocument = account.NationalDocument,
            Name = account.Name,
            NumberAccount = account.NumberAccount,
        };
    }
}
