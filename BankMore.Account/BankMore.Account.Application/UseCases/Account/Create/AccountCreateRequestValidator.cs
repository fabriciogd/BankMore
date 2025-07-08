using BankMore.Account.Domain.Validators;
using FluentValidation;

namespace BankMore.Account.Application.UseCases.Account.Create;

public class AccountCreateRequestValidator : AbstractValidator<AccountCreateRequest>
{
    public AccountCreateRequestValidator()
    {
        RuleFor(a => a.NationalDocument)
            .NotEmpty()
                .WithErrorCode("INVALID_DOCUMENT")
                .WithMessage("Documento não pode ser vazio")
            .Must(DocumentValidator.IsValid)
                .WithErrorCode("INVALID_DOCUMENT")
                .WithMessage("Documento não é valido");

        RuleFor(a => a.Name)
            .NotEmpty()
                .WithErrorCode("INVALID_NAME")
                .WithMessage("Nome não pode ser vazio");

        RuleFor(a => a.Password)
            .NotEmpty()
                .WithErrorCode("INVALID_PASSWORD")
                .WithMessage("Senha não pode ser vazio");
    }
}
