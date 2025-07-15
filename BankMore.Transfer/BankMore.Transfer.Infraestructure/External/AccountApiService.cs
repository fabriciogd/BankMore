using BankMore.Core.Domain.Primitives;
using BankMore.Core.Infraestructure.External;
using BankMore.Transfer.Domain.External.Account;
using BankMore.Transfer.Domain.External.Account.Response;

namespace BankMore.Transfer.Infraestructure.External;

public sealed class AccountApiService(HttpClient httpClient) : BaseApiService(httpClient), IAccountApiService
{
    private const string Resource = "v1/account";

    public async Task<Result<AccountResponseApi>> GetActiveAsync(int numberAccount, CancellationToken cancellationToken) =>
        await GetAsync<AccountResponseApi, AccountErrorResponseApi>($"{Resource}/{numberAccount}", cancellationToken);
}
