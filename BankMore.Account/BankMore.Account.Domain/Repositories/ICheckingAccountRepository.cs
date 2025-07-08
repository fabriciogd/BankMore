using BankMore.Account.Domain.Entities;
using BankMore.Core.Domain.Contracts;

namespace BankMore.Account.Domain.Repositories;

public interface ICheckingAccountRepository : IRepository<CheckingAccount>
{
    Task<CheckingAccount?> GetByDocumentAsync(string? document);

    Task<CheckingAccount?> GetByNumberAccountAsync(int numberAccount);
}
