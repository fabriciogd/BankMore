using FluentValidation;

namespace BankMore.Transfer.Application.UseCases.Transfer.Create;

public class TransferCreateRequestValidator : AbstractValidator<TransferCreateRequest>
{
    public TransferCreateRequestValidator()
    {
        RuleFor(a => a.Value)
            .GreaterThan(0)
                .WithErrorCode("INVALID_VALUE")
                .WithMessage("Valor precisa ser maior que 0");
    }
}
