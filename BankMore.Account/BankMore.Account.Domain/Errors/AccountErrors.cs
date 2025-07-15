using BankMore.Core.Domain.Primitives;

namespace BankMore.Account.Domain.Errors;

public class AccountErrors
{
    public static Error InvalidDocument => Error.Validation("INVALID_DOCUMENT", "Documento inválido");

    public static Error NotFound => Error.NotFound("NOT_FOUND", "Conta não encontrada");

    public static Error IsInactive => Error.NotFound("INACTIVE_ACCOUNT", "Conta inativa");
}
