namespace BankMore.Account.Application.UseCases.Authentication.Login;

public sealed record AuthenticationLoginResponse
{
    public string Token { get; set; }
}
