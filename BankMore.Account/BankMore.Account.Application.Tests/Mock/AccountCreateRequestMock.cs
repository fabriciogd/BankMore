using BankMore.Account.Application.UseCases.Account.Create;
using Bogus;
using Bogus.Extensions.Brazil;

namespace BankMore.Account.Application.Tests.Mock;

public static class AccountCreateRequestMock
{
    private static Faker<AccountCreateRequest> Faker() => new Faker<AccountCreateRequest>()
        .RuleFor(r => r.Name, f => f.Person.FullName)
        .RuleFor(r => r.NationalDocument, f => f.Person.Cpf())
        .RuleFor(r => r.Password, f => f.Random.AlphaNumeric(5));

    public static AccountCreateRequest GetDefaultInstance() => Faker().Generate();
}
