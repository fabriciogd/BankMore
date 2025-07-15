namespace BankMore.Transfer.Domain.External.Account.Response;

public sealed class AccountResponseApi
{
    public int CheckingAccountId { get; set; }

    public string? NationalDocument { get; set; }

    public string? Name { get; set; }

    public int NumberAccount { get; set; }
}
