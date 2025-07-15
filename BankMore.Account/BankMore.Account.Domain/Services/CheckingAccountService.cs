using BankMore.Account.Domain.Entities;
using BankMore.Account.Domain.Errors;
using BankMore.Account.Domain.Repositories;
using BankMore.Account.Domain.Services.Interfaces;
using BankMore.Core.Domain.Primitives;

namespace BankMore.Account.Domain.Services;

public sealed class CheckingAccountService(ICheckingAccountRepository repository) : ICheckingAccountService
{
    public async Task<Result<CheckingAccount>> GetValidCheckingAccountAsync(int numberAccount)
    {
        var checkingAccount = await repository.GetByNumberAccountAsync(numberAccount);

        if (checkingAccount is null)
            return AccountErrors.NotFound;

        if (!checkingAccount.IsActive)
            return AccountErrors.IsInactive;

        return checkingAccount;
    }
}
