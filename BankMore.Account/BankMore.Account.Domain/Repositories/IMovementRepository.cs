using BankMore.Account.Domain.Entities;
using BankMore.Core.Domain.Contracts;

namespace BankMore.Account.Domain.Repositories;

public interface IMovementRepository : IRepository<MovementAccount>
{
    Task<decimal> GetBalanceAsync(int accountId);
}
