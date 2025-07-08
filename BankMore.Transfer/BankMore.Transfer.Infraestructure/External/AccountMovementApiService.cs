using BankMore.Core.Domain.Primitives;
using BankMore.Core.Infraestructure.External;
using BankMore.Transfer.Domain.External.Account;
using BankMore.Transfer.Domain.External.Account.Request;
using BankMore.Transfer.Domain.External.Account.Response;

namespace BankMore.Transfer.Infraestructure.External;

internal sealed class AccountMovementApiService(HttpClient httpClient) : BaseApiService(httpClient), IAccountMovementApiService
{
    private const string Resource = "v1/movement";
    public async Task<Result<AccoutMovementResponseApi>> RegisterMovementAsync(AccountMovementRequestApi request) =>
        await PostAsync<AccountMovementRequestApi, AccoutMovementResponseApi, AccountErrorResponseApi>(Resource, request);

}
