namespace BankMore.Account.Application.UseCases.Account.Create;

public sealed record AccountCreateResponse
{
    public string? NationalDocument { get; set; }

    public string? Name { get; set; }

    public int NumberAccount { get; set; }
}
