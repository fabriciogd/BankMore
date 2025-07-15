using BankMore.Core.Domain.Primitives;

namespace BankMore.Transfer.Domain.Errors;

public class TransferErrors
{
    public static Error InvalidDestinationAccount => Error.Validation("INVALID_ACCOUNT", "Conta de destino inválida");
    public static Error InvalidValue => Error.Validation("INVALID_VALUE", "Valor inválido");
}
