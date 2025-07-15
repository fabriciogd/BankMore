using BankMore.Core.Domain.Primitives;

namespace BankMore.Account.Domain.Errors;

public class MovementErrors
{
    public static Error InvalidType => Error.Validation("INVALID_TYPE", "Tipo de movimento inválido");
    public static Error InvalidValue => Error.Validation("INVALID_VALUE", "Valor inválido");

}
