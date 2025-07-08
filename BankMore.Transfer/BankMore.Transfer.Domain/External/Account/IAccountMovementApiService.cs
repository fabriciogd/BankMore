using BankMore.Core.Domain.Primitives;
using BankMore.Transfer.Domain.External.Account.Request;
using BankMore.Transfer.Domain.External.Account.Response;

namespace BankMore.Transfer.Domain.External.Account;

public interface IAccountMovementApiService
{
    Task<Result<AccoutMovementResponseApi>> RegisterMovementAsync(AccountMovementRequestApi request);
}
