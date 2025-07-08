using BankMore.Core.Domain.Primitives;
using MediatR;

namespace BankMore.Account.Application.UseCases.Authentication.Login;

public sealed record AuthenticationLoginRequest : IRequest<Result<AuthenticationLoginResponse>>
{
    public string? Document { get; set; }

    public string? Password { get; set; }
}
