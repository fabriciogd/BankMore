namespace BankMore.Account.Application.UseCases.Account.GetActive;

public sealed record AccountGetActiveResponse
{
    public int CheckingAccountId { get; set; }

    public string? NationalDocument { get; set; }

    public string? Name { get; set; }

    public int NumberAccount { get; set; }
}
