using BankMore.Core.Domain.Primitives;
using BankMore.Transfer.Domain.External.Account.Response;

namespace BankMore.Transfer.Domain.External.Account;

public interface IAccountApiService
{
    Task<Result<AccountResponseApi>> GetActiveAsync(
        int numberAccount,
        CancellationToken cancellationToken);
}
