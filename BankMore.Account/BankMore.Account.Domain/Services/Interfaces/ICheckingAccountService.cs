using BankMore.Account.Domain.Entities;
using BankMore.Core.Domain.Primitives;

namespace BankMore.Account.Domain.Services.Interfaces;

public interface ICheckingAccountService
{
    Task<Result<CheckingAccount>> GetValidCheckingAccountAsync(int numberAccount);
}
