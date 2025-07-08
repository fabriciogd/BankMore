using BankMore.Core.Domain.Primitives;
using MediatR;

namespace BankMore.Account.Application.UseCases.Account.Create;

public sealed record AccountCreateRequest : IRequest<Result<AccountCreateResponse>>
{
    public string? NationalDocument { get; set; }

    public string? Name { get; set; }

    public string? Password { get; set; }
}
