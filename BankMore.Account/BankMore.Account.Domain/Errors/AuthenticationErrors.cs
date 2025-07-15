using BankMore.Core.Domain.Primitives;

namespace BankMore.Account.Domain.Errors;

public class AuthenticationErrors
{
    public static Error NotAuthorized => Error.AccessUnAuthorized("USER_UNAUTHORIZED", "Acesso não autorizado");
}
