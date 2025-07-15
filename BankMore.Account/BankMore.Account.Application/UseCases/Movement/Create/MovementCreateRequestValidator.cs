using FluentValidation;

namespace BankMore.Account.Application.UseCases.Movement.Create;

public class MovementCreateRequestValidator : AbstractValidator<MovementCreateRequest>
{
    public MovementCreateRequestValidator()
    {
        RuleFor(a => a.Type)
            .Must(t => t == 'C' || t == 'D')
                .WithErrorCode("INVALID_TYPE")
                .WithMessage("Tipo precisa ser 'C' ou 'D'");

        RuleFor(a => a.Value)
            .GreaterThan(0)
                .WithErrorCode("INVALID_VALUE")
                .WithMessage("Valor precisa ser maior que 0");
    }
}
