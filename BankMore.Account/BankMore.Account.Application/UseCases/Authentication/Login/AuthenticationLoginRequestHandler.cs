using BankMore.Account.Application.Errors;
using BankMore.Account.Domain.Repositories;
using BankMore.Core.Application.Iterfaces;
using BankMore.Core.Domain.Primitives;
using BankMore.Core.Infraestructure.Security;
using MediatR;

namespace BankMore.Account.Application.UseCases.Authentication.Login;

internal sealed class AuthenticationLoginRequestHandler(
    ICheckingAccountRepository _repository, 
    IJwtTokenGenerator tokenGenerator) : IRequestHandler<AuthenticationLoginRequest, Result<AuthenticationLoginResponse>>
{
    public async Task<Result<AuthenticationLoginResponse>> Handle(AuthenticationLoginRequest request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetByDocumentAsync(request.Document);

        if (account is null)
            return AccountErrors.NotFound;

        if (!account.IsActive)
            return AccountErrors.IsInactive;

        var validPassword = PasswordHasher.VerifyPassword(request.Password, account.Password, account.Salt);

        if (!validPassword)
            return AuthenticationErrors.NotAuthorized;

        var token = tokenGenerator.GenerateToken(account.NumberAccount);

        return new AuthenticationLoginResponse() { Token = token };
    }
}
