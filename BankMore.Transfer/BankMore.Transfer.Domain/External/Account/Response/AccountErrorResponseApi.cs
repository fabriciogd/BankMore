using BankMore.Core.Infraestructure.External.Models;

namespace BankMore.Transfer.Domain.External.Account.Response;

public record AccountErrorResponseApi(string Code, string Description) : IErrorResponse
{
    public string GetErrorCode() => Code;
    public string GetErrorDescription() => Description;
}
